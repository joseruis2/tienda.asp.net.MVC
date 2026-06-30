namespace TiendaWeb.Areas.cajero.ViewModel
{
    public class DashboardCajeroViewModel
    {
        // Estado de caja
        public bool TieneSesionAbierta { get; set; }
        public int? SesionId { get; set; }
        public string? CajaNombre { get; set; }
        public int? CajaNumero { get; set; }
        public decimal MontoApertura { get; set; }
        public DateTime? FechaApertura { get; set; }

        // Ventas del día (de esta sesión)
        public decimal VentasHoy { get; set; }
        public int TransaccionesHoy { get; set; }
        public decimal EfectivoEnCaja { get; set; }
        public decimal VentasEfectivo { get; set; }
        public decimal VentasTarjeta { get; set; }
        public decimal VentasYape { get; set; }
        public decimal VentasPlin { get; set; }
        public decimal VentasTransferencia { get; set; }

        // Lista últimas ventas de esta sesión
        public List<UltimaVentaCajeroViewModel> UltimasVentas { get; set; } = new();

        // Cajas disponibles para abrir
        public List<CajaDisponibleViewModel> CajasDisponibles { get; set; } = new();
    }

    public class UltimaVentaCajeroViewModel
    {
        public string NumeroTicket { get; set; } = "";
        public string? ClienteNombre { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; } = "";
        public string Estado { get; set; } = "";
        public DateTime FechaVenta { get; set; }
    }

    public class CajaDisponibleViewModel
    {
        public int CajaId { get; set; }
        public string Nombre { get; set; } = "";
        public int Numero { get; set; }
    }

    public class AbrirCajaCajeroViewModel
    {
        public int CajaId { get; set; }
        public decimal MontoApertura { get; set; }
        public string? Observaciones { get; set; }
    }

    public class CerrarCajaCajeroViewModel
    {
        public int SesionId { get; set; }
        public string CajaNombre { get; set; } = "";
        public int CajaNumero { get; set; }
        public decimal MontoApertura { get; set; }
        public decimal MontoSistema { get; set; }
        public DateTime FechaApertura { get; set; }
        public decimal MontoCierre { get; set; }
        public string? Observaciones { get; set; }
    }
}
