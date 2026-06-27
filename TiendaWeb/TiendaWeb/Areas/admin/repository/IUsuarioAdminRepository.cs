using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public interface IUsuarioAdminRepository
    {
        Task<List<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(int id);
        Task<bool> ExistsByUsernameAsync(string username, int? excludeId = null);
        Task<bool> ExistsByDniAsync(string dni, int? excludeId = null);
        Task CreateAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task<bool> DeleteAsync(int id);
    }
}
