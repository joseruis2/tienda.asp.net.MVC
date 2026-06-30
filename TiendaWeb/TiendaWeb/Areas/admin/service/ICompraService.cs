using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.service
{
    public interface ICompraService
    {
        Task<ComprasIndexViewModel> GetIndexDataAsync(string? estado);
        Task<CompraDetalleViewModel?> GetDetalleAsync(int id);
        Task<RecepcionCompraViewModel?> GetRecepcionDataAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(CompraCreateViewModel vm, int usuarioId);
        Task<(bool Success, string Message)> RecibirAsync(RecepcionCompraViewModel vm, int usuarioId);
        Task<(bool Success, string Message)> AnularAsync(AnularCompraViewModel vm, int usuarioId);
    }
}
