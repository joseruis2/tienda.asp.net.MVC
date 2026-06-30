using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.service
{
    public interface IDevolucionService
    {
        Task<DevolucionesIndexViewModel> GetIndexDataAsync(DateTime? fecha);
        Task<VentaParaDevolucionDto?> BuscarVentaAsync(string numeroTicket);
        Task<DevolucionDetalleViewModel?> GetDetalleAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(
            DevolucionCreateViewModel vm, int usuarioId);
        Task<(bool Success, string Message)> AnularAsync(int id, int usuarioId);
    }
}
