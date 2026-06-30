using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public interface IVentaRepository
    {
        Task<List<Venta>> GetAllWithRelationsAsync(DateTime? fecha, string? estado, string? metodo);
        Task<Venta?> GetByIdWithDetalleAsync(int id);
        Task<int> GetNextCorrelativoAsync(string serie);
        Task<string> GenerarNumeroTicketAsync(string serie);
        Task CreateAsync(Venta venta, List<DetalleVenta> detalles);
        Task AnularAsync(int ventaId, string motivo, int usuarioId);
    }
}
