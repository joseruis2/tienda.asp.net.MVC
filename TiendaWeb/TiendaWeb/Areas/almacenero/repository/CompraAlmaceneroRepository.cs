using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.almacenero.repository
{
    public class CompraAlmaceneroRepository : ICompraAlmaceneroRepository
    {
        private readonly AppDbContext _db;
        public CompraAlmaceneroRepository(AppDbContext db) => _db = db;

        public async Task<List<Compra>> GetAllAsync(string? estado)
        {
            var query = _db.Compras
                .Include(c => c.Proveedor)
                .Include(c => c.Usuario)
                .AsQueryable();

            if (!string.IsNullOrEmpty(estado))
                query = query.Where(c => c.Estado == estado);
            else
                query = query.Where(c => c.Estado != "ANULADA");

            return await query
                .OrderBy(c => c.Estado == "PENDIENTE" ? 0 :
                              c.Estado == "PARCIAL" ? 1 : 2)
                .ThenBy(c => c.FechaEntrega)
                .ThenByDescending(c => c.FechaCompra)
                .ToListAsync();
        }

        public async Task<Compra?> GetByIdWithDetalleAsync(int id)
            => await _db.Compras
                .Include(c => c.Proveedor)
                .Include(c => c.DetalleCompras)
                .FirstOrDefaultAsync(c => c.CompraId == id);

        public async Task<(bool Success, string Message)> RecibirAsync(
            int compraId,
            List<(int DetalleId, int ProductoId, decimal CantidadARecibir)> items,
            int usuarioId)
        {
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var compra = await _db.Compras
                    .Include(c => c.DetalleCompras)
                    .FirstOrDefaultAsync(c => c.CompraId == compraId);

                if (compra == null)
                    return (false, "Orden no encontrada.");

                if (compra.Estado == "ANULADA")
                    return (false, "La orden está anulada.");

                if (compra.Estado == "RECIBIDA")
                    return (false, "La orden ya fue recibida completamente.");

                int productosRecibidos = 0;

                foreach (var (detalleId, productoId, cantARecibir) in items)
                {
                    if (cantARecibir <= 0) continue;

                    var det = compra.DetalleCompras
                        .FirstOrDefault(d => d.DetalleId == detalleId);
                    if (det == null) continue;

                    var pendiente = det.Cantidad - (det.CantidadRecibida ?? 0);
                    var recibir = Math.Min(cantARecibir, pendiente);
                    if (recibir <= 0) continue;

                    det.CantidadRecibida = (det.CantidadRecibida ?? 0) + recibir;

                    // Subir stock
                    var producto = await _db.Productos.FindAsync(productoId);
                    if (producto != null)
                    {
                        var stockAntes = producto.StockActual ?? 0;
                        producto.StockActual = stockAntes + (int)recibir;
                        producto.PrecioCosto = det.PrecioUnitario;

                        _db.Kardices.Add(new Kardex
                        {
                            ProductoId = productoId,
                            UsuarioId = usuarioId,
                            Tipo = "ENTRADA",
                            Origen = "COMPRA",
                            ReferenciaId = compraId,
                            ReferenciaTipo = "COMPRA",
                            Entrada = recibir,
                            StockAnterior = stockAntes,
                            StockResultante = producto.StockActual ?? 0,
                            CostoUnitario = det.PrecioUnitario,
                            Descripcion = $"Recepción {compra.NumeroOrden}",
                            FechaMovimiento = DateTime.Now
                        });

                        productosRecibidos++;
                    }
                }

                if (productosRecibidos == 0)
                    return (false, "No se ingresaron cantidades a recibir.");

                // Actualizar estado
                bool todoRecibido = compra.DetalleCompras
                    .All(d => (d.CantidadRecibida ?? 0) >= d.Cantidad);
                bool algoRecibido = compra.DetalleCompras
                    .Any(d => (d.CantidadRecibida ?? 0) > 0);

                compra.Estado = todoRecibido ? "RECIBIDA"
                              : algoRecibido ? "PARCIAL"
                              : compra.Estado;

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                return (true, todoRecibido
                    ? $"✓ Orden {compra.NumeroOrden} recibida completamente. Stock actualizado."
                    : $"✓ Recepción parcial registrada. Stock actualizado para {productosRecibidos} producto(s).");
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return (false, $"Error al procesar: {ex.Message}");
            }
        }
    }
}
