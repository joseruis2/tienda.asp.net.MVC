using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.almacenero.repository
{
    public interface IProductoAlmaceneroRepository
    {
        Task<List<Producto>> GetAllActivosAsync(
            string? busqueda, string? categoria, string? filtroStock);
        Task<List<string>> GetCategoriasAsync();
        Task<int> CountStockBajoAsync();
        Task<int> CountSinStockAsync();
        Task<int> CountPorVencerAsync(DateOnly hasta);
    }
}
