using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public interface IProductoRepository
    {
        Task<List<Producto>> GetAllWithRelationsAsync();
        Task<Producto?> GetByIdAsync(int id);
        Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null);
        Task<bool> ExistsByCodigoBarrasAsync(string codigoBarras, int? excludeId = null);
        Task CreateAsync(Producto producto);
        Task UpdateAsync(Producto producto);
        Task<bool> DeleteAsync(int id);
    }
}
