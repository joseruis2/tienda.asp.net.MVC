using System;
using System.Collections.Generic;

namespace TiendaWeb.Infrastructure.Models;

public partial class ConfiguracionNegocio
{
    public int ConfigId { get; set; }

    public string NombreNegocio { get; set; } = null!;

    public string? Ruc { get; set; }

    public string? Direccion { get; set; }

    public string? Distrito { get; set; }

    public string? Provincia { get; set; }

    public string? Departamento { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? MensajeTicket { get; set; }

    public string? LogoUrl { get; set; }

    public decimal? IgvPorcentaje { get; set; }

    public string? Moneda { get; set; }

    public string? SimboloMoneda { get; set; }

    public string? SerieTicket { get; set; }

    public string? SerieBoleta { get; set; }

    public string? SerieFactura { get; set; }

    public int? CorrelativoTicket { get; set; }

    public int? CorrelativoBoleta { get; set; }

    public int? CorrelativoFactura { get; set; }

    public string? SerieOrdenCompra { get; set; }

    public int? DiasDevolucion { get; set; }

    public DateTime? FechaActualizacion { get; set; }
}
