using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public interface IPromocionRepository
    {
        Task<List<Promocione>> GetAllWithRelationsAsync();
        Task<Promocione?> GetByIdAsync(int id);
        Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null);
        Task CreateAsync(Promocione promo);
        Task UpdateAsync(Promocione promo);
        Task<bool> DeleteAsync(int id);
    }
}
