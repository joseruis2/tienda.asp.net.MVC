using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public class DevolucionRepository : IDevolucionRepository
    {
        private readonly AppDbContext _db;
        public DevolucionRepository(AppDbContext db) => _db = db;

        public async Task<List<Devolucione>> GetAllWithRelationsAsync(DateTime? fecha)
        {
            var query = _db.Devoluciones
                .Include(d => d.Venta)
                .Include(d => d.Usuario)
                .AsQueryable();

            if (fecha.HasValue)
            {
                var inicio = fecha.Value.Date;
                var fin = inicio.AddDays(1);
                query = query.Where(d => d.Fecha >= inicio && d.Fecha < fin);
            }

            return await query
                .OrderByDescending(d => d.Fecha)
                .ToListAsync();
        }

        public async Task<Devolucione?> GetByIdWithDetalleAsync(int id)
            => await _db.Devoluciones
                .Include(d => d.Venta)
                .Include(d => d.Usuario)
                .Include(d => d.DetalleDevoluciones)
                .FirstOrDefaultAsync(d => d.DevolucionId == id);

        public async Task<string> GenerarNumeroAsync()
        {
            var ultimo = await _db.Devoluciones
                .MaxAsync(d => (int?)d.DevolucionId) ?? 0;
            return $"DEV-{(ultimo + 1):D8}";
        }

        public async Task CreateAsync(
            Devolucione dev, List<DetalleDevolucione> detalles)
        {
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                _db.Devoluciones.Add(dev);
                await _db.SaveChangesAsync();

                foreach (var d in detalles)
                {
                    d.DevolucionId = dev.DevolucionId;
                    _db.DetalleDevoluciones.Add(d);

                    if (d.RegresaStock == true)
                    {
                        var producto = await _db.Productos.FindAsync(d.ProductoId);
                        if (producto != null)
                        {
                            var stockAntes = producto.StockActual ?? 0;
                            producto.StockActual = stockAntes + (int)d.Cantidad;

                            _db.Kardices.Add(new Kardex
                            {
                                ProductoId = d.ProductoId,
                                UsuarioId = dev.UsuarioId ?? 1,
                                Tipo = "ENTRADA",
                                Origen = "DEVOLUCION",
                                ReferenciaId = dev.VentaId,
                                ReferenciaTipo = "VENTA",
                                Entrada = d.Cantidad,
                                StockAnterior = stockAntes,
                                StockResultante = producto.StockActual ?? 0,
                                CostoUnitario = d.PrecioUnitario,
                                Descripcion = $"Devolución {dev.Numero}",
                                FechaMovimiento = DateTime.Now
                            });
                        }
                    }
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

        public async Task<(bool Success, string Message)> AnularAsync(
            int id, int usuarioId)
        {
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var dev = await _db.Devoluciones
                    .Include(d => d.DetalleDevoluciones)
                    .FirstOrDefaultAsync(d => d.DevolucionId == id);

                if (dev == null) return (false, "Devolución no encontrada.");
                if (dev.Estado == "ANULADA") return (false, "Ya está anulada.");

                // Revertir stock si se había devuelto
                foreach (var d in dev.DetalleDevoluciones)
                {
                    if (d.RegresaStock != true) continue;

                    var producto = await _db.Productos.FindAsync(d.ProductoId);
                    if (producto == null) continue;

                    var stockAntes = producto.StockActual ?? 0;
                    producto.StockActual = Math.Max(0, stockAntes - (int)d.Cantidad);

                    _db.Kardices.Add(new Kardex
                    {
                        ProductoId = d.ProductoId,
                        UsuarioId = usuarioId,
                        Tipo = "SALIDA",
                        Origen = "AJUSTE_MANUAL",
                        ReferenciaId = dev.VentaId,
                        ReferenciaTipo = "VENTA",
                        Salida = d.Cantidad,
                        StockAnterior = stockAntes,
                        StockResultante = producto.StockActual ?? 0,
                        Descripcion = $"Anulación devolución {dev.Numero}",
                        FechaMovimiento = DateTime.Now
                    });
                }

                dev.Estado = "ANULADA";
                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                return (true, "Devolución anulada correctamente.");
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}
