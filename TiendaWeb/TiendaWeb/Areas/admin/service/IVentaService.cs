using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.service
{
    public interface IVentaService
    {
        Task<PosViewModel> GetPosDataAsync();
        Task<VentasHistorialViewModel> GetHistorialAsync(DateTime? fecha, string? estado, string? metodo);
        Task<VentaDetalleViewModel?> GetDetalleAsync(int id);
        Task<(bool Success, string Message, int? VentaId)> CreateAsync(VentaCreateViewModel vm, int usuarioId);
        Task<(bool Success, string Message)> AnularAsync(AnularVentaViewModel vm, int usuarioId);
    }
}
