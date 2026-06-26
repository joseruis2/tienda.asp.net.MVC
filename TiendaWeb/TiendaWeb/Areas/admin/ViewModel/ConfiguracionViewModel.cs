using System.ComponentModel.DataAnnotations;

namespace TiendaWeb.Areas.admin.ViewModel
{
    public class ConfiguracionViewModel
    {
        public int ConfigId { get; set; }

        [Required(ErrorMessage = "El nombre del negocio es obligatorio")]
        [Display(Name = "Nombre del negocio")]
        public string NombreNegocio { get; set; } = null!;

        [Display(Name = "RUC")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "El RUC debe tener 11 dígitos")]
        public string? Ruc { get; set; }

        [Display(Name = "Dirección")]
        public string? Direccion { get; set; }

        [Display(Name = "Distrito")]
        public string? Distrito { get; set; }

        [Display(Name = "Provincia")]
        public string? Provincia { get; set; }

        [Display(Name = "Departamento")]
        public string? Departamento { get; set; }

        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? Email { get; set; }

        [Display(Name = "Mensaje en ticket")]
        public string? MensajeTicket { get; set; }

        [Display(Name = "IGV (%)")]
        [Range(0, 100, ErrorMessage = "El IGV debe estar entre 0 y 100")]
        public decimal IgvPorcentaje { get; set; } = 18;

        [Display(Name = "Moneda")]
        public string Moneda { get; set; } = "PEN";

        [Display(Name = "Símbolo")]
        public string SimboloMoneda { get; set; } = "S/";

        // Series comprobantes
        [Display(Name = "Serie Ticket")]
        public string SerieTicket { get; set; } = "T001";

        [Display(Name = "Serie Boleta")]
        public string SerieBoleta { get; set; } = "B001";

        [Display(Name = "Serie Factura")]
        public string SerieFactura { get; set; } = "F001";

        [Display(Name = "Serie Orden Compra")]
        public string SerieOrdenCompra { get; set; } = "OC01";

        [Display(Name = "Días para devolución")]
        [Range(0, 365)]
        public int DiasDevolucion { get; set; } = 7;
    }
}
