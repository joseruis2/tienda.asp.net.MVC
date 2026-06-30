using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Areas.admin.ViewModel
{
    public class DevolucionListViewModel
    {
        public int DevolucionId { get; set; }
        public string Numero { get; set; } = "";
        public int VentaId { get; set; }
        public string NumeroTicket { get; set; } = "";
        public string? ClienteNombre { get; set; }
        public DateTime Fecha { get; set; }
        public string? Motivo { get; set; }
        public decimal TotalDevuelto { get; set; }
        public string TipoDevolucion { get; set; } = "";
        public string Estado { get; set; } = "";
        public string? UsuarioNombre { get; set; }
        public bool PuedeAnular => Estado == "PROCESADA";
    }

    public class DevolucionCreateViewModel
    {
        // Datos de la venta origen
        public int VentaId { get; set; }
        public string NumeroTicket { get; set; } = "";
        public string? ClienteNombre { get; set; }
        public decimal TotalVenta { get; set; }

        [Required(ErrorMessage = "El tipo de devolución es obligatorio")]
        [Display(Name = "Tipo de devolución")]
        public string TipoDevolucion { get; set; } = "EFECTIVO";

        [Display(Name = "Motivo")]
        [StringLength(256)]
        public string? Motivo { get; set; }

        public List<DevolucionItemViewModel> Items { get; set; } = new();
    }

    public class DevolucionItemViewModel
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = "";
        public decimal CantidadVendida { get; set; }
        public decimal PrecioUnitario { get; set; }

        [Range(0, 999999)]
        [Display(Name = "Cantidad a devolver")]
        public decimal CantidadDevolver { get; set; } = 0;

        [Display(Name = "Regresa al stock")]
        public bool RegresaStock { get; set; } = true;
    }

    public class DevolucionDetalleViewModel
    {
        public int DevolucionId { get; set; }
        public string Numero { get; set; } = "";
        public string NumeroTicket { get; set; } = "";
        public string? ClienteNombre { get; set; }
        public DateTime Fecha { get; set; }
        public string? Motivo { get; set; }
        public decimal TotalDevuelto { get; set; }
        public string TipoDevolucion { get; set; } = "";
        public string Estado { get; set; } = "";
        public string? UsuarioNombre { get; set; }
        public List<DevolucionDetalleItemViewModel> Items { get; set; } = new();
    }

    public class DevolucionDetalleItemViewModel
    {
        public string NombreProducto { get; set; } = "";
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
        public bool RegresaStock { get; set; }
    }

    public class AnularDevolucionViewModel
    {
        public int DevolucionId { get; set; }
    }

    public class DevolucionesIndexViewModel
    {
        public List<DevolucionListViewModel> Lista { get; set; } = new();
        public string? FiltroFecha { get; set; }
        public int TotalProcesadas { get; set; }
        public decimal MontoDevuelto { get; set; }
    }

    // Para buscar venta y crear devolución
    public class BuscarVentaViewModel
    {
        public string? NumeroTicket { get; set; }
        public VentaParaDevolucionDto? Venta { get; set; }
    }

    public class VentaParaDevolucionDto
    {
        public int VentaId { get; set; }
        public string NumeroTicket { get; set; } = "";
        public string? ClienteNombre { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaVenta { get; set; }
        public List<ItemVentaDto> Items { get; set; } = new();
    }

    public class ItemVentaDto
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = "";
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
    }
}
