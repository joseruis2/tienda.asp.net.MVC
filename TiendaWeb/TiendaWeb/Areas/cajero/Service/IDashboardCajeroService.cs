using TiendaWeb.Areas.cajero.ViewModel;

namespace TiendaWeb.Areas.cajero.Service
{
    public interface IDashboardCajeroService
    {
        Task<DashboardCajeroViewModel> GetDashboardDataAsync(int usuarioId);
        Task<(bool Success, string Message)> AbrirCajaAsync(
            AbrirCajaCajeroViewModel vm, int usuarioId);
        Task<CerrarCajaCajeroViewModel?> GetCierreDataAsync(int usuarioId);
        Task<(bool Success, string Message)> CerrarCajaAsync(
            CerrarCajaCajeroViewModel vm);
    }
}
