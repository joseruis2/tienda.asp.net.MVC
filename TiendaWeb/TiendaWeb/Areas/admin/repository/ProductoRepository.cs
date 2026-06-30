using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly AppDbContext _db;
        public ProductoRepository(AppDbContext db) => _db = db;

        public async Task<List<Producto>> GetAllWithRelationsAsync()
            => await _db.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();

        public async Task<Producto?> GetByIdAsync(int id)
            => await _db.Productos.FindAsync(id);

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null)
            => await _db.Productos.AnyAsync(p =>
                p.Codigo == codigo &&
                (excludeId == null || p.ProductoId != excludeId));

        public async Task<bool> ExistsByCodigoBarrasAsync(string codigoBarras, int? excludeId = null)
            => await _db.Productos.AnyAsync(p =>
                p.CodigoBarras == codigoBarras &&
                (excludeId == null || p.ProductoId != excludeId));

        public async Task CreateAsync(Producto producto)
        {
            _db.Productos.Add(producto);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Producto producto)
        {
            _db.Productos.Update(producto);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var p = await _db.Productos.FindAsync(id);
            if (p == null) return false;
            p.Estado = false;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
