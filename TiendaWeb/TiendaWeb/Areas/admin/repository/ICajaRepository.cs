using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public interface ICajaRepository
    {
        Task<List<Caja>> GetAllAsync();
        Task<Caja?> GetByIdAsync(int id);
        Task<bool> ExistsByNumeroAsync(int numero, int? excludeId = null);
        Task CreateAsync(Caja caja);
        Task UpdateAsync(Caja caja);
        Task<bool> DeleteAsync(int id);
    }
}
