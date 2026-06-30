using Microsoft.EntityFrameworkCore;
using TiendaWeb.Areas.admin.repository;
using TiendaWeb.Areas.admin.ViewModel;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.service
{
    public class DevolucionService : IDevolucionService
    {
        private readonly IDevolucionRepository _repo;
        private readonly AppDbContext _db;

        public DevolucionService(IDevolucionRepository repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        public async Task<DevolucionesIndexViewModel> GetIndexDataAsync(DateTime? fecha)
        {
            var lista = await _repo.GetAllWithRelationsAsync(fecha);
            var vms = lista.Select(d => new DevolucionListViewModel
            {
                DevolucionId = d.DevolucionId,
                Numero = d.Numero,
                VentaId = d.VentaId,
                NumeroTicket = d.Venta?.NumeroTicket ?? "",
                ClienteNombre = d.Venta?.ClienteNombre,
                Fecha = d.Fecha ?? DateTime.Now,
                Motivo = d.Motivo,
                TotalDevuelto = d.TotalDevuelto,
                TipoDevolucion = d.TipoDevolucion ?? "EFECTIVO",
                Estado = d.Estado ?? "PROCESADA",
                UsuarioNombre = d.Usuario != null
                    ? $"{d.Usuario.Nombres} {d.Usuario.Apellidos}" : ""
            }).ToList();

            var procesadas = vms.Where(d => d.Estado == "PROCESADA").ToList();

            return new DevolucionesIndexViewModel
            {
                Lista = vms,
                FiltroFecha = fecha?.ToString("yyyy-MM-dd"),
                TotalProcesadas = procesadas.Count,
                MontoDevuelto = procesadas.Sum(d => d.TotalDevuelto)
            };
        }

        public async Task<VentaParaDevolucionDto?> BuscarVentaAsync(string numeroTicket)
        {
            var venta = await _db.Ventas
                .Include(v => v.DetalleVenta)
                .FirstOrDefaultAsync(v => v.NumeroTicket == numeroTicket
                                       && v.Estado == "COMPLETADA");

            if (venta == null) return null;

            return new VentaParaDevolucionDto
            {
                VentaId = venta.VentaId,
                NumeroTicket = venta.NumeroTicket,
                ClienteNombre = venta.ClienteNombre,
                Total = venta.Total,
                FechaVenta = venta.FechaVenta ?? DateTime.Now,
                Items = venta.DetalleVenta.Select(d => new ItemVentaDto
                {
                    ProductoId = d.ProductoId,
                    NombreProducto = d.NombreProducto,
                    Cantidad = (decimal)d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Total = d.Total
                }).ToList()
            };
        }

        public async Task<DevolucionDetalleViewModel?> GetDetalleAsync(int id)
        {
            var d = await _repo.GetByIdWithDetalleAsync(id);
            if (d == null) return null;

            return new DevolucionDetalleViewModel
            {
                DevolucionId = d.DevolucionId,
                Numero = d.Numero,
                NumeroTicket = d.Venta?.NumeroTicket ?? "",
                ClienteNombre = d.Venta?.ClienteNombre,
                Fecha = d.Fecha ?? DateTime.Now,
                Motivo = d.Motivo,
                TotalDevuelto = d.TotalDevuelto,
                TipoDevolucion = d.TipoDevolucion ?? "EFECTIVO",
                Estado = d.Estado ?? "PROCESADA",
                UsuarioNombre = d.Usuario != null
                    ? $"{d.Usuario.Nombres} {d.Usuario.Apellidos}" : "",
                Items = d.DetalleDevoluciones.Select(i => new DevolucionDetalleItemViewModel
                {
                    NombreProducto = i.NombreProducto,
                    Cantidad = i.Cantidad,
                    PrecioUnitario = i.PrecioUnitario,
                    Total = i.Total,
                    RegresaStock = i.RegresaStock ?? true
                }).ToList()
            };
        }

        public async Task<(bool Success, string Message)> CreateAsync(
            DevolucionCreateViewModel vm, int usuarioId)
        {
            var itemsValidos = vm.Items
                .Where(i => i.CantidadDevolver > 0).ToList();

            if (!itemsValidos.Any())
                return (false, "Selecciona al menos un producto a devolver.");

            var numero = await _repo.GenerarNumeroAsync();
            var total = itemsValidos
                .Sum(i => Math.Round(i.CantidadDevolver * i.PrecioUnitario, 2));

            var dev = new Devolucione
            {
                Numero = numero,
                VentaId = vm.VentaId,
                Fecha = DateTime.Now,
                Motivo = vm.Motivo,
                TotalDevuelto = total,
                TipoDevolucion = vm.TipoDevolucion,
                UsuarioId = usuarioId,
                Estado = "PROCESADA"
            };

            var detalles = itemsValidos.Select(i => new DetalleDevolucione
            {
                ProductoId = i.ProductoId,
                NombreProducto = i.NombreProducto,
                Cantidad = i.CantidadDevolver,
                PrecioUnitario = i.PrecioUnitario,
                Total = Math.Round(i.CantidadDevolver * i.PrecioUnitario, 2),
                RegresaStock = i.RegresaStock
            }).ToList();

            await _repo.CreateAsync(dev, detalles);

            // ── Anular la venta en historial ──
            var venta = await _db.Ventas.FindAsync(vm.VentaId);
            if (venta != null)
            {
                venta.Estado = "ANULADA";
                venta.FechaAnulacion = DateTime.Now;
                venta.MotivoAnulacion = $"Devolución {numero}";
                await _db.SaveChangesAsync();
            }

            return (true, $"Devolución {numero} procesada. Total: S/ {total:N2}");
        }

        public async Task<(bool Success, string Message)> AnularAsync(
            int id, int usuarioId)
        {
            var (ok, msg) = await _repo.AnularAsync(id, usuarioId);

            if (ok)
            {
                // ── Al anular la devolución, reactivar la venta como COMPLETADA ──
                var dev = await _db.Devoluciones.FindAsync(id);
                if (dev != null)
                {
                    var venta = await _db.Ventas.FindAsync(dev.VentaId);
                    if (venta != null)
                    {
                        venta.Estado = "COMPLETADA";
                        venta.FechaAnulacion = null;
                        venta.MotivoAnulacion = null;
                        await _db.SaveChangesAsync();
                    }
                }
            }

            return (ok, msg);
        }
    }
}