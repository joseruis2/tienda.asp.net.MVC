using Microsoft.EntityFrameworkCore;
using TiendaWeb.Areas.admin.ViewModel;
using TiendaWeb.Infrastructure.Data;

namespace TiendaWeb.Areas.admin.service
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _db;
        public DashboardService(AppDbContext db) => _db = db;

        public async Task<DashboardViewModel> GetDashboardDataAsync()
        {
            var hoy = DateTime.Today;
            var manana = hoy.AddDays(1);

            // Ventas del día
            var ventasHoy = await _db.Ventas
                .Where(v => v.FechaVenta >= hoy && v.FechaVenta < manana
                         && v.Estado == "COMPLETADA")
                .ToListAsync();

            // Stock bajo
            var stockBajo = await _db.Productos
                .CountAsync(p => p.Estado == true
                              && p.StockActual <= p.StockMinimo);

            // Clientes nuevos hoy
            var clientesNuevos = await _db.Clientes
                .CountAsync(c => c.FechaCreacion >= hoy
                              && c.FechaCreacion < manana);

            // Compras pendientes
            var comprasPendientes = await _db.Compras
                .CountAsync(c => c.Estado == "PENDIENTE");

            // Estado de caja (sesión abierta)
            var sesionAbierta = await _db.SesionesCajas
                .Where(s => s.Estado == "ABIERTA")
                .FirstOrDefaultAsync();

            // Últimas 10 ventas
            var ultimas = await _db.Ventas
                .OrderByDescending(v => v.FechaVenta)
                .Take(10)
                .Select(v => new UltimaVentaDto
                {
                    NumeroTicket = v.NumeroTicket,
                    ClienteNombre = v.ClienteNombre ?? "Consumidor Final",
                    Total = v.Total,
                    MetodoPago = v.MetodoPago ?? "",
                    Estado = v.Estado ?? "",
                    FechaVenta = v.FechaVenta ?? DateTime.MinValue

                })
                .ToListAsync();

            return new DashboardViewModel
            {
                VentasHoy = ventasHoy.Sum(v => v.Total),
                TransaccionesHoy = ventasHoy.Count,
                ProductosStockBajo = stockBajo,
                ClientesNuevosHoy = clientesNuevos,
                ComprasPendientes = comprasPendientes,
                EstadoCaja = sesionAbierta != null ? "ABIERTA" : "CERRADA",
                MontoCaja = sesionAbierta?.MontoApertura ?? 0,
                UltimasVentas = ultimas
            };
        }
    }
}
