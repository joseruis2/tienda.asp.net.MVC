using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.service
{
    public interface IUsuarioAdminService
    {
        Task<List<UsuarioListViewModel>> GetAllAsync();
        Task<UsuarioEditViewModel?> GetForEditAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(UsuarioCreateViewModel vm, int creadoPor);
        Task<(bool Success, string Message)> UpdateAsync(UsuarioEditViewModel vm);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}
