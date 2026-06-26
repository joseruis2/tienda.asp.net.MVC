using System;
using System.Collections.Generic;

namespace TiendaWeb.Infrastructure.Models;

public partial class DetalleCompra
{
    public int DetalleId { get; set; }

    public int CompraId { get; set; }

    public int ProductoId { get; set; }

    public string NombreProducto { get; set; } = null!;

    public decimal Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal? Descuento { get; set; }

    public decimal Subtotal { get; set; }

    public decimal? Igv { get; set; }

    public decimal Total { get; set; }

    public decimal? CantidadRecibida { get; set; }

    public virtual Compra Compra { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
