using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.almacenero.repository
{
    public class KardexAlmaceneroRepository : IKardexAlmaceneroRepository
    {
        private readonly AppDbContext _db;
        public KardexAlmaceneroRepository(AppDbContext db) => _db = db;

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
                .Take(300)
                .ToListAsync();
        }

        // Reemplaza GetProductosAsync con esto:
        public async Task<List<(int Id, string Nombre, string Codigo)>> GetProductosAsync()
        {
            var lista = await _db.Productos
                .Where(p => p.Estado == true)
                .OrderBy(p => p.Nombre)
                .Select(p => new { p.ProductoId, p.Nombre, p.Codigo })
                .ToListAsync();

            return lista.Select(p => (p.ProductoId, p.Nombre, p.Codigo)).ToList();
        }
    }
}
