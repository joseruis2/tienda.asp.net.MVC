using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _db;
        public ClienteRepository(AppDbContext db) => _db = db;

        public async Task<List<Cliente>> GetAllAsync()
            => await _db.Clientes
                .OrderByDescending(c => c.FechaCreacion)
                .ToListAsync();

        public async Task<Cliente?> GetByIdAsync(int id)
            => await _db.Clientes.FindAsync(id);

        public async Task<bool> ExistsByDocumentoAsync(string numero, int? excludeId = null)
            => await _db.Clientes.AnyAsync(c =>
                c.NumeroDocumento == numero &&
                (excludeId == null || c.ClienteId != excludeId));

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null)
            => await _db.Clientes.AnyAsync(c =>
                c.Codigo == codigo &&
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

        public async Task<bool> DeleteAsync(int id)
        {
            var c = await _db.Clientes.FindAsync(id);
            if (c == null) return false;
            c.Estado = false;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
