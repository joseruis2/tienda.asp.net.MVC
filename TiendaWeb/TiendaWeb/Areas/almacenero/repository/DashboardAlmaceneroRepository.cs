using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.almacenero.repository
{
    public class DashboardAlmaceneroRepository : IDashboardAlmaceneroRepository
    {
        private readonly AppDbContext _db;
        public DashboardAlmaceneroRepository(AppDbContext db) => _db = db;

        public async Task<List<Producto>> GetSinStockAsync()
            => await _db.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Where(p => p.Estado == true && p.StockActual == 0)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

        public async Task<List<Producto>> GetStockBajoAsync()
            => await _db.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Where(p => p.Estado == true
                         && p.StockActual > 0
                         && p.StockActual <= p.StockMinimo)
                .OrderBy(p => p.StockActual)
                .ToListAsync();

        public async Task<List<Producto>> GetPorVencerAsync(DateOnly hasta)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            return await _db.Productos
                .Where(p => p.Estado == true
                         && p.FechaVencimiento != null
                         && p.FechaVencimiento >= hoy
                         && p.FechaVencimiento <= hasta)
                .OrderBy(p => p.FechaVencimiento)
                .ToListAsync();
        }

        public async Task<List<Compra>> GetComprasPendientesAsync()
            => await _db.Compras
                .Include(c => c.Proveedor)
                .Where(c => c.Estado == "PENDIENTE" || c.Estado == "PARCIAL")
                .OrderBy(c => c.FechaEntrega)
                .ToListAsync();

        public async Task<List<Kardex>> GetUltimosMovimientosAsync(int cantidad)
            => await _db.Kardices
                .Include(k => k.Producto)
                .OrderByDescending(k => k.FechaMovimiento)
                .Take(cantidad)
                .ToListAsync();

        public async Task<int> CountTotalProductosAsync()
            => await _db.Productos
                .CountAsync(p => p.Estado == true);
    }
}
