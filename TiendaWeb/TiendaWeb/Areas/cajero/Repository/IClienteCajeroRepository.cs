using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.cajero.Repository
{
    public interface IClienteCajeroRepository
    {
        Task<List<Cliente>> BuscarAsync(string? busqueda);
        Task<Cliente?> GetByIdAsync(int id);
        Task<bool> ExistsByDocumentoAsync(string numero, int? excludeId = null);
        Task CreateAsync(Cliente cliente);
        Task UpdateAsync(Cliente cliente);
    }
}
