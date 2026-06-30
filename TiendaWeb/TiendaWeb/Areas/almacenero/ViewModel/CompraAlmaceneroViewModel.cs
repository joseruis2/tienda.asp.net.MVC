namespace TiendaWeb.Areas.almacenero.ViewModel
{
    public class CompraAlmaceneroListViewModel
    {
        public int CompraId { get; set; }
        public string NumeroOrden { get; set; } = "";
        public string ProveedorNombre { get; set; } = "";
        public string? TipoDocProveedor { get; set; }
        public string? NroDocProveedor { get; set; }
        public DateTime FechaCompra { get; set; }
        public DateOnly? FechaEntrega { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = "";
        public string? UsuarioNombre { get; set; }
        public bool PuedeRecibir => Estado == "PENDIENTE" || Estado == "PARCIAL";
        public bool EsUrgente =>
            FechaEntrega.HasValue &&
            FechaEntrega.Value <= DateOnly.FromDateTime(DateTime.Now.AddDays(2));
    }

    public class CompraAlmaceneroDetalleViewModel
    {
        public int CompraId { get; set; }
        public string NumeroOrden { get; set; } = "";
        public string ProveedorNombre { get; set; } = "";
        public string? TipoDocProveedor { get; set; }
        public string? NroDocProveedor { get; set; }
        public DateTime FechaCompra { get; set; }
        public DateOnly? FechaEntrega { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; } = "";
        public string? Observaciones { get; set; }
        public List<CompraAlmaceneroItemViewModel> Items { get; set; } = new();
    }

    public class CompraAlmaceneroItemViewModel
    {
        public int DetalleId { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = "";
        public decimal Cantidad { get; set; }
        public decimal CantidadRecibida { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
        public decimal CantidadPendiente => Cantidad - CantidadRecibida;
        public bool EstaCompleto => CantidadPendiente <= 0;
    }

    public class RecepcionAlmaceneroViewModel
    {
        public int CompraId { get; set; }
        public string NumeroOrden { get; set; } = "";
        public string Proveedor { get; set; } = "";
        public DateOnly? FechaEntrega { get; set; }
        public List<RecepcionAlmaceneroItemViewModel> Items { get; set; } = new();
    }

    public class RecepcionAlmaceneroItemViewModel
    {
        public int DetalleId { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = "";
        public string? Ubicacion { get; set; }
        public int StockActual { get; set; }
        public decimal Cantidad { get; set; }
        public decimal CantidadRecibida { get; set; }
        public decimal CantidadPendiente => Cantidad - CantidadRecibida;
        public decimal CantidadARecibir { get; set; }
    }

    public class ComprasAlmaceneroIndexViewModel
    {
        public List<CompraAlmaceneroListViewModel> Lista { get; set; } = new();
        public List<CompraAlmaceneroListViewModel> Pendientes { get; set; } = new();
        public string? FiltroEstado { get; set; }
        public int CountPendientes { get; set; }
        public int CountParciales { get; set; }
    }
}
