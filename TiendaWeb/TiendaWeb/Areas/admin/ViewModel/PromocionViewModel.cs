using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Areas.admin.ViewModel
{
    public class PromocionListViewModel
    {
        public int PromoId { get; set; }
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public string Tipo { get; set; } = "";
        public decimal Valor { get; set; }
        public string AplicaA { get; set; } = "";
        public string? ProductoNombre { get; set; }
        public string? CategoriaNombre { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
        public TimeOnly? HoraInicio { get; set; }
        public TimeOnly? HoraFin { get; set; }
        public decimal MontoMinimo { get; set; }
        public int CantidadMinima { get; set; }
        public bool? Estado { get; set; }
        public bool EsVigente =>
            Estado == true &&
            (!FechaInicio.HasValue || FechaInicio <= DateOnly.FromDateTime(DateTime.Now)) &&
            (!FechaFin.HasValue || FechaFin >= DateOnly.FromDateTime(DateTime.Now));
    }

    public class PromocionCreateViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Descripción")]
        [StringLength(256)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El tipo es obligatorio")]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; } = "PORCENTAJE";

        [Required(ErrorMessage = "El valor es obligatorio")]
        [Display(Name = "Valor")]
        [Range(0.01, 999999, ErrorMessage = "Debe ser mayor a 0")]
        public decimal Valor { get; set; }

        [Display(Name = "Aplica a")]
        public string AplicaA { get; set; } = "PRODUCTO";

        [Display(Name = "Producto")]
        public int? ProductoId { get; set; }

        [Display(Name = "Categoría")]
        public int? CategoriaId { get; set; }

        [Display(Name = "Fecha inicio")]
        [DataType(DataType.Date)]
        public DateOnly? FechaInicio { get; set; }

        [Display(Name = "Fecha fin")]
        [DataType(DataType.Date)]
        public DateOnly? FechaFin { get; set; }

        [Display(Name = "Hora inicio")]
        [DataType(DataType.Time)]
        public TimeOnly? HoraInicio { get; set; }

        [Display(Name = "Hora fin")]
        [DataType(DataType.Time)]
        public TimeOnly? HoraFin { get; set; }

        [Display(Name = "Monto mínimo")]
        [Range(0, 999999)]
        public decimal MontoMinimo { get; set; } = 0;

        [Display(Name = "Cantidad mínima")]
        [Range(1, 9999)]
        public int CantidadMinima { get; set; } = 1;
    }

    public class PromocionEditViewModel
    {
        public int PromoId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Descripción")]
        [StringLength(256)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El tipo es obligatorio")]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; } = "PORCENTAJE";

        [Required(ErrorMessage = "El valor es obligatorio")]
        [Display(Name = "Valor")]
        [Range(0.01, 999999)]
        public decimal Valor { get; set; }

        [Display(Name = "Aplica a")]
        public string AplicaA { get; set; } = "PRODUCTO";

        [Display(Name = "Producto")]
        public int? ProductoId { get; set; }

        [Display(Name = "Categoría")]
        public int? CategoriaId { get; set; }

        [Display(Name = "Fecha inicio")]
        [DataType(DataType.Date)]
        public DateOnly? FechaInicio { get; set; }

        [Display(Name = "Fecha fin")]
        [DataType(DataType.Date)]
        public DateOnly? FechaFin { get; set; }

        [Display(Name = "Hora inicio")]
        [DataType(DataType.Time)]
        public TimeOnly? HoraInicio { get; set; }

        [Display(Name = "Hora fin")]
        [DataType(DataType.Time)]
        public TimeOnly? HoraFin { get; set; }

        [Display(Name = "Monto mínimo")]
        [Range(0, 999999)]
        public decimal MontoMinimo { get; set; } = 0;

        [Display(Name = "Cantidad mínima")]
        [Range(1, 9999)]
        public int CantidadMinima { get; set; } = 1;

        [Display(Name = "Estado")]
        public bool Estado { get; set; } = true;
    }

    public class PromocionesIndexViewModel
    {
        public List<PromocionListViewModel> Lista { get; set; } = new();
        public PromocionCreateViewModel Crear { get; set; } = new();
        public PromocionEditViewModel Editar { get; set; } = new();
        public List<SelectItemViewModel> Productos { get; set; } = new();
        public List<SelectItemViewModel> Categorias { get; set; } = new();
    }
}
