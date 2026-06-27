using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public interface IProveedorRepository
    {
        Task<List<Proveedore>> GetAllAsync();
        Task<Proveedore?> GetByIdAsync(int id);
        Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null);
        Task<bool> ExistsByRucDniAsync(string rucDni, int? excludeId = null);
        Task CreateAsync(Proveedore proveedor);
        Task UpdateAsync(Proveedore proveedor);
        Task<bool> DeleteAsync(int id);
    }
}
