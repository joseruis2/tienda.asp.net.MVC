using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public class PromocionRepository : IPromocionRepository
    {
        private readonly AppDbContext _db;
        public PromocionRepository(AppDbContext db) => _db = db;

        public async Task<List<Promocione>> GetAllWithRelationsAsync()
            => await _db.Promociones
                .Include(p => p.Producto)
                .Include(p => p.Categoria)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();

        public async Task<Promocione?> GetByIdAsync(int id)
            => await _db.Promociones.FindAsync(id);

        public async Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null)
            => await _db.Promociones.AnyAsync(p =>
                p.Nombre == nombre &&
                (excludeId == null || p.PromoId != excludeId));

        public async Task CreateAsync(Promocione promo)
        {
            _db.Promociones.Add(promo);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Promocione promo)
        {
            _db.Promociones.Update(promo);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var p = await _db.Promociones.FindAsync(id);
            if (p == null) return false;
            p.Estado = false;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
