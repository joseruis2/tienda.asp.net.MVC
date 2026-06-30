using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Areas.cajero.ViewModel
{
    public class ClienteBusquedaDto
    {
        public int ClienteId { get; set; }
        public string? Codigo { get; set; }
        public string? TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }
        public string NombreCompleto { get; set; } = "";
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public int? PuntosAcumulados { get; set; }
        public decimal? TotalCompras { get; set; }
    }

    public class ClienteRapidoCreateViewModel
    {
        [Required(ErrorMessage = "El tipo de documento es obligatorio")]
        [Display(Name = "Tipo documento")]
        public string TipoDocumento { get; set; } = "DNI";

        [Display(Name = "N° documento")]
        [StringLength(20)]
        public string? NumeroDocumento { get; set; }

        [Display(Name = "Nombres")]
        [StringLength(100)]
        public string? Nombres { get; set; }

        [Display(Name = "Apellidos")]
        [StringLength(100)]
        public string? Apellidos { get; set; }

        [Display(Name = "Razón social")]
        [StringLength(256)]
        public string? RazonSocial { get; set; }

        [Display(Name = "Teléfono")]
        [StringLength(20)]
        public string? Telefono { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100)]
        public string? Email { get; set; }
    }

    public class ClienteRapidoEditViewModel
    {
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El tipo de documento es obligatorio")]
        [Display(Name = "Tipo documento")]
        public string TipoDocumento { get; set; } = "DNI";

        [Display(Name = "N° documento")]
        [StringLength(20)]
        public string? NumeroDocumento { get; set; }

        [Display(Name = "Nombres")]
        [StringLength(100)]
        public string? Nombres { get; set; }

        [Display(Name = "Apellidos")]
        [StringLength(100)]
        public string? Apellidos { get; set; }

        [Display(Name = "Razón social")]
        [StringLength(256)]
        public string? RazonSocial { get; set; }

        [Display(Name = "Teléfono")]
        [StringLength(20)]
        public string? Telefono { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100)]
        public string? Email { get; set; }
    }

    public class ClientesCajeroIndexViewModel
    {
        public List<ClienteBusquedaDto> Lista { get; set; } = new();
        public ClienteRapidoCreateViewModel Crear { get; set; } = new();
        public ClienteRapidoEditViewModel Editar { get; set; } = new();
        public string? Busqueda { get; set; }
        public int Total { get; set; }
    }
}
