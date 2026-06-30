using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.almacenero.repository
{
    public interface IKardexAlmaceneroRepository
    {
        Task<List<Kardex>> GetAllWithFiltersAsync(
            int? productoId, string? tipo,
            string? origen, DateTime? fecha);
        Task<List<(int Id, string Nombre, string Codigo)>> GetProductosAsync();
    }
}
