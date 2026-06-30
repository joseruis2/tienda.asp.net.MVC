using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public interface IConfiguracionRepository
    {
        Task<ConfiguracionNegocio?> GetAsync();
        Task UpdateAsync(ConfiguracionNegocio config);
    }
}
