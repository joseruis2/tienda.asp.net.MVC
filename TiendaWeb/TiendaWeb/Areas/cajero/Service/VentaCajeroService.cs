using Microsoft.EntityFrameworkCore;
using TiendaWeb.Areas.cajero.Repository;
using TiendaWeb.Areas.cajero.ViewModel;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.cajero.Service
{
    public class VentaCajeroService : IVentaCajeroService
    {
        private readonly IVentaCajeroRepository _repo;
        private readonly AppDbContext _db;

        public VentaCajeroService(IVentaCajeroRepository repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        public async Task<PosCajeroViewModel> GetPosDataAsync(int usuarioId)
        {
            var sesion = await _repo.GetSesionAbiertaByUsuarioAsync(usuarioId);
            var config = await _db.ConfiguracionNegocios.FirstOrDefaultAsync();

            var productos = await _db.Productos
                .Where(p => p.Estado == true && p.StockActual > 0)
                .Select(p => new ProductoPosCajeroDto
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
                .Select(c => new SelectCajeroItem
                {
                    Id = c.ClienteId,
                    Nombre = (c.RazonSocial != null && c.RazonSocial != "")
                        ? c.RazonSocial
                        : $"{c.Nombres} {c.Apellidos}".Trim()
                })
                .ToListAsync();

            return new PosCajeroViewModel
            {
                Productos = productos,
                Clientes = clientes,
                SerieTicket = config?.SerieTicket ?? "T001",
                SerieBoleta = config?.SerieBoleta ?? "B001",
                SerieFactura = config?.SerieFactura ?? "F001",
                IgvPorcentaje = config?.IgvPorcentaje ?? 18,
                TieneSesionAbierta = sesion != null,
                CajaNombre = sesion?.Caja?.Nombre,
                CajaNumero = sesion?.Caja?.Numero
            };
        }

        public async Task<(bool Success, string Message, int? VentaId)> CreateAsync(
            VentaCajeroCreateViewModel vm, int usuarioId)
        {
            if (!vm.Items.Any())
                return (false, "La venta debe tener al menos un producto.", null);

            var sesion = await _repo.GetSesionAbiertaByUsuarioAsync(usuarioId);
            if (sesion == null)
                return (false, "No tienes una caja abierta. Abre una caja primero.", null);

            var config = await _db.ConfiguracionNegocios.FirstOrDefaultAsync();
            var igvPct = (config?.IgvPorcentaje ?? 18) / 100m;

            var serie = vm.TipoComprobante switch
            {
                "BOLETA" => config?.SerieBoleta ?? "B001",
                "FACTURA" => config?.SerieFactura ?? "F001",
                _ => config?.SerieTicket ?? "T001"
            };

            var correlativo = await _repo.GetNextCorrelativoAsync(serie);
            var numeroTicket = $"{serie}-{correlativo:D8}";

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
                CajaId = sesion.CajaId,
                SesionId = sesion.SesionId,
                Estado = "COMPLETADA",
                Observaciones = vm.Observaciones
            };

            await _repo.CreateAsync(venta, detalles);

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
    }
}
