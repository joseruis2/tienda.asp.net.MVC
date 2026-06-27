using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _db;
        public CategoriaRepository(AppDbContext db) => _db = db;

        public async Task<List<Categoria>> GetAllAsync()
            => await _db.Categorias
                .OrderByDescending(c => c.FechaCreacion)
                .ToListAsync();

        public async Task<Categoria?> GetByIdAsync(int id)
            => await _db.Categorias.FindAsync(id);

        public async Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null)
            => await _db.Categorias.AnyAsync(c =>
                c.Nombre == nombre &&
                (excludeId == null || c.CategoriaId != excludeId));

        public async Task CreateAsync(Categoria categoria)
        {
            _db.Categorias.Add(categoria);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Categoria categoria)
        {
            _db.Categorias.Update(categoria);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var c = await _db.Categorias.FindAsync(id);
            if (c == null) return false;
            c.Estado = false;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
