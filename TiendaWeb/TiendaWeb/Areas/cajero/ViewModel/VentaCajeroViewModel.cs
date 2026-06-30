using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Areas.cajero.ViewModel
{
    public class PosCajeroViewModel
    {
        public List<ProductoPosCajeroDto> Productos { get; set; } = new();
        public List<SelectCajeroItem> Clientes { get; set; } = new();
        public string SerieTicket { get; set; } = "T001";
        public string SerieBoleta { get; set; } = "B001";
        public string SerieFactura { get; set; } = "F001";
        public decimal IgvPorcentaje { get; set; } = 18;
        public bool TieneSesionAbierta { get; set; }
        public string? CajaNombre { get; set; }
        public int? CajaNumero { get; set; }
    }

    public class ProductoPosCajeroDto
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

    public class SelectCajeroItem
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
    }

    public class VentaCajeroCreateViewModel
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

        public List<VentaCajeroItemViewModel> Items { get; set; } = new();
    }

    public class VentaCajeroItemViewModel
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = "";
        public string? CodigoBarras { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; } = 0;
    }
}
