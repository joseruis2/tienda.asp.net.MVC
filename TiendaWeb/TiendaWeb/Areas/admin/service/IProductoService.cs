using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.service
{
    public interface IProductoService
    {
        Task<ProductosIndexViewModel> GetIndexDataAsync();
        Task<ProductoEditViewModel?> GetForEditAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(ProductoCreateViewModel vm);
        Task<(bool Success, string Message)> UpdateAsync(ProductoEditViewModel vm);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}
