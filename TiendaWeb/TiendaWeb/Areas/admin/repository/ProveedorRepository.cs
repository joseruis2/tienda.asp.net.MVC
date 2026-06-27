using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly AppDbContext _db;
        public ProveedorRepository(AppDbContext db) => _db = db;

        public async Task<List<Proveedore>> GetAllAsync()
            => await _db.Proveedores
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();

        public async Task<Proveedore?> GetByIdAsync(int id)
            => await _db.Proveedores.FindAsync(id);

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null)
            => await _db.Proveedores.AnyAsync(p =>
                p.Codigo == codigo &&
                (excludeId == null || p.ProveedorId != excludeId));

        public async Task<bool> ExistsByRucDniAsync(string rucDni, int? excludeId = null)
            => await _db.Proveedores.AnyAsync(p =>
                p.RucDni == rucDni &&
                (excludeId == null || p.ProveedorId != excludeId));

        public async Task CreateAsync(Proveedore proveedor)
        {
            _db.Proveedores.Add(proveedor);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Proveedore proveedor)
        {
            _db.Proveedores.Update(proveedor);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var p = await _db.Proveedores.FindAsync(id);
            if (p == null) return false;
            p.Estado = false;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
