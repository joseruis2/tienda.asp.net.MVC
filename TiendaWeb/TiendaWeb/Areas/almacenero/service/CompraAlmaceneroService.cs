using TiendaWeb.Areas.almacenero.repository;
using TiendaWeb.Areas.almacenero.ViewModel;
using TiendaWeb.Infrastructure.Data;

namespace TiendaWeb.Areas.almacenero.service
{
    public class CompraAlmaceneroService : ICompraAlmaceneroService
    {
        private readonly ICompraAlmaceneroRepository _repo;
        private readonly AppDbContext _db;

        public CompraAlmaceneroService(
            ICompraAlmaceneroRepository repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        public async Task<ComprasAlmaceneroIndexViewModel> GetIndexDataAsync(string? estado)
        {
            var lista = await _repo.GetAllAsync(estado);

            var vms = lista.Select(c => new CompraAlmaceneroListViewModel
            {
                CompraId = c.CompraId,
                NumeroOrden = c.NumeroOrden,
                ProveedorNombre = c.Proveedor?.RazonSocial ?? "",
                TipoDocProveedor = c.TipoDocProveedor,
                NroDocProveedor = c.NroDocProveedor,
                FechaCompra = c.FechaCompra ?? DateTime.Now,
                FechaEntrega = c.FechaEntrega,
                Total = c.Total,
                Estado = c.Estado ?? "PENDIENTE",
                UsuarioNombre = c.Usuario != null
                    ? $"{c.Usuario.Nombres} {c.Usuario.Apellidos}" : ""
            }).ToList();

            return new ComprasAlmaceneroIndexViewModel
            {
                Lista = vms,
                Pendientes = vms.Where(c => c.PuedeRecibir).ToList(),
                FiltroEstado = estado,
                CountPendientes = vms.Count(c => c.Estado == "PENDIENTE"),
                CountParciales = vms.Count(c => c.Estado == "PARCIAL")
            };
        }

        public async Task<RecepcionAlmaceneroViewModel?> GetRecepcionDataAsync(int id)
        {
            var compra = await _repo.GetByIdWithDetalleAsync(id);
            if (compra == null || compra.Estado == "ANULADA" ||
                compra.Estado == "RECIBIDA") return null;

            var itemsPendientes = compra.DetalleCompras
                .Where(d => d.Cantidad > (d.CantidadRecibida ?? 0))
                .ToList();

            var items = new List<RecepcionAlmaceneroItemViewModel>();
            foreach (var d in itemsPendientes)
            {
                var producto = await _db.Productos.FindAsync(d.ProductoId);
                items.Add(new RecepcionAlmaceneroItemViewModel
                {
                    DetalleId = d.DetalleId,
                    ProductoId = d.ProductoId,
                    NombreProducto = d.NombreProducto,
                    Ubicacion = producto?.Ubicacion,
                    StockActual = producto?.StockActual ?? 0,
                    Cantidad = d.Cantidad,
                    CantidadRecibida = d.CantidadRecibida ?? 0,
                    CantidadARecibir = d.Cantidad - (d.CantidadRecibida ?? 0)
                });
            }

            return new RecepcionAlmaceneroViewModel
            {
                CompraId = compra.CompraId,
                NumeroOrden = compra.NumeroOrden,
                Proveedor = compra.Proveedor?.RazonSocial ?? "",
                FechaEntrega = compra.FechaEntrega,
                Items = items
            };
        }

        public async Task<CompraAlmaceneroDetalleViewModel?> GetDetalleAsync(int id)
        {
            var c = await _repo.GetByIdWithDetalleAsync(id);
            if (c == null) return null;

            return new CompraAlmaceneroDetalleViewModel
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
                Total = c.Total,
                Estado = c.Estado ?? "PENDIENTE",
                Observaciones = c.Observaciones,
                Items = c.DetalleCompras.Select(d => new CompraAlmaceneroItemViewModel
                {
                    DetalleId = d.DetalleId,
                    ProductoId = d.ProductoId,
                    NombreProducto = d.NombreProducto,
                    Cantidad = d.Cantidad,
                    CantidadRecibida = d.CantidadRecibida ?? 0,
                    PrecioUnitario = d.PrecioUnitario,
                    Total = d.Total
                }).ToList()
            };
        }

        public async Task<(bool Success, string Message)> RecibirAsync(
            RecepcionAlmaceneroViewModel vm, int usuarioId)
        {
            var items = vm.Items
                .Select(i => (i.DetalleId, i.ProductoId, i.CantidadARecibir))
                .ToList();

            return await _repo.RecibirAsync(vm.CompraId, items, usuarioId);
        }
    }
}
