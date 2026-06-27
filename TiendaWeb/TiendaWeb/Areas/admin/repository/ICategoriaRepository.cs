using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public interface ICategoriaRepository
    {
        Task<List<Categoria>> GetAllAsync();
        Task<Categoria?> GetByIdAsync(int id);
        Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null);
        Task CreateAsync(Categoria categoria);
        Task UpdateAsync(Categoria categoria);
        Task<bool> DeleteAsync(int id);
    }
}
