using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Infrastructure.repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> GetByUsernameAsync(string username)
            => await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == username && u.Estado == true);

        public async Task<Usuario?> GetByIdAsync(int id)
            => await _context.Usuarios
                .FirstOrDefaultAsync(u => u.UsuarioId == id && u.Estado == true);

        public async Task<bool> ExistsByUsernameAsync(string username)
            => await _context.Usuarios
                .AnyAsync(u => u.Username == username);

        public async Task<bool> ExistsByDniAsync(string dni)
            => await _context.Usuarios
                .AnyAsync(u => u.Dni == dni);

        public async Task CreateAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
