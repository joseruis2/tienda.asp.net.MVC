using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Areas.admin.ViewModel
{
    public class CajaListViewModel
    {
        public int CajaId { get; set; }
        public string Nombre { get; set; } = "";
        public int Numero { get; set; }
        public bool? Activa { get; set; }
        public string Estado { get; set; } = "CERRADA";
        public int? SesionActual { get; set; }
        public bool EstaAbierta => Estado == "ABIERTA";
    }

    public class CajaCreateViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(50)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El número es obligatorio")]
        [Display(Name = "Número de caja")]
        [Range(1, 999, ErrorMessage = "Debe ser entre 1 y 999")]
        public int Numero { get; set; }

        [Display(Name = "Habilitada")]
        public bool Activa { get; set; } = true;
    }

    public class CajaEditViewModel
    {
        public int CajaId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(50)]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El número es obligatorio")]
        [Display(Name = "Número de caja")]
        [Range(1, 999, ErrorMessage = "Debe ser entre 1 y 999")]
        public int Numero { get; set; }

        [Display(Name = "Habilitada")]
        public bool Activa { get; set; } = true;
    }

    public class CajasIndexViewModel
    {
        public List<CajaListViewModel> Lista { get; set; } = new();
        public CajaCreateViewModel Crear { get; set; } = new();
        public CajaEditViewModel Editar { get; set; } = new();
    }
}
