using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Areas.admin.ViewModel
{
    public class ClienteListViewModel
    {
        public int ClienteId { get; set; }
        public string? Codigo { get; set; }
        public string? TipoDocumento { get; set; }
        public string? NumeroDocumento { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? RazonSocial { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public int? PuntosAcumulados { get; set; }
        public decimal? TotalCompras { get; set; }
        public bool? Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public string NombreCompleto => !string.IsNullOrEmpty(RazonSocial)
            ? RazonSocial
            : $"{Nombres} {Apellidos}".Trim();

        public bool EsEmpresa => TipoDocumento == "RUC";
    }

    public class ClienteCreateViewModel
    {
        [Display(Name = "Código")]
        [StringLength(20)]
        public string? Codigo { get; set; }

        [Required(ErrorMessage = "El tipo de documento es obligatorio")]
        [Display(Name = "Tipo documento")]
        public string TipoDocumento { get; set; } = "DNI";

        [Display(Name = "Número documento")]
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

        [Display(Name = "Dirección")]
        [StringLength(256)]
        public string? Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [StringLength(20)]
        public string? Telefono { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        public DateOnly? FechaNacimiento { get; set; }
    }

    public class ClienteEditViewModel
    {
        public int ClienteId { get; set; }

        [Display(Name = "Código")]
        [StringLength(20)]
        public string? Codigo { get; set; }

        [Required(ErrorMessage = "El tipo de documento es obligatorio")]
        [Display(Name = "Tipo documento")]
        public string TipoDocumento { get; set; } = "DNI";

        [Display(Name = "Número documento")]
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

        [Display(Name = "Dirección")]
        [StringLength(256)]
        public string? Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [StringLength(20)]
        public string? Telefono { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        [DataType(DataType.Date)]
        public DateOnly? FechaNacimiento { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; } = true;
    }

    public class ClientesIndexViewModel
    {
        public List<ClienteListViewModel> Lista { get; set; } = new();
        public ClienteCreateViewModel Crear { get; set; } = new();
        public ClienteEditViewModel Editar { get; set; } = new();
    }
}
