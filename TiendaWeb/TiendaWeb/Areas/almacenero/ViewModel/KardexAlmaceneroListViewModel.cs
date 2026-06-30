namespace TiendaWeb.Areas.almacenero.ViewModel
{
    public class KardexAlmaceneroListViewModel
    {
        public int KardexId { get; set; }
        public string ProductoNombre { get; set; } = "";
        public string ProductoCodigo { get; set; } = "";
        public string UsuarioNombre { get; set; } = "";
        public string Tipo { get; set; } = "";
        public string Origen { get; set; } = "";
        public int? ReferenciaId { get; set; }
        public string? ReferenciaTipo { get; set; }
        public decimal Entrada { get; set; }
        public decimal Salida { get; set; }
        public decimal StockAnterior { get; set; }
        public decimal StockResultante { get; set; }
        public decimal CostoUnitario { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaMovimiento { get; set; }
    }

    public class KardexAlmaceneroIndexViewModel
    {
        public List<KardexAlmaceneroListViewModel> Lista { get; set; } = new();
        public List<SelectAlmaceneroItem> Productos { get; set; } = new();
        // Filtros
        public string? FiltroProductoId { get; set; }
        public string? FiltroTipo { get; set; }
        public string? FiltroOrigen { get; set; }
        public string? FiltroFecha { get; set; }
        // Métricas del filtro actual
        public decimal TotalEntradas { get; set; }
        public decimal TotalSalidas { get; set; }
        public int TotalMovimientos { get; set; }
    }

    public class SelectAlmaceneroItem
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
    }
}
