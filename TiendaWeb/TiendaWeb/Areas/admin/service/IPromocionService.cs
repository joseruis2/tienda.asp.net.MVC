using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.service
{
    public interface IPromocionService
    {
        Task<PromocionesIndexViewModel> GetIndexDataAsync();
        Task<PromocionEditViewModel?> GetForEditAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(PromocionCreateViewModel vm);
        Task<(bool Success, string Message)> UpdateAsync(PromocionEditViewModel vm);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}
