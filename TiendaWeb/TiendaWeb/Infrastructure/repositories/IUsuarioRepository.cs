using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Infrastructure.repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByUsernameAsync(string username);
        Task<Usuario?> GetByIdAsync(int id);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<bool> ExistsByDniAsync(string dni);
        Task CreateAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
    }
}
