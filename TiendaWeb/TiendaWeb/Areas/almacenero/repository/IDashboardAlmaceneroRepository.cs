using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.almacenero.repository
{
    public interface IDashboardAlmaceneroRepository
    {
        Task<List<Producto>> GetSinStockAsync();
        Task<List<Producto>> GetStockBajoAsync();
        Task<List<Producto>> GetPorVencerAsync(DateOnly hasta);
        Task<List<Compra>> GetComprasPendientesAsync();
        Task<List<Kardex>> GetUltimosMovimientosAsync(int cantidad);
        Task<int> CountTotalProductosAsync();
    }
}
