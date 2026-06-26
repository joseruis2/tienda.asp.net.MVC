using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.service
{
    public interface IConfiguracionService
    {
        Task<ConfiguracionViewModel?> GetAsync();
        Task<(bool Success, string Message)> UpdateAsync(ConfiguracionViewModel vm);
    }
}
