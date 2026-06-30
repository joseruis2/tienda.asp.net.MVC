using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.almacenero.repository
{
    public class ProductoAlmaceneroRepository : IProductoAlmaceneroRepository
    {
        private readonly AppDbContext _db;
        public ProductoAlmaceneroRepository(AppDbContext db) => _db = db;

        public async Task<List<Producto>> GetAllActivosAsync(
            string? busqueda, string? categoria, string? filtroStock)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            var en7dias = hoy.AddDays(7);

            var query = _db.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .Where(p => p.Estado == true)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(busqueda))
                query = query.Where(p =>
                    p.Nombre.Contains(busqueda) ||
                    p.Codigo.Contains(busqueda) ||
                    (p.CodigoBarras != null &&
                     p.CodigoBarras.Contains(busqueda)));

            if (!string.IsNullOrWhiteSpace(categoria))
                query = query.Where(p =>
                    p.Categoria != null &&
                    p.Categoria.Nombre == categoria);

            if (filtroStock == "bajo")
                query = query.Where(p =>
                    p.StockActual <= p.StockMinimo &&
                    p.StockActual > 0);
            else if (filtroStock == "critico")
                query = query.Where(p => p.StockActual == 0);
            else if (filtroStock == "vencer")
                query = query.Where(p =>
                    p.FechaVencimiento != null &&
                    p.FechaVencimiento <= en7dias &&
                    p.FechaVencimiento >= hoy);

            return await query
                .OrderBy(p => p.StockActual)
                .ThenBy(p => p.Nombre)
                .ToListAsync();
        }

        public async Task<List<string>> GetCategoriasAsync()
            => await _db.Categorias
                .Where(c => c.Estado == true)
                .Select(c => c.Nombre)
                .OrderBy(n => n)
                .ToListAsync();

        public async Task<int> CountStockBajoAsync()
            => await _db.Productos
                .CountAsync(p =>
                    p.Estado == true &&
                    p.StockActual <= p.StockMinimo &&
                    p.StockActual > 0);

        public async Task<int> CountSinStockAsync()
            => await _db.Productos
                .CountAsync(p =>
                    p.Estado == true &&
                    p.StockActual == 0);

        public async Task<int> CountPorVencerAsync(DateOnly hasta)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            return await _db.Productos
                .CountAsync(p =>
                    p.Estado == true &&
                    p.FechaVencimiento != null &&
                    p.FechaVencimiento >= hoy &&
                    p.FechaVencimiento <= hasta);
        }
    }
}
