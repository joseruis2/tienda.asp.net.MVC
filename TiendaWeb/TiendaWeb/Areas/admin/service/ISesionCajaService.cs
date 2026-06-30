using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.service
{
    public interface ISesionCajaService
    {
        Task<SesionesCajaIndexViewModel> GetIndexDataAsync(int usuarioId);
        Task<CerrarCajaViewModel?> GetCierreDataAsync(int sesionId);
        Task<(bool Success, string Message)> AbrirCajaAsync(AbrirCajaViewModel vm, int usuarioId);
        Task<(bool Success, string Message)> CerrarCajaAsync(CerrarCajaViewModel vm);
    }
}
