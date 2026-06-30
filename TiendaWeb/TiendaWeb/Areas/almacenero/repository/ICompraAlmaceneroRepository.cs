using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.almacenero.repository
{
    public interface ICompraAlmaceneroRepository
    {
        Task<List<Compra>> GetAllAsync(string? estado);
        Task<Compra?> GetByIdWithDetalleAsync(int id);
        Task<(bool Success, string Message)> RecibirAsync(
            int compraId,
            List<(int DetalleId, int ProductoId, decimal CantidadARecibir)> items,
            int usuarioId);
    }
}
