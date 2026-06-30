using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.cajero.Repository
{
    public interface IDashboardCajeroRepository
    {
        Task<SesionesCaja?> GetSesionAbiertaByUsuarioAsync(int usuarioId);
        Task<List<Venta>> GetVentasDeSesionAsync(int sesionId);
        Task<List<Caja>> GetCajasDisponiblesAsync();
        Task<SesionesCaja?> GetCajaAsync(int cajaId);
    }
}
