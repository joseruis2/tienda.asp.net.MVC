using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.service
{
    public interface IProveedorService
    {
        Task<List<ProveedorListViewModel>> GetAllAsync();
        Task<ProveedorEditViewModel?> GetForEditAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(ProveedorCreateViewModel vm);
        Task<(bool Success, string Message)> UpdateAsync(ProveedorEditViewModel vm);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}
