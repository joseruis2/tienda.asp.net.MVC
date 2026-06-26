namespace TiendaWeb.Areas.admin.ViewModel
{
    public class DashboardViewModel
    {
        // Tarjetas
        public decimal VentasHoy { get; set; }
        public int TransaccionesHoy { get; set; }
        public int ProductosStockBajo { get; set; }
        public int ClientesNuevosHoy { get; set; }
        public int ComprasPendientes { get; set; }
        public string EstadoCaja { get; set; } = "CERRADA";
        public decimal MontoCaja { get; set; }

        // Tabla últimas ventas
        public List<UltimaVentaDto> UltimasVentas { get; set; } = new();
    }

    public class UltimaVentaDto
    {
        public string NumeroTicket { get; set; } = "";
        public string ClienteNombre { get; set; } = "";
        public decimal Total { get; set; }
        public string MetodoPago { get; set; } = "";
        public string Estado { get; set; } = "";
        public DateTime? FechaVenta { get; set; }
    }
}
