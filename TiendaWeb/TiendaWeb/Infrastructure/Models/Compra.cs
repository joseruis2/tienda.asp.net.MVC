using System;
using System.Collections.Generic;

namespace TiendaWeb.Infrastructure.Models;

public partial class Compra
{
    public int CompraId { get; set; }

    public string NumeroOrden { get; set; } = null!;

    public string? Serie { get; set; }

    public int NumeroCorrelativo { get; set; }

    public int ProveedorId { get; set; }

    public string? TipoDocProveedor { get; set; }

    public string? NroDocProveedor { get; set; }

    public DateTime? FechaCompra { get; set; }

    public DateOnly? FechaEntrega { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Igv { get; set; }

    public decimal? Descuento { get; set; }

    public decimal Total { get; set; }

    public string? CondicionPago { get; set; }

    public DateOnly? FechaVencimiento { get; set; }

    public string? Estado { get; set; }

    public int? UsuarioId { get; set; }

    public string? Observaciones { get; set; }

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual Proveedore Proveedor { get; set; } = null!;

    public virtual Usuario? Usuario { get; set; }
}
