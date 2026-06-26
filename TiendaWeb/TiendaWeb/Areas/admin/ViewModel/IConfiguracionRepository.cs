using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.ViewModel
{
    public interface IConfiguracionRepository
    {
        Task<ConfiguracionNegocio?> GetAsync();
        Task UpdateAsync(ConfiguracionNegocio config);
    }
}
