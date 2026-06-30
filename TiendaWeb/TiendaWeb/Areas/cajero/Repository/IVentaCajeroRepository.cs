using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.cajero.Repository
{
    public interface IVentaCajeroRepository
    {
        Task<SesionesCaja?> GetSesionAbiertaByUsuarioAsync(int usuarioId);
        Task<int> GetNextCorrelativoAsync(string serie);
        Task CreateAsync(Venta venta, List<DetalleVenta> detalles);
    }
}
