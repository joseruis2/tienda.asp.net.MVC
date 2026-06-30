using TiendaWeb.Areas.almacenero.ViewModel;

namespace TiendaWeb.Areas.almacenero.service
{
    public interface IKardexAlmaceneroService
    {
        Task<KardexAlmaceneroIndexViewModel> GetIndexDataAsync(
            int? productoId, string? tipo,
            string? origen, DateTime? fecha);
    }
}
