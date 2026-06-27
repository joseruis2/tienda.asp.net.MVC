using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Areas.admin.ViewModel
{
    public class CategoriaListViewModel
    {
        public int CategoriaId { get; set; }
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public string? Imagen { get; set; }
        public bool? Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }

    public class CategoriaCreateViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(256)]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Descripción")]
        [StringLength(256)]
        public string? Descripcion { get; set; }

        [Display(Name = "Imagen (URL)")]
        [StringLength(255)]
        public string? Imagen { get; set; }
    }

    public class CategoriaEditViewModel
    {
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(256)]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Descripción")]
        [StringLength(256)]
        public string? Descripcion { get; set; }

        [Display(Name = "Imagen (URL)")]
        [StringLength(255)]
        public string? Imagen { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; } = true;
    }

    public class CategoriasIndexViewModel
    {
        public List<CategoriaListViewModel> Lista { get; set; } = new();
        public CategoriaCreateViewModel Crear { get; set; } = new();
        public CategoriaEditViewModel Editar { get; set; } = new();
    }
}
