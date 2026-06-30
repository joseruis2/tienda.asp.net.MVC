using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Areas.admin.ViewModel
{
    public class KardexListViewModel
    {
        public int KardexId { get; set; }
        public string ProductoNombre { get; set; } = "";
        public string ProductoCodigo { get; set; } = "";
        public string UsuarioNombre { get; set; } = "";
        public string Tipo { get; set; } = "";
        public string Origen { get; set; } = "";
        public int? ReferenciaId { get; set; }
        public string? ReferenciaTipo { get; set; }
        public decimal Entrada { get; set; }
        public decimal Salida { get; set; }
        public decimal StockAnterior { get; set; }
        public decimal StockResultante { get; set; }
        public decimal CostoUnitario { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaMovimiento { get; set; }
    }

    public class AjusteManualViewModel
    {
        [Required(ErrorMessage = "El producto es obligatorio")]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "El tipo es obligatorio")]
        [Display(Name = "Tipo de ajuste")]
        public string Tipo { get; set; } = "ENTRADA";

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Display(Name = "Cantidad")]
        [Range(0.001, 999999, ErrorMessage = "Debe ser mayor a 0")]
        public decimal Cantidad { get; set; }

        [Display(Name = "Costo unitario")]
        [Range(0, 999999)]
        public decimal CostoUnitario { get; set; } = 0;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción / Motivo")]
        [StringLength(255)]
        public string Descripcion { get; set; } = null!;
    }

    public class KardexIndexViewModel
    {
        public List<KardexListViewModel> Lista { get; set; } = new();
        public AjusteManualViewModel Ajuste { get; set; } = new();
        public List<SelectItemViewModel> Productos { get; set; } = new();
        // Filtros
        public string? FiltroProductoId { get; set; }
        public string? FiltroTipo { get; set; }
        public string? FiltroOrigen { get; set; }
        public string? FiltroFecha { get; set; }
    }
}
