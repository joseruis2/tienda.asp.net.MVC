using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.cajero.Repository
{
    public class DashboardCajeroRepository : IDashboardCajeroRepository
    {
        private readonly AppDbContext _db;
        public DashboardCajeroRepository(AppDbContext db) => _db = db;

        public async Task<SesionesCaja?> GetSesionAbiertaByUsuarioAsync(int usuarioId)
            => await _db.SesionesCajas
                .Include(s => s.Caja)
                .FirstOrDefaultAsync(s => s.Estado == "ABIERTA"
                                       && s.UsuarioId == usuarioId);

        public async Task<List<Venta>> GetVentasDeSesionAsync(int sesionId)
            => await _db.Ventas
                .Where(v => v.SesionId == sesionId
                         && v.Estado == "COMPLETADA")
                .OrderByDescending(v => v.FechaVenta)
                .ToListAsync();

        public async Task<List<Caja>> GetCajasDisponiblesAsync()
            => await _db.Cajas
                .Where(c => c.Activa == true && c.Estado == "CERRADA")
                .OrderBy(c => c.Numero)
                .ToListAsync();

        public async Task<SesionesCaja?> GetCajaAsync(int cajaId)
            => await _db.SesionesCajas
                .Include(s => s.Caja)
                .FirstOrDefaultAsync(s => s.CajaId == cajaId
                                       && s.Estado == "ABIERTA");
    }
}
