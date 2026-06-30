using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Areas.admin.ViewModel
{
    public class SesionCajaListViewModel
    {
        public int SesionId { get; set; }
        public string CajaNombre { get; set; } = "";
        public int CajaNumero { get; set; }
        public string UsuarioNombre { get; set; } = "";
        public decimal MontoApertura { get; set; }
        public decimal? MontoCierre { get; set; }
        public decimal? MontoSistema { get; set; }
        public decimal? Diferencia { get; set; }
        public string Estado { get; set; } = "ABIERTA";
        public DateTime FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string? Observaciones { get; set; }
        public bool EstaAbierta => Estado == "ABIERTA";
        public TimeSpan? Duracion => FechaCierre.HasValue
            ? FechaCierre - FechaApertura : DateTime.Now - FechaApertura;
    }

    public class AbrirCajaViewModel
    {
        [Required(ErrorMessage = "Selecciona una caja")]
        [Display(Name = "Caja")]
        public int CajaId { get; set; }

        [Required(ErrorMessage = "El monto de apertura es obligatorio")]
        [Display(Name = "Monto de apertura")]
        [Range(0, 999999, ErrorMessage = "Monto inválido")]
        public decimal MontoApertura { get; set; } = 0;

        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }
    }

    public class CerrarCajaViewModel
    {
        public int SesionId { get; set; }
        public string CajaNombre { get; set; } = "";
        public int CajaNumero { get; set; }
        public decimal MontoApertura { get; set; }
        public decimal MontoSistema { get; set; }  // calculado automático
        public DateTime FechaApertura { get; set; }

        [Required(ErrorMessage = "El monto contado es obligatorio")]
        [Display(Name = "Monto contado en caja")]
        [Range(0, 999999, ErrorMessage = "Monto inválido")]
        public decimal MontoCierre { get; set; }

        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        // Calculado en vista
        public decimal Diferencia => MontoCierre - MontoSistema;
    }

    public class SesionesCajaIndexViewModel
    {
        public List<SesionCajaListViewModel> Historial { get; set; } = new();
        public SesionCajaListViewModel? SesionActiva { get; set; }
        public AbrirCajaViewModel Abrir { get; set; } = new();
        public List<SelectItemViewModel> CajasDisponibles { get; set; } = new();
    }
}
