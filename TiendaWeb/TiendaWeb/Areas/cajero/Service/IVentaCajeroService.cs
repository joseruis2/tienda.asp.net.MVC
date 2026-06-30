using TiendaWeb.Areas.cajero.ViewModel;

namespace TiendaWeb.Areas.cajero.Service
{
    public interface IVentaCajeroService
    {
        Task<PosCajeroViewModel> GetPosDataAsync(int usuarioId);
        Task<(bool Success, string Message, int? VentaId)> CreateAsync(
            VentaCajeroCreateViewModel vm, int usuarioId);
    }
}
