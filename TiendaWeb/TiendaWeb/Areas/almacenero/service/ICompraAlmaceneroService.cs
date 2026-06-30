using TiendaWeb.Areas.almacenero.ViewModel;

namespace TiendaWeb.Areas.almacenero.service
{
    public interface ICompraAlmaceneroService
    {
        Task<ComprasAlmaceneroIndexViewModel> GetIndexDataAsync(string? estado);
        Task<RecepcionAlmaceneroViewModel?> GetRecepcionDataAsync(int id);
        Task<CompraAlmaceneroDetalleViewModel?> GetDetalleAsync(int id);
        Task<(bool Success, string Message)> RecibirAsync(
            RecepcionAlmaceneroViewModel vm, int usuarioId);
    }
}
