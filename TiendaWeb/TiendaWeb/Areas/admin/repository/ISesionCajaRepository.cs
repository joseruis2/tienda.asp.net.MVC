using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public interface ISesionCajaRepository
    {
        Task<List<SesionesCaja>> GetAllWithRelationsAsync();
        Task<SesionesCaja?> GetByIdAsync(int id);
        Task<SesionesCaja?> GetSesionAbiertaAsync();
        Task<SesionesCaja?> GetSesionAbiertaByUsuarioAsync(int usuarioId);
        Task<decimal> GetVentasTotalesSesionAsync(int sesionId);
        Task CreateAsync(SesionesCaja sesion);
        Task UpdateAsync(SesionesCaja sesion);
    }
}
