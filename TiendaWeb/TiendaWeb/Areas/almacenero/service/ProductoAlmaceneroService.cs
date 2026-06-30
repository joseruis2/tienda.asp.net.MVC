using TiendaWeb.Areas.almacenero.repository;
using TiendaWeb.Areas.almacenero.ViewModel;

namespace TiendaWeb.Areas.almacenero.service
{
    public class ProductoAlmaceneroService : IProductoAlmaceneroService
    {
        private readonly IProductoAlmaceneroRepository _repo;

        public ProductoAlmaceneroService(IProductoAlmaceneroRepository repo)
        {
            _repo = repo;
        }

        public async Task<ProductosAlmaceneroIndexViewModel> GetIndexDataAsync(
            string? busqueda, string? categoria, string? filtroStock)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            var en7dias = hoy.AddDays(7);

            var productos = await _repo.GetAllActivosAsync(busqueda, categoria, filtroStock);
            var categorias = await _repo.GetCategoriasAsync();

            var lista = productos.Select(p => new ProductoAlmaceneroListViewModel
            {
                ProductoId = p.ProductoId,
                Codigo = p.Codigo,
                CodigoBarras = p.CodigoBarras,
                Nombre = p.Nombre,
                CategoriaNombre = p.Categoria?.Nombre,
                ProveedorNombre = p.Proveedor?.RazonSocial,
                StockActual = p.StockActual ?? 0,
                StockMinimo = p.StockMinimo ?? 5,
                StockMaximo = p.StockMaximo ?? 100,
                UnidadMedida = p.UnidadMedida ?? "Unidad",
                Ubicacion = p.Ubicacion,
                FechaVencimiento = p.FechaVencimiento,
                Estado = p.Estado
            }).ToList();

            return new ProductosAlmaceneroIndexViewModel
            {
                Lista = lista,
                StockBajo = lista.Where(p => p.StockBajo && !p.StockCritico).ToList(),
                StockCritico = lista.Where(p => p.StockCritico).ToList(),
                FiltroBusqueda = busqueda,
                FiltroCategoria = categoria,
                FiltroStock = filtroStock,
                Categorias = categorias,
                TotalProductos = lista.Count,
                ProductosStockBajo = await _repo.CountStockBajoAsync(),
                ProductosSinStock = await _repo.CountSinStockAsync(),
                ProductosPorVencer = await _repo.CountPorVencerAsync(en7dias)
            };
        }
    }
}
