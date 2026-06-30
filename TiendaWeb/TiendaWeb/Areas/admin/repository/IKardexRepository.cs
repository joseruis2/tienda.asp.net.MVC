using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public interface IKardexRepository
    {
        Task<List<Kardex>> GetAllWithFiltersAsync(
            int? productoId, string? tipo, string? origen, DateTime? fecha);
        Task<Kardex?> GetUltimoByProductoAsync(int productoId);
        Task CreateAsync(Kardex kardex);
    }
}
