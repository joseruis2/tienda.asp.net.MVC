using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.service
{
    public interface IClienteService
    {
        Task<List<ClienteListViewModel>> GetAllAsync();
        Task<ClienteEditViewModel?> GetForEditAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(ClienteCreateViewModel vm);
        Task<(bool Success, string Message)> UpdateAsync(ClienteEditViewModel vm);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}
