using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public class KardexRepository : IKardexRepository
    {
        private readonly AppDbContext _db;
        public KardexRepository(AppDbContext db) => _db = db;

        public async Task<List<Kardex>> GetAllWithFiltersAsync(
            int? productoId, string? tipo, string? origen, DateTime? fecha)
        {
            var query = _db.Kardices
                .Include(k => k.Producto)
                .Include(k => k.Usuario)
                .AsQueryable();

            if (productoId.HasValue)
                query = query.Where(k => k.ProductoId == productoId);

            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(k => k.Tipo == tipo);

            if (!string.IsNullOrEmpty(origen))
                query = query.Where(k => k.Origen == origen);

            if (fecha.HasValue)
            {
                var inicio = fecha.Value.Date;
                var fin = inicio.AddDays(1);
                query = query.Where(k =>
                    k.FechaMovimiento >= inicio &&
                    k.FechaMovimiento < fin);
            }

            return await query
                .OrderByDescending(k => k.FechaMovimiento)
                .Take(200)
                .ToListAsync();
        }

        public async Task<Kardex?> GetUltimoByProductoAsync(int productoId)
            => await _db.Kardices
                .Where(k => k.ProductoId == productoId)
                .OrderByDescending(k => k.FechaMovimiento)
                .FirstOrDefaultAsync();

        public async Task CreateAsync(Kardex kardex)
        {
            _db.Kardices.Add(kardex);
            await _db.SaveChangesAsync();
        }
    }
}
