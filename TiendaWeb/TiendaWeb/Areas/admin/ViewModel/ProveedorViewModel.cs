using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Areas.admin.ViewModel
{
    public class ProveedorListViewModel
    {
        public int ProveedorId { get; set; }
        public string Codigo { get; set; } = "";
        public string RazonSocial { get; set; } = "";
        public string? NombreComercial { get; set; }
        public string? TipoDocumento { get; set; }
        public string RucDni { get; set; } = "";
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? ContactoNombre { get; set; }
        public string? ContactoTelefono { get; set; }
        public int? DiasCredito { get; set; }
        public decimal? LimiteCredito { get; set; }
        public decimal? SaldoCredito { get; set; }
        public bool? Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    public class ProveedorCreateViewModel
    {
        [Required(ErrorMessage = "El código es obligatorio")]
        [Display(Name = "Código")]
        [StringLength(20)]
        public string Codigo { get; set; } = null!;

        [Required(ErrorMessage = "La razón social es obligatoria")]
        [Display(Name = "Razón social")]
        [StringLength(256)]
        public string RazonSocial { get; set; } = null!;

        [Display(Name = "Nombre comercial")]
        [StringLength(256)]
        public string? NombreComercial { get; set; }

        [Display(Name = "Tipo documento")]
        public string TipoDocumento { get; set; } = "RUC";

        [Required(ErrorMessage = "El RUC/DNI es obligatorio")]
        [Display(Name = "RUC / DNI")]
        [StringLength(20)]
        public string RucDni { get; set; } = null!;

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

        [Display(Name = "Contacto nombre")]
        [StringLength(100)]
        public string? ContactoNombre { get; set; }

        [Display(Name = "Contacto teléfono")]
        [StringLength(20)]
        public string? ContactoTelefono { get; set; }

        [Display(Name = "Días de crédito")]
        [Range(0, 365)]
        public int DiasCredito { get; set; } = 0;

        [Display(Name = "Límite de crédito")]
        [Range(0, 9999999)]
        public decimal LimiteCredito { get; set; } = 0;
    }

    public class ProveedorEditViewModel
    {
        public int ProveedorId { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [Display(Name = "Código")]
        [StringLength(20)]
        public string Codigo { get; set; } = null!;

        [Required(ErrorMessage = "La razón social es obligatoria")]
        [Display(Name = "Razón social")]
        [StringLength(256)]
        public string RazonSocial { get; set; } = null!;

        [Display(Name = "Nombre comercial")]
        [StringLength(256)]
        public string? NombreComercial { get; set; }

        [Display(Name = "Tipo documento")]
        public string TipoDocumento { get; set; } = "RUC";

        [Required(ErrorMessage = "El RUC/DNI es obligatorio")]
        [Display(Name = "RUC / DNI")]
        [StringLength(20)]
        public string RucDni { get; set; } = null!;

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

        [Display(Name = "Contacto nombre")]
        [StringLength(100)]
        public string? ContactoNombre { get; set; }

        [Display(Name = "Contacto teléfono")]
        [StringLength(20)]
        public string? ContactoTelefono { get; set; }

        [Display(Name = "Días de crédito")]
        [Range(0, 365)]
        public int DiasCredito { get; set; } = 0;

        [Display(Name = "Límite de crédito")]
        [Range(0, 9999999)]
        public decimal LimiteCredito { get; set; } = 0;

        [Display(Name = "Estado")]
        public bool Estado { get; set; } = true;
    }

    public class ProveedoresIndexViewModel
    {
        public List<ProveedorListViewModel> Lista { get; set; } = new();
        public ProveedorCreateViewModel Crear { get; set; } = new();
        public ProveedorEditViewModel Editar { get; set; } = new();
    }
}
