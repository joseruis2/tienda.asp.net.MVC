using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public class CajaRepository : ICajaRepository
    {
        private readonly AppDbContext _db;
        public CajaRepository(AppDbContext db) => _db = db;

        public async Task<List<Caja>> GetAllAsync()
            => await _db.Cajas
                .OrderBy(c => c.Numero)
                .ToListAsync();

        public async Task<Caja?> GetByIdAsync(int id)
            => await _db.Cajas.FindAsync(id);

        public async Task<bool> ExistsByNumeroAsync(int numero, int? excludeId = null)
            => await _db.Cajas.AnyAsync(c =>
                c.Numero == numero &&
                (excludeId == null || c.CajaId != excludeId));

        public async Task CreateAsync(Caja caja)
        {
            _db.Cajas.Add(caja);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Caja caja)
        {
            _db.Cajas.Update(caja);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var c = await _db.Cajas.FindAsync(id);
            if (c == null) return false;

            // No se puede eliminar si está abierta
            if (c.Estado == "ABIERTA") return false;

            c.Activa = false;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
