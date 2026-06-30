using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.cajero.Repository
{
    public class VentaCajeroRepository : IVentaCajeroRepository
    {
        private readonly AppDbContext _db;
        public VentaCajeroRepository(AppDbContext db) => _db = db;

        public async Task<SesionesCaja?> GetSesionAbiertaByUsuarioAsync(int usuarioId)
            => await _db.SesionesCajas
                .Include(s => s.Caja)
                .FirstOrDefaultAsync(s => s.Estado == "ABIERTA"
                                       && s.UsuarioId == usuarioId);

        public async Task<int> GetNextCorrelativoAsync(string serie)
        {
            var ultimo = await _db.Ventas
                .Where(v => v.Serie == serie)
                .MaxAsync(v => (int?)v.NumeroCorrelativo) ?? 0;
            return ultimo + 1;
        }

        public async Task CreateAsync(Venta venta, List<DetalleVenta> detalles)
        {
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                _db.Ventas.Add(venta);
                await _db.SaveChangesAsync();

                foreach (var d in detalles)
                {
                    d.VentaId = venta.VentaId;
                    _db.DetalleVentas.Add(d);

                    var producto = await _db.Productos.FindAsync(d.ProductoId);
                    if (producto != null)
                    {
                        var stockAntes = producto.StockActual ?? 0;
                        producto.StockActual = stockAntes - (int)d.Cantidad;

                        _db.Kardices.Add(new Kardex
                        {
                            ProductoId = d.ProductoId,
                            UsuarioId = venta.UsuarioId ?? 1,
                            Tipo = "SALIDA",
                            Origen = "VENTA",
                            ReferenciaId = venta.VentaId,
                            ReferenciaTipo = "VENTA",
                            Salida = d.Cantidad,
                            StockAnterior = stockAntes,
                            StockResultante = producto.StockActual ?? 0,
                            CostoUnitario = d.PrecioUnitario,
                            Descripcion = $"Venta {venta.NumeroTicket}",
                            FechaMovimiento = DateTime.Now
                        });
                    }
                }

                await _db.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}
