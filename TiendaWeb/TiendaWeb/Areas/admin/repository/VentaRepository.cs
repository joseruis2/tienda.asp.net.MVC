using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public class VentaRepository : IVentaRepository
    {
        private readonly AppDbContext _db;
        public VentaRepository(AppDbContext db) => _db = db;

        public async Task<List<Venta>> GetAllWithRelationsAsync(
            DateTime? fecha, string? estado, string? metodo)
        {
            var query = _db.Ventas
                .Include(v => v.Usuario)
                .AsQueryable();

            if (fecha.HasValue)
            {
                var inicio = fecha.Value.Date;
                var fin = inicio.AddDays(1);
                query = query.Where(v => v.FechaVenta >= inicio && v.FechaVenta < fin);
            }
            else
            {
                // Por defecto hoy
                var hoy = DateTime.Today;
                query = query.Where(v => v.FechaVenta >= hoy && v.FechaVenta < hoy.AddDays(1));
            }

            if (!string.IsNullOrEmpty(estado))
                query = query.Where(v => v.Estado == estado);

            if (!string.IsNullOrEmpty(metodo))
                query = query.Where(v => v.MetodoPago == metodo);

            return await query
                .OrderByDescending(v => v.FechaVenta)
                .ToListAsync();
        }

        public async Task<Venta?> GetByIdWithDetalleAsync(int id)
            => await _db.Ventas
                .Include(v => v.DetalleVenta)
                .Include(v => v.Usuario)
                .FirstOrDefaultAsync(v => v.VentaId == id);

        public async Task<int> GetNextCorrelativoAsync(string serie)
        {
            var ultimo = await _db.Ventas
                .Where(v => v.Serie == serie)
                .MaxAsync(v => (int?)v.NumeroCorrelativo) ?? 0;
            return ultimo + 1;
        }

        public async Task<string> GenerarNumeroTicketAsync(string serie)
        {
            var correlativo = await GetNextCorrelativoAsync(serie);
            return $"{serie}-{correlativo:D8}";
        }

        public async Task CreateAsync(Venta venta, List<DetalleVenta> detalles)
        {
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                _db.Ventas.Add(venta);
                await _db.SaveChangesAsync();

                foreach (var d in detalles)
                {
                    d.VentaId = venta.VentaId;
                    _db.DetalleVentas.Add(d);

                    // Descontar stock
                    var producto = await _db.Productos.FindAsync(d.ProductoId);
                    if (producto != null)
                        producto.StockActual = (producto.StockActual ?? 0) - (int)d.Cantidad;

                    // Kardex
                    _db.Kardices.Add(new Kardex
                    {
                        ProductoId = d.ProductoId,
                        UsuarioId = venta.UsuarioId ?? 1,
                        Tipo = "SALIDA",
                        Origen = "VENTA",
                        ReferenciaId = venta.VentaId,
                        ReferenciaTipo = "VENTA",
                        Salida = d.Cantidad,
                        StockAnterior = (producto?.StockActual ?? 0) + (int)d.Cantidad,
                        StockResultante = producto?.StockActual ?? 0,
                        CostoUnitario = d.PrecioUnitario,
                        Descripcion = $"Venta {venta.NumeroTicket}",
                        FechaMovimiento = DateTime.Now
                    });
                }

                await _db.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task AnularAsync(int ventaId, string motivo, int usuarioId)
        {
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var venta = await _db.Ventas
                    .Include(v => v.DetalleVenta)
                    .FirstOrDefaultAsync(v => v.VentaId == ventaId)
                    ?? throw new Exception("Venta no encontrada.");

                if (venta.Estado == "ANULADA")
                    throw new Exception("La venta ya está anulada.");

                venta.Estado = "ANULADA";
                venta.FechaAnulacion = DateTime.Now;
                venta.MotivoAnulacion = motivo;
                venta.UsuarioAnulacion = usuarioId;

                // Devolver stock
                foreach (var d in venta.DetalleVenta)
                {
                    var producto = await _db.Productos.FindAsync(d.ProductoId);
                    if (producto != null)
                        producto.StockActual = (producto.StockActual ?? 0) + (int)d.Cantidad;

                    _db.Kardices.Add(new Kardex
                    {
                        ProductoId = d.ProductoId,
                        UsuarioId = usuarioId,
                        Tipo = "ENTRADA",
                        Origen = "DEVOLUCION",
                        ReferenciaId = ventaId,
                        ReferenciaTipo = "VENTA",
                        Entrada = d.Cantidad,
                        StockAnterior = (producto?.StockActual ?? 0) - (int)d.Cantidad,
                        StockResultante = producto?.StockActual ?? 0,
                        Descripcion = $"Anulación {venta.NumeroTicket}",
                        FechaMovimiento = DateTime.Now
                    });
                }

                await _db.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}
