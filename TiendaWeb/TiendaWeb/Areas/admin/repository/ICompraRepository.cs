using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public interface ICompraRepository
    {
        Task<List<Compra>> GetAllWithRelationsAsync(string? estado);
        Task<Compra?> GetByIdWithDetalleAsync(int id);
        Task<int> GetNextCorrelativoAsync(string serie);
        Task CreateAsync(Compra compra, List<DetalleCompra> detalles);
        Task<(bool Success, string Message)> RecibirAsync(int compraId,
            List<(int DetalleId, decimal CantidadARecibir)> recepciones, int usuarioId);
        Task<(bool Success, string Message)> AnularAsync(int compraId, int usuarioId);
    }
}
