using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.service
{
    public interface ICategoriaService
    {
        Task<List<CategoriaListViewModel>> GetAllAsync();
        Task<CategoriaEditViewModel?> GetForEditAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(CategoriaCreateViewModel vm);
        Task<(bool Success, string Message)> UpdateAsync(CategoriaEditViewModel vm);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}
