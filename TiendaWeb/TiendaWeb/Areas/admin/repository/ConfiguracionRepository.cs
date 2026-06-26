using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.repository
{
    public class ConfiguracionRepository : IConfiguracionRepository
    {
        private readonly AppDbContext _db;
        public ConfiguracionRepository(AppDbContext db) => _db = db;

        public async Task<ConfiguracionNegocio?> GetAsync()
            => await _db.ConfiguracionNegocios.FirstOrDefaultAsync();

        public async Task UpdateAsync(ConfiguracionNegocio config)
        {
            _db.ConfiguracionNegocios.Update(config);
            await _db.SaveChangesAsync();
        }
    }
}
