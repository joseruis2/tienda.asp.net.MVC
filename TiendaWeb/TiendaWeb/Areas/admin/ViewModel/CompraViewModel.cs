using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Areas.admin.ViewModel
{
    // ── LISTA ──
    public class CompraListViewModel
    {
        public int CompraId { get; set; }
        public string NumeroOrden { get; set; } = "";
        public string ProveedorNombre { get; set; } = "";
        public string? TipoDocProveedor { get; set; }
        public string? NroDocProveedor { get; set; }
        public DateTime FechaCompra { get; set; }
        public DateOnly? FechaEntrega { get; set; }
        public decimal Total { get; set; }
        public string CondicionPago { get; set; } = "";
        public DateOnly? FechaVencimiento { get; set; }
        public string Estado { get; set; } = "";
        public string? UsuarioNombre { get; set; }
        public bool PuedeRecibir => Estado == "PENDIENTE" || Estado == "PARCIAL";
        public bool PuedeAnular => Estado != "ANULADA";
    }

    // ── CREAR ──
    public class CompraCreateViewModel
    {
        [Required(ErrorMessage = "El proveedor es obligatorio")]
        [Display(Name = "Proveedor")]
        public int ProveedorId { get; set; }

        [Display(Name = "Tipo doc. proveedor")]
        public string? TipoDocProveedor { get; set; } = "FACTURA";

        [Display(Name = "N° doc. proveedor")]
        [StringLength(30)]
        public string? NroDocProveedor { get; set; }

        [Display(Name = "Fecha entrega")]
        [DataType(DataType.Date)]
        public DateOnly? FechaEntrega { get; set; }

        [Display(Name = "Condición de pago")]
        public string CondicionPago { get; set; } = "CONTADO";

        [Display(Name = "Fecha vencimiento")]
        [DataType(DataType.Date)]
        public DateOnly? FechaVencimiento { get; set; }

        [Display(Name = "Descuento global")]
        [Range(0, 999999)]
        public decimal Descuento { get; set; } = 0;

        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        public List<CompraItemViewModel> Items { get; set; } = new();
    }

    public class CompraItemViewModel
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = "";
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; } = 0;
    }

    // ── DETALLE ──
    public class CompraDetalleViewModel
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
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public string CondicionPago { get; set; } = "";
        public DateOnly? FechaVencimiento { get; set; }
        public string Estado { get; set; } = "";
        public string? UsuarioNombre { get; set; }
        public string? Observaciones { get; set; }
        public List<CompraDetalleItemViewModel> Items { get; set; } = new();
    }

    public class CompraDetalleItemViewModel
    {
        public int DetalleId { get; set; }
        public string NombreProducto { get; set; } = "";
        public decimal Cantidad { get; set; }
        public decimal CantidadRecibida { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public decimal Pendiente => Cantidad - CantidadRecibida;
    }

    // ── RECEPCIÓN ──
    public class RecepcionCompraViewModel
    {
        public int CompraId { get; set; }
        public string NumeroOrden { get; set; } = "";
        public string Proveedor { get; set; } = "";
        public List<RecepcionItemViewModel> Items { get; set; } = new();
    }

    public class RecepcionItemViewModel
    {
        public int DetalleId { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = "";
        public decimal Cantidad { get; set; }
        public decimal CantidadRecibida { get; set; }
        public decimal CantidadPendiente => Cantidad - CantidadRecibida;

        [Range(0, 999999)]
        [Display(Name = "Cantidad a recibir")]
        public decimal CantidadARecibir { get; set; }
    }

    // ── ANULAR ──
    public class AnularCompraViewModel
    {
        public int CompraId { get; set; }

        [Required(ErrorMessage = "El motivo es obligatorio")]
        [Display(Name = "Motivo")]
        [StringLength(256)]
        public string Motivo { get; set; } = null!;
    }

    // ── INDEX ──
    public class ComprasIndexViewModel
    {
        public List<CompraListViewModel> Lista { get; set; } = new();
        public List<SelectItemViewModel> Proveedores { get; set; } = new();
        public List<ProductoPosDto> Productos { get; set; } = new();
        public string? FiltroEstado { get; set; }
        public decimal TotalPendiente { get; set; }
        public int CountPendiente { get; set; }
    }
}
