using TiendaWeb.Areas.almacenero.repository;
using TiendaWeb.Areas.almacenero.ViewModel;

namespace TiendaWeb.Areas.almacenero.service
{
    public class DashboardAlmaceneroService : IDashboardAlmaceneroService
    {
        private readonly IDashboardAlmaceneroRepository _repo;
        public DashboardAlmaceneroService(IDashboardAlmaceneroRepository repo)
            => _repo = repo;

        public async Task<DashboardAlmaceneroViewModel> GetDashboardDataAsync()
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            var en15dias = hoy.AddDays(15);

            var sinStock = await _repo.GetSinStockAsync();
            var stockBajo = await _repo.GetStockBajoAsync();
            var porVencer = await _repo.GetPorVencerAsync(en15dias);
            var compras = await _repo.GetComprasPendientesAsync();
            var movimientos = await _repo.GetUltimosMovimientosAsync(10);
            var total = await _repo.CountTotalProductosAsync();

            return new DashboardAlmaceneroViewModel
            {
                TotalProductos = total,
                ProductosSinStock = sinStock.Count,
                ProductosStockBajo = stockBajo.Count,
                ProductosPorVencer = porVencer.Count,
                ComprasPendientes = compras.Count,

                SinStock = sinStock.Select(p => new AlertaStockViewModel
                {
                    ProductoId = p.ProductoId,
                    Codigo = p.Codigo,
                    Nombre = p.Nombre,
                    CategoriaNombre = p.Categoria?.Nombre,
                    ProveedorNombre = p.Proveedor?.RazonSocial,
                    StockActual = p.StockActual ?? 0,
                    StockMinimo = p.StockMinimo ?? 5,
                    StockMaximo = p.StockMaximo ?? 100,
                    UnidadMedida = p.UnidadMedida ?? "Unidad",
                    Ubicacion = p.Ubicacion
                }).ToList(),

                StockBajo = stockBajo.Select(p => new AlertaStockViewModel
                {
                    ProductoId = p.ProductoId,
                    Codigo = p.Codigo,
                    Nombre = p.Nombre,
                    CategoriaNombre = p.Categoria?.Nombre,
                    ProveedorNombre = p.Proveedor?.RazonSocial,
                    StockActual = p.StockActual ?? 0,
                    StockMinimo = p.StockMinimo ?? 5,
                    StockMaximo = p.StockMaximo ?? 100,
                    UnidadMedida = p.UnidadMedida ?? "Unidad",
                    Ubicacion = p.Ubicacion
                }).ToList(),

                PorVencer = porVencer.Select(p => new AlertaVencimientoViewModel
                {
                    ProductoId = p.ProductoId,
                    Codigo = p.Codigo,
                    Nombre = p.Nombre,
                    StockActual = p.StockActual ?? 0,
                    FechaVencimiento = p.FechaVencimiento!.Value,
                    DiasRestantes = p.FechaVencimiento!.Value.DayNumber - hoy.DayNumber
                }).ToList(),

                Compras = compras.Select(c => new ComprasPendienteViewModel
                {
                    CompraId = c.CompraId,
                    NumeroOrden = c.NumeroOrden,
                    ProveedorNombre = c.Proveedor?.RazonSocial ?? "",
                    Total = c.Total,
                    Estado = c.Estado ?? "PENDIENTE",
                    FechaEntrega = c.FechaEntrega
                }).ToList(),

                UltimosMovimientos = movimientos.Select(k => new UltimoMovimientoViewModel
                {
                    ProductoNombre = k.Producto?.Nombre ?? "",
                    Tipo = k.Tipo,
                    Origen = k.Origen,
                    Entrada = k.Entrada ?? 0,
                    Salida = k.Salida ?? 0,
                    StockResultante = k.StockResultante,
                    FechaMovimiento = k.FechaMovimiento ?? DateTime.Now
                }).ToList()
            };
        }
    }
}
