using Microsoft.EntityFrameworkCore;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.repository
{
    public class CompraRepository : ICompraRepository
    {
        private readonly AppDbContext _db;
        public CompraRepository(AppDbContext db) => _db = db;

        public async Task<List<Compra>> GetAllWithRelationsAsync(string? estado)
        {
            var query = _db.Compras
                .Include(c => c.Proveedor)
                .Include(c => c.Usuario)
                .AsQueryable();

            if (!string.IsNullOrEmpty(estado))
                query = query.Where(c => c.Estado == estado);

            return await query
                .OrderByDescending(c => c.FechaCompra)
                .ToListAsync();
        }

        public async Task<Compra?> GetByIdWithDetalleAsync(int id)
            => await _db.Compras
                .Include(c => c.Proveedor)
                .Include(c => c.Usuario)
                .Include(c => c.DetalleCompras)
                .FirstOrDefaultAsync(c => c.CompraId == id);

        public async Task<int> GetNextCorrelativoAsync(string serie)
        {
            var ultimo = await _db.Compras
                .Where(c => c.Serie == serie)
                .MaxAsync(c => (int?)c.NumeroCorrelativo) ?? 0;
            return ultimo + 1;
        }

        public async Task CreateAsync(Compra compra, List<DetalleCompra> detalles)
        {
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                _db.Compras.Add(compra);
                await _db.SaveChangesAsync();

                foreach (var d in detalles)
                {
                    d.CompraId = compra.CompraId;
                    _db.DetalleCompras.Add(d);
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

        public async Task<(bool Success, string Message)> RecibirAsync(
            int compraId,
            List<(int DetalleId, decimal CantidadARecibir)> recepciones,
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

                foreach (var (detalleId, cantARecibir) in recepciones)
                {
                    if (cantARecibir <= 0) continue;

                    var det = compra.DetalleCompras
                        .FirstOrDefault(d => d.DetalleId == detalleId);
                    if (det == null) continue;

                    var maxRecibir = det.Cantidad - (det.CantidadRecibida ?? 0);
                    var recibir = Math.Min(cantARecibir, maxRecibir);
                    if (recibir <= 0) continue;

                    det.CantidadRecibida = (det.CantidadRecibida ?? 0) + recibir;

                    // Subir stock
                    var producto = await _db.Productos.FindAsync(det.ProductoId);
                    if (producto != null)
                    {
                        var stockAntes = producto.StockActual ?? 0;
                        producto.StockActual = stockAntes + (int)recibir;
                        // Actualizar precio costo
                        producto.PrecioCosto = det.PrecioUnitario;

                        // Kardex
                        _db.Kardices.Add(new Kardex
                        {
                            ProductoId = det.ProductoId,
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
                    }
                }

                // Determinar nuevo estado
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
                    ? "Orden recibida completamente."
                    : "Recepción parcial registrada.");
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<(bool Success, string Message)> AnularAsync(
            int compraId, int usuarioId)
        {
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var compra = await _db.Compras
                    .Include(c => c.DetalleCompras)
                    .FirstOrDefaultAsync(c => c.CompraId == compraId);

                if (compra == null) return (false, "Orden no encontrada.");
                if (compra.Estado == "ANULADA") return (false, "Ya está anulada.");

                // Si se recibió mercadería → devolver stock
                foreach (var d in compra.DetalleCompras)
                {
                    var recibida = d.CantidadRecibida ?? 0;
                    if (recibida <= 0) continue;

                    var producto = await _db.Productos.FindAsync(d.ProductoId);
                    if (producto != null)
                    {
                        var stockAntes = producto.StockActual ?? 0;
                        producto.StockActual = Math.Max(0, stockAntes - (int)recibida);

                        _db.Kardices.Add(new Kardex
                        {
                            ProductoId = d.ProductoId,
                            UsuarioId = usuarioId,
                            Tipo = "SALIDA",
                            Origen = "AJUSTE_MANUAL",
                            ReferenciaId = compraId,
                            ReferenciaTipo = "COMPRA",
                            Salida = recibida,
                            StockAnterior = stockAntes,
                            StockResultante = producto.StockActual ?? 0,
                            Descripcion = $"Anulación compra {compra.NumeroOrden}",
                            FechaMovimiento = DateTime.Now
                        });
                    }
                }

                compra.Estado = "ANULADA";
                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                return (true, "Orden anulada correctamente.");
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}
