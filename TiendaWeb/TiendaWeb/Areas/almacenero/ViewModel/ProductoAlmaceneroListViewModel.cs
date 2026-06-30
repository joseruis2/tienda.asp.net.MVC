namespace TiendaWeb.Areas.almacenero.ViewModel
{
    public class ProductoAlmaceneroListViewModel
    {
        public int ProductoId { get; set; }
        public string Codigo { get; set; } = "";
        public string? CodigoBarras { get; set; }
        public string Nombre { get; set; } = "";
        public string? CategoriaNombre { get; set; }
        public string? ProveedorNombre { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public int StockMaximo { get; set; }
        public string UnidadMedida { get; set; } = "Unidad";
        public string? Ubicacion { get; set; }
        public DateOnly? FechaVencimiento { get; set; }
        public bool? Estado { get; set; }
        public bool StockBajo => StockActual <= StockMinimo;
        public bool StockCritico => StockActual == 0;
        public int PorcentajeStock =>
            StockMaximo > 0
                ? Math.Min(100, (int)(StockActual * 100.0 / StockMaximo))
                : 0;
    }

    public class ProductosAlmaceneroIndexViewModel
    {
        public List<ProductoAlmaceneroListViewModel> Lista { get; set; } = new();
        public List<ProductoAlmaceneroListViewModel> StockBajo { get; set; } = new();
        public List<ProductoAlmaceneroListViewModel> StockCritico { get; set; } = new();
        public string? FiltroBusqueda { get; set; }
        public string? FiltroCategoria { get; set; }
        public string? FiltroStock { get; set; }
        public List<string> Categorias { get; set; } = new();
        // Métricas
        public int TotalProductos { get; set; }
        public int ProductosStockBajo { get; set; }
        public int ProductosSinStock { get; set; }
        public int ProductosPorVencer { get; set; }
    }
}
