using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.cajero.Repository
{
    public class ClienteCajeroRepository : IClienteCajeroRepository
    {
        private readonly AppDbContext _db;
        public ClienteCajeroRepository(AppDbContext db) => _db = db;

        public async Task<List<Cliente>> BuscarAsync(string? busqueda)
        {
            var query = _db.Clientes
                .Where(c => c.Estado == true)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(busqueda))
                query = query.Where(c =>
                    (c.Nombres != null && c.Nombres.Contains(busqueda)) ||
                    (c.Apellidos != null && c.Apellidos.Contains(busqueda)) ||
                    (c.RazonSocial != null && c.RazonSocial.Contains(busqueda)) ||
                    (c.NumeroDocumento != null && c.NumeroDocumento.Contains(busqueda)) ||
                    (c.Telefono != null && c.Telefono.Contains(busqueda)) ||
                    (c.Codigo != null && c.Codigo.Contains(busqueda)));

            return await query
                .OrderByDescending(c => c.TotalCompras)
                .Take(20)
                .ToListAsync();
        }

        public async Task<Cliente?> GetByIdAsync(int id)
            => await _db.Clientes.FindAsync(id);

        public async Task<bool> ExistsByDocumentoAsync(string numero, int? excludeId = null)
            => await _db.Clientes.AnyAsync(c =>
                c.NumeroDocumento == numero &&
                (excludeId == null || c.ClienteId != excludeId));

        public async Task CreateAsync(Cliente cliente)
        {
            _db.Clientes.Add(cliente);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cliente cliente)
        {
            _db.Clientes.Update(cliente);
            await _db.SaveChangesAsync();
        }
    }
}
