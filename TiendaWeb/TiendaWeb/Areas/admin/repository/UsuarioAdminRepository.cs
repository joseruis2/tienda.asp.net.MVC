using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace TiendaWeb.Areas.admin.repository
{
    public class UsuarioAdminRepository : IUsuarioAdminRepository
    {
        private readonly AppDbContext _db;
        public UsuarioAdminRepository(AppDbContext db) => _db = db;

        public async Task<List<Usuario>> GetAllAsync()
            => await _db.Usuarios
                .OrderByDescending(u => u.FechaCreacion)
                .ToListAsync();

        public async Task<Usuario?> GetByIdAsync(int id)
            => await _db.Usuarios.FindAsync(id);

        public async Task<bool> ExistsByUsernameAsync(string username, int? excludeId = null)
            => await _db.Usuarios.AnyAsync(u =>
                u.Username == username &&
                (excludeId == null || u.UsuarioId != excludeId));

        public async Task<bool> ExistsByDniAsync(string dni, int? excludeId = null)
            => await _db.Usuarios.AnyAsync(u =>
                u.Dni == dni &&
                (excludeId == null || u.UsuarioId != excludeId));

        public async Task CreateAsync(Usuario usuario)
        {
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _db.Usuarios.Update(usuario);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var u = await _db.Usuarios.FindAsync(id);
            if (u == null) return false;
            // Soft delete — no borramos el registro
            u.Estado = false;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
