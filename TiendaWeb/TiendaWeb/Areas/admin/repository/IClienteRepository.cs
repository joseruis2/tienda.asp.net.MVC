using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public interface IClienteRepository
    {
        Task<List<Cliente>> GetAllAsync();
        Task<Cliente?> GetByIdAsync(int id);
        Task<bool> ExistsByDocumentoAsync(string numero, int? excludeId = null);
        Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null);
        Task CreateAsync(Cliente cliente);
        Task UpdateAsync(Cliente cliente);
        Task<bool> DeleteAsync(int id);
    }
}
