using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.service
{
    public interface IKardexService
    {
        Task<KardexIndexViewModel> GetIndexDataAsync(
            int? productoId, string? tipo, string? origen, DateTime? fecha);
        Task<(bool Success, string Message)> AjusteManualAsync(
            AjusteManualViewModel vm, int usuarioId);
    }
}
