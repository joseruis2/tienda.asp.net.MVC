using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Areas.admin.ViewModel
{
    // ── POS ──
    public class PosViewModel
    {
        public List<ProductoPosDto> Productos { get; set; } = new();
        public List<SelectItemViewModel> Clientes { get; set; } = new();
        public string SerieTicket { get; set; } = "T001";
        public string SerieBoleta { get; set; } = "B001";
        public string SerieFactura { get; set; } = "F001";
        public decimal IgvPorcentaje { get; set; } = 18;
    }

    public class ProductoPosDto
    {
        public int ProductoId { get; set; }
        public string Codigo { get; set; } = "";
        public string? CodigoBarras { get; set; }
        public string Nombre { get; set; } = "";
        public decimal Precio { get; set; }
        public decimal PrecioMayorista { get; set; }
        public int StockActual { get; set; }
        public string UnidadMedida { get; set; } = "Unidad";
    }

    // ── CREAR VENTA ──
    public class VentaCreateViewModel
    {
        [Required]
        public string TipoComprobante { get; set; } = "TICKET";
        public string Serie { get; set; } = "T001";
        public int? ClienteId { get; set; }
        public string? ClienteNombre { get; set; }
        public string? ClienteDocumento { get; set; }

        [Required]
        public string MetodoPago { get; set; } = "EFECTIVO";
        public decimal MontoPagado { get; set; }
        public decimal Descuento { get; set; } = 0;
        public string? Observaciones { get; set; }

        public List<VentaItemViewModel> Items { get; set; } = new();
    }

    public class VentaItemViewModel
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = "";
        public string? CodigoBarras { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; } = 0;
    }

    // ── LISTA HISTORIAL ──
    public class VentaListViewModel
    {
        public int VentaId { get; set; }
        public string NumeroTicket { get; set; } = "";
        public string TipoComprobante { get; set; } = "";
        public string? ClienteNombre { get; set; }
        public string? ClienteDocumento { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; } = "";
        public decimal MontoPagado { get; set; }
        public decimal Vuelto { get; set; }
        public string Estado { get; set; } = "";
        public DateTime FechaVenta { get; set; }
        public string? UsuarioNombre { get; set; }
        public bool PuedeAnular => Estado == "COMPLETADA";
    }

    // ── DETALLE VENTA ──
    public class VentaDetalleViewModel
    {
        public int VentaId { get; set; }
        public string NumeroTicket { get; set; } = "";
        public string TipoComprobante { get; set; } = "";
        public string Serie { get; set; } = "";
        public int NumeroCorrelativo { get; set; }
        public string? ClienteNombre { get; set; }
        public string? ClienteDocumento { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; } = "";
        public decimal MontoPagado { get; set; }
        public decimal Vuelto { get; set; }
        public string Estado { get; set; } = "";
        public DateTime FechaVenta { get; set; }
        public string? UsuarioNombre { get; set; }
        public string? Observaciones { get; set; }
        public string? MotivoAnulacion { get; set; }
        public DateTime? FechaAnulacion { get; set; }
        public List<VentaDetalleItemViewModel> Items { get; set; } = new();
    }

    public class VentaDetalleItemViewModel
    {
        public string NombreProducto { get; set; } = "";
        public string? CodigoBarras { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
    }

    // ── ANULAR ──
    public class AnularVentaViewModel
    {
        public int VentaId { get; set; }

        [Required(ErrorMessage = "El motivo es obligatorio")]
        [Display(Name = "Motivo de anulación")]
        [StringLength(256)]
        public string MotivoAnulacion { get; set; } = null!;
    }

    // ── HISTORIAL INDEX ──
    public class VentasHistorialViewModel
    {
        public List<VentaListViewModel> Lista { get; set; } = new();
        public string? FiltroFecha { get; set; }
        public string? FiltroEstado { get; set; }
        public string? FiltroMetodo { get; set; }
        public decimal TotalDia { get; set; }
        public int TransaccionesDia { get; set; }
    }
}
