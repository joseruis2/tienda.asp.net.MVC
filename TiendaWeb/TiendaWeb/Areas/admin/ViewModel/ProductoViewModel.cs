using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Areas.admin.ViewModel
{
    public class ProductoListViewModel
    {
        public int ProductoId { get; set; }
        public string Codigo { get; set; } = "";
        public string? CodigoBarras { get; set; }
        public string Nombre { get; set; } = "";
        public string? CategoriaNombre { get; set; }
        public string? ProveedorNombre { get; set; }
        public decimal PrecioCosto { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioMayorista { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public bool? Estado { get; set; }
        public bool StockBajo => StockActual <= StockMinimo;
    }

    public class ProductoCreateViewModel
    {
        [Required(ErrorMessage = "El código es obligatorio")]
        [Display(Name = "Código")]
        [StringLength(50)]
        public string Codigo { get; set; } = null!;

        [Display(Name = "Código de barras")]
        [StringLength(50)]
        public string? CodigoBarras { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(256)]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Descripción")]
        [StringLength(1000)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        [Display(Name = "Proveedor")]
        public int? ProveedorId { get; set; }

        [Display(Name = "Precio costo")]
        [Range(0, 9999999)]
        public decimal PrecioCosto { get; set; } = 0;

        [Display(Name = "Precio venta")]
        [Range(0, 9999999)]
        public decimal Precio { get; set; } = 0;

        [Display(Name = "Precio mayorista")]
        [Range(0, 9999999)]
        public decimal PrecioMayorista { get; set; } = 0;

        [Display(Name = "Stock inicial")]
        [Range(0, 999999)]
        public int StockActual { get; set; } = 0;

        [Display(Name = "Stock mínimo")]
        [Range(0, 999999)]
        public int StockMinimo { get; set; } = 5;

        [Display(Name = "Stock máximo")]
        [Range(0, 999999)]
        public int StockMaximo { get; set; } = 100;

        [Display(Name = "Unidad de medida")]
        [StringLength(20)]
        public string UnidadMedida { get; set; } = "Unidad";

        [Display(Name = "Peso (kg)")]
        [Range(0, 99999)]
        public decimal Peso { get; set; } = 0;

        [Display(Name = "Ubicación")]
        [StringLength(50)]
        public string? Ubicacion { get; set; }

        [Display(Name = "Fecha de vencimiento")]
        [DataType(DataType.Date)]
        public DateOnly? FechaVencimiento { get; set; }

        [Display(Name = "Imagen (URL)")]
        [StringLength(256)]
        public string? ImagenPrincipal { get; set; }
    }

    public class ProductoEditViewModel
    {
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [Display(Name = "Código")]
        [StringLength(50)]
        public string Codigo { get; set; } = null!;

        [Display(Name = "Código de barras")]
        [StringLength(50)]
        public string? CodigoBarras { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(256)]
        public string Nombre { get; set; } = null!;

        [Display(Name = "Descripción")]
        [StringLength(1000)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        [Display(Name = "Proveedor")]
        public int? ProveedorId { get; set; }

        [Display(Name = "Precio costo")]
        [Range(0, 9999999)]
        public decimal PrecioCosto { get; set; }

        [Display(Name = "Precio venta")]
        [Range(0, 9999999)]
        public decimal Precio { get; set; }

        [Display(Name = "Precio mayorista")]
        [Range(0, 9999999)]
        public decimal PrecioMayorista { get; set; }

        [Display(Name = "Stock mínimo")]
        [Range(0, 999999)]
        public int StockMinimo { get; set; }

        [Display(Name = "Stock máximo")]
        [Range(0, 999999)]
        public int StockMaximo { get; set; }

        [Display(Name = "Unidad de medida")]
        [StringLength(20)]
        public string UnidadMedida { get; set; } = "Unidad";

        [Display(Name = "Peso (kg)")]
        [Range(0, 99999)]
        public decimal Peso { get; set; }

        [Display(Name = "Ubicación")]
        [StringLength(50)]
        public string? Ubicacion { get; set; }

        [Display(Name = "Fecha de vencimiento")]
        [DataType(DataType.Date)]
        public DateOnly? FechaVencimiento { get; set; }

        [Display(Name = "Imagen (URL)")]
        [StringLength(256)]
        public string? ImagenPrincipal { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; } = true;
    }

    public class ProductosIndexViewModel
    {
        public List<ProductoListViewModel> Lista { get; set; } = new();
        public ProductoCreateViewModel Crear { get; set; } = new();
        public ProductoEditViewModel Editar { get; set; } = new();
        // Para los selects
        public List<SelectItemViewModel> Categorias { get; set; } = new();
        public List<SelectItemViewModel> Proveedores { get; set; } = new();
    }

    public class SelectItemViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
    }
}
