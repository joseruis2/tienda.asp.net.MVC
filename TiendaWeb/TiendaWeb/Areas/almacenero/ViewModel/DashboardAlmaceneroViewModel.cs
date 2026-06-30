namespace TiendaWeb.Areas.almacenero.ViewModel
{
    public class DashboardAlmaceneroViewModel
    {
        // Métricas principales
        public int TotalProductos { get; set; }
        public int ProductosSinStock { get; set; }
        public int ProductosStockBajo { get; set; }
        public int ProductosPorVencer { get; set; }
        public int ComprasPendientes { get; set; }

        // Listas de alertas
        public List<AlertaStockViewModel> SinStock { get; set; } = new();
        public List<AlertaStockViewModel> StockBajo { get; set; } = new();
        public List<AlertaVencimientoViewModel> PorVencer { get; set; } = new();
        public List<ComprasPendienteViewModel> Compras { get; set; } = new();

        // Últimos movimientos del día
        public List<UltimoMovimientoViewModel> UltimosMovimientos { get; set; } = new();
    }

    public class AlertaStockViewModel
    {
        public int ProductoId { get; set; }
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string? CategoriaNombre { get; set; }
        public string? ProveedorNombre { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public int StockMaximo { get; set; }
        public string UnidadMedida { get; set; } = "Unidad";
        public string? Ubicacion { get; set; }
        public int PorcentajeStock =>
            StockMaximo > 0
                ? Math.Min(100, (int)(StockActual * 100.0 / StockMaximo))
                : 0;
    }

    public class AlertaVencimientoViewModel
    {
        public int ProductoId { get; set; }
        public string Codigo { get; set; } = "";
        public string Nombre { get; set; } = "";
        public int StockActual { get; set; }
        public DateOnly FechaVencimiento { get; set; }
        public int DiasRestantes { get; set; }
        public bool EsUrgente => DiasRestantes <= 3;
        public bool EsVencido => DiasRestantes < 0;
    }

    public class ComprasPendienteViewModel
    {
        public int CompraId { get; set; }
        public string NumeroOrden { get; set; } = "";
        public string ProveedorNombre { get; set; } = "";
        public decimal Total { get; set; }
        public string Estado { get; set; } = "";
        public DateOnly? FechaEntrega { get; set; }
        public bool EsUrgente =>
            FechaEntrega.HasValue &&
            FechaEntrega.Value <= DateOnly.FromDateTime(DateTime.Now.AddDays(2));
    }

    public class UltimoMovimientoViewModel
    {
        public string ProductoNombre { get; set; } = "";
        public string Tipo { get; set; } = "";
        public string Origen { get; set; } = "";
        public decimal Entrada { get; set; }
        public decimal Salida { get; set; }
        public decimal StockResultante { get; set; }
        public DateTime FechaMovimiento { get; set; }
    }
}
