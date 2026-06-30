using TiendaWeb.Areas.almacenero.repository;
using TiendaWeb.Areas.almacenero.ViewModel;

namespace TiendaWeb.Areas.almacenero.service
{
    public class KardexAlmaceneroService : IKardexAlmaceneroService
    {
        private readonly IKardexAlmaceneroRepository _repo;
        public KardexAlmaceneroService(IKardexAlmaceneroRepository repo) => _repo = repo;

        public async Task<KardexAlmaceneroIndexViewModel> GetIndexDataAsync(
            int? productoId, string? tipo, string? origen, DateTime? fecha)
        {
            var kardex = await _repo.GetAllWithFiltersAsync(productoId, tipo, origen, fecha);
            var productos = await _repo.GetProductosAsync();

            var lista = kardex.Select(k => new KardexAlmaceneroListViewModel
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
            }).ToList();

            return new KardexAlmaceneroIndexViewModel
            {
                Lista = lista,
                Productos = productos.Select(p => new SelectAlmaceneroItem
                {
                    Id = p.Id,
                    Nombre = $"{p.Codigo} — {p.Nombre}"
                }).ToList(),
                FiltroProductoId = productoId?.ToString(),
                FiltroTipo = tipo,
                FiltroOrigen = origen,
                FiltroFecha = fecha?.ToString("yyyy-MM-dd"),
                TotalEntradas = lista.Sum(k => k.Entrada),
                TotalSalidas = lista.Sum(k => k.Salida),
                TotalMovimientos = lista.Count
            };
        }
    }
}
