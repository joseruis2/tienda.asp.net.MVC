using System;
using System.Collections.Generic;

namespace TiendaWeb.Infrastructure.Models;

public partial class Proveedore
{
    public int ProveedorId { get; set; }

    public string Codigo { get; set; } = null!;

    public string RazonSocial { get; set; } = null!;

    public string? NombreComercial { get; set; }

    public string? TipoDocumento { get; set; }

    public string RucDni { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? ContactoNombre { get; set; }

    public string? ContactoTelefono { get; set; }

    public int? DiasCredito { get; set; }

    public decimal? LimiteCredito { get; set; }

    public decimal? SaldoCredito { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
