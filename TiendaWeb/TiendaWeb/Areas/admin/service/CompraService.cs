using Microsoft.EntityFrameworkCore;
using TiendaWeb.Areas.admin.repository;
using TiendaWeb.Areas.admin.ViewModel;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.service
{
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _repo;
        private readonly AppDbContext _db;

        public CompraService(ICompraRepository repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        public async Task<ComprasIndexViewModel> GetIndexDataAsync(string? estado)
        {
            var lista = await _repo.GetAllWithRelationsAsync(estado);
            var config = await _db.ConfiguracionNegocios.FirstOrDefaultAsync();
            var igvPct = (config?.IgvPorcentaje ?? 18) / 100m;

            var vms = lista.Select(c => new CompraListViewModel
            {
                CompraId = c.CompraId,
                NumeroOrden = c.NumeroOrden,
                ProveedorNombre = c.Proveedor?.RazonSocial ?? "",
                TipoDocProveedor = c.TipoDocProveedor,
                NroDocProveedor = c.NroDocProveedor,
                FechaCompra = c.FechaCompra ?? DateTime.Now,
                FechaEntrega = c.FechaEntrega,
                Total = c.Total,
                CondicionPago = c.CondicionPago ?? "CONTADO",
                FechaVencimiento = c.FechaVencimiento,
                Estado = c.Estado ?? "PENDIENTE",
                UsuarioNombre = c.Usuario != null
                    ? $"{c.Usuario.Nombres} {c.Usuario.Apellidos}" : ""
            }).ToList();

            var proveedores = await _db.Proveedores
                .Where(p => p.Estado == true)
                .Select(p => new SelectItemViewModel
                { Id = p.ProveedorId, Nombre = p.RazonSocial })
                .ToListAsync();

            var productos = await _db.Productos
                .Where(p => p.Estado == true)
                .Select(p => new ProductoPosDto
                {
                    ProductoId = p.ProductoId,
                    Codigo = p.Codigo,
                    CodigoBarras = p.CodigoBarras,
                    Nombre = p.Nombre,
                    Precio = p.PrecioCosto ?? 0,
                    StockActual = p.StockActual ?? 0,
                    UnidadMedida = p.UnidadMedida ?? "Unidad"
                })
                .ToListAsync();

            var pendientes = vms.Where(c => c.Estado == "PENDIENTE"
                                         || c.Estado == "PARCIAL").ToList();

            return new ComprasIndexViewModel
            {
                Lista = vms,
                Proveedores = proveedores,
                Productos = productos,
                FiltroEstado = estado,
                TotalPendiente = pendientes.Sum(c => c.Total),
                CountPendiente = pendientes.Count
            };
        }

        public async Task<CompraDetalleViewModel?> GetDetalleAsync(int id)
        {
            var c = await _repo.GetByIdWithDetalleAsync(id);
            if (c == null) return null;

            return new CompraDetalleViewModel
            {
                CompraId = c.CompraId,
                NumeroOrden = c.NumeroOrden,
                ProveedorNombre = c.Proveedor?.RazonSocial ?? "",
                TipoDocProveedor = c.TipoDocProveedor,
                NroDocProveedor = c.NroDocProveedor,
                FechaCompra = c.FechaCompra ?? DateTime.Now,
                FechaEntrega = c.FechaEntrega,
                Subtotal = c.Subtotal ?? 0,
                Igv = c.Igv ?? 0,
                Descuento = c.Descuento ?? 0,
                Total = c.Total,
                CondicionPago = c.CondicionPago ?? "CONTADO",
                FechaVencimiento = c.FechaVencimiento,
                Estado = c.Estado ?? "PENDIENTE",
                UsuarioNombre = c.Usuario != null
                    ? $"{c.Usuario.Nombres} {c.Usuario.Apellidos}" : "",
                Observaciones = c.Observaciones,
                Items = c.DetalleCompras.Select(d => new CompraDetalleItemViewModel
                {
                    DetalleId = d.DetalleId,
                    NombreProducto = d.NombreProducto,
                    Cantidad = d.Cantidad,
                    CantidadRecibida = d.CantidadRecibida ?? 0,
                    PrecioUnitario = d.PrecioUnitario,
                    Descuento = d.Descuento ?? 0,
                    Total = d.Total
                }).ToList()
            };
        }

        public async Task<RecepcionCompraViewModel?> GetRecepcionDataAsync(int id)
        {
            var c = await _repo.GetByIdWithDetalleAsync(id);
            if (c == null || c.Estado == "ANULADA" || c.Estado == "RECIBIDA")
                return null;

            return new RecepcionCompraViewModel
            {
                CompraId = c.CompraId,
                NumeroOrden = c.NumeroOrden,
                Proveedor = c.Proveedor?.RazonSocial ?? "",
                Items = c.DetalleCompras
                    .Where(d => d.Cantidad > (d.CantidadRecibida ?? 0))
                    .Select(d => new RecepcionItemViewModel
                    {
                        DetalleId = d.DetalleId,
                        ProductoId = d.ProductoId,
                        NombreProducto = d.NombreProducto,
                        Cantidad = d.Cantidad,
                        CantidadRecibida = d.CantidadRecibida ?? 0,
                        CantidadARecibir = d.Cantidad - (d.CantidadRecibida ?? 0)
                    }).ToList()
            };
        }

        public async Task<(bool Success, string Message)> CreateAsync(
            CompraCreateViewModel vm, int usuarioId)
        {
            if (!vm.Items.Any())
                return (false, "La orden debe tener al menos un producto.");

            var serie = "OC01";
            var correlativo = await _repo.GetNextCorrelativoAsync(serie);
            var numero = $"{serie}-{correlativo:D8}";
            var config = await _db.ConfiguracionNegocios.FirstOrDefaultAsync();
            var igvPct = (config?.IgvPorcentaje ?? 18) / 100m;

            decimal subtotalBruto = 0;
            var detalles = new List<DetalleCompra>();

            foreach (var item in vm.Items)
            {
                var producto = await _db.Productos.FindAsync(item.ProductoId);
                var subtItem = Math.Round(item.Cantidad * item.PrecioUnitario, 2);
                var descItem = Math.Round(item.Descuento, 2);
                var totalItem = subtItem - descItem;
                var igvItem = Math.Round(totalItem - totalItem / (1 + igvPct), 2);

                subtotalBruto += totalItem;

                detalles.Add(new DetalleCompra
                {
                    ProductoId = item.ProductoId,
                    NombreProducto = producto?.Nombre ?? item.NombreProducto,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.PrecioUnitario,
                    Descuento = descItem,
                    Subtotal = subtItem,
                    Igv = igvItem,
                    Total = totalItem,
                    CantidadRecibida = 0
                });
            }

            var descuento = Math.Round(vm.Descuento, 2);
            var total = Math.Round(subtotalBruto - descuento, 2);
            var igv = Math.Round(total - total / (1 + igvPct), 2);
            var subtotal = total - igv;

            var compra = new Compra
            {
                NumeroOrden = numero,
                Serie = serie,
                NumeroCorrelativo = correlativo,
                ProveedorId = vm.ProveedorId,
                TipoDocProveedor = vm.TipoDocProveedor,
                NroDocProveedor = vm.NroDocProveedor,
                FechaCompra = DateTime.Now,
                FechaEntrega = vm.FechaEntrega,
                Subtotal = subtotal,
                Igv = igv,
                Descuento = descuento,
                Total = total,
                CondicionPago = vm.CondicionPago,
                FechaVencimiento = vm.FechaVencimiento,
                Estado = "PENDIENTE",
                UsuarioId = usuarioId,
                Observaciones = vm.Observaciones
            };

            await _repo.CreateAsync(compra, detalles);
            return (true, $"Orden {numero} creada correctamente.");
        }

        public async Task<(bool Success, string Message)> RecibirAsync(
            RecepcionCompraViewModel vm, int usuarioId)
        {
            var recepciones = vm.Items
                .Select(i => (i.DetalleId, i.CantidadARecibir))
                .ToList();

            return await _repo.RecibirAsync(vm.CompraId, recepciones, usuarioId);
        }

        public async Task<(bool Success, string Message)> AnularAsync(
            AnularCompraViewModel vm, int usuarioId)
        {
            return await _repo.AnularAsync(vm.CompraId, usuarioId);
        }
    }
}
