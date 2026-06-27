using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Areas.admin.ViewModel
{
    // ── LISTA ──
    public class UsuarioListViewModel
    {
        public int UsuarioId { get; set; }
        public string Username { get; set; } = "";
        public string NombreCompleto { get; set; } = "";
        public string? Dni { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Rol { get; set; }
        public bool? Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? UltimoAcceso { get; set; }
    }

    // ── CREAR ──
    public class UsuarioCreateViewModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [Display(Name = "Username")]
        [StringLength(50)]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [Display(Name = "Contraseña")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mínimo 6 caracteres")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirma la contraseña")]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; } = null!;

        [Display(Name = "DNI")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos")]
        public string? Dni { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? Email { get; set; }

        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        [Display(Name = "Rol")]
        public string Rol { get; set; } = "CAJERO";
    }

    // ── EDITAR ──
    public class UsuarioEditViewModel
    {
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [Display(Name = "Username")]
        [StringLength(50)]
        public string Username { get; set; } = null!;

        [Display(Name = "Nueva contraseña")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mínimo 6 caracteres")]
        public string? NewPassword { get; set; }

        [Display(Name = "Confirmar nueva contraseña")]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string? ConfirmNewPassword { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios")]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; } = null!;

        [Display(Name = "DNI")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos")]
        public string? Dni { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? Email { get; set; }

        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio")]
        [Display(Name = "Rol")]
        public string Rol { get; set; } = "CAJERO";

        [Display(Name = "Estado")]
        public bool Estado { get; set; } = true;
    }

    // ── INDEX (contenedor de la página) ──
    public class UsuariosIndexViewModel
    {
        public List<UsuarioListViewModel> Lista { get; set; } = new();
        public UsuarioCreateViewModel Crear { get; set; } = new();
        public UsuarioEditViewModel Editar { get; set; } = new();
    }
}
