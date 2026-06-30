using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public class SesionCajaRepository : ISesionCajaRepository
    {
        private readonly AppDbContext _db;
        public SesionCajaRepository(AppDbContext db) => _db = db;

        public async Task<List<SesionesCaja>> GetAllWithRelationsAsync()
            => await _db.SesionesCajas
                .Include(s => s.Caja)
                .Include(s => s.Usuario)
                .OrderByDescending(s => s.FechaApertura)
                .Take(50)
                .ToListAsync();

        public async Task<SesionesCaja?> GetByIdAsync(int id)
            => await _db.SesionesCajas
                .Include(s => s.Caja)
                .Include(s => s.Usuario)
                .FirstOrDefaultAsync(s => s.SesionId == id);

        public async Task<SesionesCaja?> GetSesionAbiertaAsync()
            => await _db.SesionesCajas
                .Include(s => s.Caja)
                .Include(s => s.Usuario)
                .FirstOrDefaultAsync(s => s.Estado == "ABIERTA");

        public async Task<SesionesCaja?> GetSesionAbiertaByUsuarioAsync(int usuarioId)
            => await _db.SesionesCajas
                .Include(s => s.Caja)
                .FirstOrDefaultAsync(s => s.Estado == "ABIERTA"
                                       && s.UsuarioId == usuarioId);

        public async Task<decimal> GetVentasTotalesSesionAsync(int sesionId)
            => await _db.Ventas
                .Where(v => v.SesionId == sesionId
                         && v.Estado == "COMPLETADA"
                         && v.MetodoPago == "EFECTIVO")
                .SumAsync(v => (decimal?)v.Total) ?? 0;

        public async Task CreateAsync(SesionesCaja sesion)
        {
            _db.SesionesCajas.Add(sesion);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(SesionesCaja sesion)
        {
            _db.SesionesCajas.Update(sesion);
            await _db.SaveChangesAsync();
        }
    }
}
