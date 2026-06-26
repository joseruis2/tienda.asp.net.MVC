using System;
using System.Collections.Generic;

namespace TiendaWeb.Infrastructure.Models;

public partial class DetalleVenta
{
    public int DetalleId { get; set; }

    public int VentaId { get; set; }

    public int ProductoId { get; set; }

    public string? CodigoBarras { get; set; }

    public string NombreProducto { get; set; } = null!;

    public decimal? Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal? Descuento { get; set; }

    public decimal Subtotal { get; set; }

    public decimal? Igv { get; set; }

    public decimal Total { get; set; }

    public virtual Producto Producto { get; set; } = null!;

    public virtual Venta Venta { get; set; } = null!;
}
