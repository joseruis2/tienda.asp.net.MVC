using System;
using System.Collections.Generic;

namespace TiendaWeb.Infrastructure.Models;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public string? Codigo { get; set; }

    public string? TipoDocumento { get; set; }

    public string? NumeroDocumento { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public string? RazonSocial { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public int? PuntosAcumulados { get; set; }

    public decimal? TotalCompras { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
