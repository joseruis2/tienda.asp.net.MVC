using Microsoft.EntityFrameworkCore;
using TiendaWeb.Areas.admin.repository;
using TiendaWeb.Areas.admin.ViewModel;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.service
{
    public class KardexService : IKardexService
    {
        private readonly IKardexRepository _repo;
        private readonly AppDbContext _db;

        public KardexService(IKardexRepository repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        public async Task<KardexIndexViewModel> GetIndexDataAsync(
            int? productoId, string? tipo, string? origen, DateTime? fecha)
        {
            var lista = await _repo.GetAllWithFiltersAsync(productoId, tipo, origen, fecha);

            var productos = await _db.Productos
                .Where(p => p.Estado == true)
                .Select(p => new SelectItemViewModel
                {
                    Id = p.ProductoId,
                    Nombre = $"{p.Codigo} — {p.Nombre}"
                })
                .ToListAsync();

            return new KardexIndexViewModel
            {
                Lista = lista.Select(k => new KardexListViewModel
                {
                    KardexId = k.KardexId,
                    ProductoNombre = k.Producto?.Nombre ?? "",
                    ProductoCodigo = k.Producto?.Codigo ?? "",
                    UsuarioNombre = k.Usuario != null
                        ? $"{k.Usuario.Nombres} {k.Usuario.Apellidos}" : "",
                    Tipo = k.Tipo,
                    Origen = k.Origen,
                    ReferenciaId = k.ReferenciaId,
                    ReferenciaTipo = k.ReferenciaTipo,
                    Entrada = k.Entrada ?? 0,
                    Salida = k.Salida ?? 0,
                    StockAnterior = k.StockAnterior,
                    StockResultante = k.StockResultante,
                    CostoUnitario = k.CostoUnitario ?? 0,
                    Descripcion = k.Descripcion,
                    FechaMovimiento = k.FechaMovimiento ?? DateTime.Now
                }).ToList(),
                Productos = productos,
                FiltroProductoId = productoId?.ToString(),
                FiltroTipo = tipo,
                FiltroOrigen = origen,
                FiltroFecha = fecha?.ToString("yyyy-MM-dd")
            };
        }

        public async Task<(bool Success, string Message)> AjusteManualAsync(
            AjusteManualViewModel vm, int usuarioId)
        {
            var producto = await _db.Productos.FindAsync(vm.ProductoId);
            if (producto == null)
                return (false, "Producto no encontrado.");

            var stockAnterior = (decimal)(producto.StockActual ?? 0);
            decimal stockNuevo;

            if (vm.Tipo == "ENTRADA")
            {
                stockNuevo = stockAnterior + vm.Cantidad;
                producto.StockActual = (int)stockNuevo;
            }
            else if (vm.Tipo == "SALIDA")
            {
                if (vm.Cantidad > stockAnterior)
                    return (false, $"Stock insuficiente. Stock actual: {stockAnterior}");
                stockNuevo = stockAnterior - vm.Cantidad;
                producto.StockActual = (int)stockNuevo;
            }
            else // AJUSTE
            {
                stockNuevo = vm.Cantidad; // El ajuste establece el stock directamente
                producto.StockActual = (int)stockNuevo;
            }

            var kardex = new Kardex
            {
                ProductoId = vm.ProductoId,
                UsuarioId = usuarioId,
                Tipo = vm.Tipo == "AJUSTE" ? "AJUSTE" : vm.Tipo,
                Origen = "AJUSTE_MANUAL",
                Entrada = vm.Tipo == "ENTRADA" ? vm.Cantidad : 0,
                Salida = vm.Tipo == "SALIDA" ? vm.Cantidad : 0,
                StockAnterior = stockAnterior,
                StockResultante = stockNuevo,
                CostoUnitario = vm.CostoUnitario,
                Descripcion = vm.Descripcion,
                FechaMovimiento = DateTime.Now
            };

            await _repo.CreateAsync(kardex);
            await _db.SaveChangesAsync();

            return (true, $"Ajuste registrado. Stock actualizado: {stockNuevo}");
        }
    }
}
