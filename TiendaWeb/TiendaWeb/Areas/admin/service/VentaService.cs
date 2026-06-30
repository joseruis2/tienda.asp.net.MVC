using Microsoft.EntityFrameworkCore;
using TiendaWeb.Areas.admin.repository;
using TiendaWeb.Areas.admin.ViewModel;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.service
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _repo;
        private readonly AppDbContext _db;

        public VentaService(IVentaRepository repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        public async Task<PosViewModel> GetPosDataAsync()
        {
            var config = await _db.ConfiguracionNegocios.FirstOrDefaultAsync();

            var productos = await _db.Productos
                .Where(p => p.Estado == true && p.StockActual > 0)
                .Select(p => new ProductoPosDto
                {
                    ProductoId = p.ProductoId,
                    Codigo = p.Codigo,
                    CodigoBarras = p.CodigoBarras,
                    Nombre = p.Nombre,
                    Precio = p.Precio ?? 0,
                    PrecioMayorista = p.PrecioMayorista ?? 0,
                    StockActual = p.StockActual ?? 0,
                    UnidadMedida = p.UnidadMedida ?? "Unidad"
                })
                .ToListAsync();

            var clientes = await _db.Clientes
                .Where(c => c.Estado == true)
                .Select(c => new SelectItemViewModel
                {
                    Id = c.ClienteId,
                    Nombre = (c.RazonSocial != null && c.RazonSocial != "")
                        ? c.RazonSocial
                        : $"{c.Nombres} {c.Apellidos}".Trim()
                })
                .ToListAsync();

            return new PosViewModel
            {
                Productos = productos,
                Clientes = clientes,
                SerieTicket = config?.SerieTicket ?? "T001",
                SerieBoleta = config?.SerieBoleta ?? "B001",
                SerieFactura = config?.SerieFactura ?? "F001",
                IgvPorcentaje = config?.IgvPorcentaje ?? 18
            };
        }

        public async Task<VentasHistorialViewModel> GetHistorialAsync(
            DateTime? fecha, string? estado, string? metodo)
        {
            var lista = await _repo.GetAllWithRelationsAsync(fecha, estado, metodo);

            var vms = lista.Select(v => new VentaListViewModel
            {
                VentaId = v.VentaId,
                NumeroTicket = v.NumeroTicket,
                TipoComprobante = v.TipoComprobante ?? "",
                ClienteNombre = v.ClienteNombre,
                ClienteDocumento = v.ClienteDocumento,
                Subtotal = v.Subtotal ?? 0,
                Igv = v.Igv ?? 0,
                Descuento = v.Descuento ?? 0,
                Total = v.Total,
                MetodoPago = v.MetodoPago ?? "",
                MontoPagado = v.MontoPagado ?? 0,
                Vuelto = v.Vuelto ?? 0,
                Estado = v.Estado ?? "",
                FechaVenta = v.FechaVenta ?? DateTime.Now,
                UsuarioNombre = v.Usuario != null
                    ? $"{v.Usuario.Nombres} {v.Usuario.Apellidos}" : ""
            }).ToList();

            var completadas = vms.Where(v => v.Estado == "COMPLETADA").ToList();

            return new VentasHistorialViewModel
            {
                Lista = vms,
                FiltroFecha = fecha?.ToString("yyyy-MM-dd"),
                FiltroEstado = estado,
                FiltroMetodo = metodo,
                TotalDia = completadas.Sum(v => v.Total),
                TransaccionesDia = completadas.Count
            };
        }

        public async Task<VentaDetalleViewModel?> GetDetalleAsync(int id)
        {
            var v = await _repo.GetByIdWithDetalleAsync(id);
            if (v == null) return null;

            return new VentaDetalleViewModel
            {
                VentaId = v.VentaId,
                NumeroTicket = v.NumeroTicket,
                TipoComprobante = v.TipoComprobante ?? "",
                Serie = v.Serie ?? "",
                NumeroCorrelativo = v.NumeroCorrelativo,
                ClienteNombre = v.ClienteNombre,
                ClienteDocumento = v.ClienteDocumento,
                Subtotal = v.Subtotal ?? 0,
                Igv = v.Igv ?? 0,
                Descuento = v.Descuento ?? 0,
                Total = v.Total,
                MetodoPago = v.MetodoPago ?? "",
                MontoPagado = v.MontoPagado ?? 0,
                Vuelto = v.Vuelto ?? 0,
                Estado = v.Estado ?? "",
                FechaVenta = v.FechaVenta ?? DateTime.Now,
                UsuarioNombre = v.Usuario != null
                    ? $"{v.Usuario.Nombres} {v.Usuario.Apellidos}" : "",
                Observaciones = v.Observaciones,
                MotivoAnulacion = v.MotivoAnulacion,
                FechaAnulacion = v.FechaAnulacion,
                Items = v.DetalleVenta.Select(d => new VentaDetalleItemViewModel
                {
                    NombreProducto = d.NombreProducto,
                    CodigoBarras = d.CodigoBarras,
                    Cantidad = (decimal)d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Descuento = d.Descuento ?? 0,
                    Subtotal = d.Subtotal,
                    Total = d.Total
                }).ToList()
            };
        }

        public async Task<(bool Success, string Message, int? VentaId)> CreateAsync(
            VentaCreateViewModel vm, int usuarioId)
        {
            if (!vm.Items.Any())
                return (false, "La venta debe tener al menos un producto.", null);

            // Obtener sesión activa
            var sesion = await _db.SesionesCajas
                .FirstOrDefaultAsync(s => s.Estado == "ABIERTA");
            

            var config = await _db.ConfiguracionNegocios.FirstOrDefaultAsync();
            var igvPct = (config?.IgvPorcentaje ?? 18) / 100m;

            // Serie según tipo comprobante
            var serie = vm.TipoComprobante switch
            {
                "BOLETA" => config?.SerieBoleta ?? "B001",
                "FACTURA" => config?.SerieFactura ?? "F001",
                _ => config?.SerieTicket ?? "T001"
            };

            var correlativo = await _repo.GetNextCorrelativoAsync(serie);
            var numeroTicket = $"{serie}-{correlativo:D8}";

            // Calcular totales
            decimal subtotalBruto = 0;
            var detalles = new List<DetalleVenta>();

            foreach (var item in vm.Items)
            {
                var subtotalItem = Math.Round(item.Cantidad * item.PrecioUnitario, 2);
                var descItem = Math.Round(item.Descuento, 2);
                var totalItem = subtotalItem - descItem;
                var igvItem = Math.Round(totalItem - totalItem / (1 + igvPct), 2);

                subtotalBruto += totalItem;

                detalles.Add(new DetalleVenta
                {
                    ProductoId = item.ProductoId,
                    CodigoBarras = item.CodigoBarras,
                    NombreProducto = item.NombreProducto,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.PrecioUnitario,
                    Descuento = descItem,
                    Subtotal = subtotalItem,
                    Igv = igvItem,
                    Total = totalItem
                });
            }

            var descuento = Math.Round(vm.Descuento, 2);
            var total = Math.Round(subtotalBruto - descuento, 2);
            var igv = Math.Round(total - total / (1 + igvPct), 2);
            var subtotal = total - igv;
            var vuelto = Math.Max(0, Math.Round(vm.MontoPagado - total, 2));

            // Obtener cliente
            string? clienteNombre = vm.ClienteNombre;
            string? clienteDocumento = vm.ClienteDocumento;
            if (vm.ClienteId.HasValue)
            {
                var cli = await _db.Clientes.FindAsync(vm.ClienteId.Value);
                if (cli != null)
                {
                    clienteNombre = !string.IsNullOrEmpty(cli.RazonSocial)
                        ? cli.RazonSocial
                        : $"{cli.Nombres} {cli.Apellidos}".Trim();
                    clienteDocumento = cli.NumeroDocumento;

                    // Actualizar total compras
                    cli.TotalCompras = (cli.TotalCompras ?? 0) + total;
                }
            }

            var venta = new Venta
            {
                NumeroTicket = numeroTicket,
                TipoComprobante = vm.TipoComprobante,
                Serie = serie,
                NumeroCorrelativo = correlativo,
                FechaVenta = DateTime.Now,
                ClienteId = vm.ClienteId,
                ClienteNombre = clienteNombre ?? "Consumidor Final",
                ClienteDocumento = clienteDocumento,
                Subtotal = subtotal,
                Igv = igv,
                Descuento = descuento,
                Total = total,
                MetodoPago = vm.MetodoPago,
                MontoPagado = vm.MontoPagado,
                Vuelto = vuelto,
                UsuarioId = usuarioId,
                CajaId = sesion?.CajaId,
                SesionId = sesion?.SesionId,
                Estado = "COMPLETADA",
                Observaciones = vm.Observaciones
            };

            await _repo.CreateAsync(venta, detalles);

            // Actualizar correlativo en configuración
            if (config != null)
            {
                switch (vm.TipoComprobante)
                {
                    case "BOLETA": config.CorrelativoBoleta = correlativo + 1; break;
                    case "FACTURA": config.CorrelativoFactura = correlativo + 1; break;
                    default: config.CorrelativoTicket = correlativo + 1; break;
                }
                await _db.SaveChangesAsync();
            }

            return (true, $"Venta {numeroTicket} registrada correctamente.", venta.VentaId);
        }

        public async Task<(bool Success, string Message)> AnularAsync(
            AnularVentaViewModel vm, int usuarioId)
        {
            try
            {
                await _repo.AnularAsync(vm.VentaId, vm.MotivoAnulacion, usuarioId);
                return (true, "Venta anulada correctamente.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
