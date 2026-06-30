using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.service
{
    public interface ICajaService
    {
        Task<List<CajaListViewModel>> GetAllAsync();
        Task<CajaEditViewModel?> GetForEditAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(CajaCreateViewModel vm);
        Task<(bool Success, string Message)> UpdateAsync(CajaEditViewModel vm);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}
