using TiendaWeb.Areas.almacenero.ViewModel;

namespace TiendaWeb.Areas.almacenero.service
{
    public interface IProductoAlmaceneroService
    {
        Task<ProductosAlmaceneroIndexViewModel> GetIndexDataAsync(
            string? busqueda, string? categoria, string? filtroStock);
    }
}
