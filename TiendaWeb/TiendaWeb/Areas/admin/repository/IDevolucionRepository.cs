using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public interface IDevolucionRepository
    {
        Task<List<Devolucione>> GetAllWithRelationsAsync(DateTime? fecha);
        Task<Devolucione?> GetByIdWithDetalleAsync(int id);
        Task<string> GenerarNumeroAsync();
        Task CreateAsync(Devolucione dev, List<DetalleDevolucione> detalles);
        Task<(bool Success, string Message)> AnularAsync(int id, int usuarioId);
    }
}
