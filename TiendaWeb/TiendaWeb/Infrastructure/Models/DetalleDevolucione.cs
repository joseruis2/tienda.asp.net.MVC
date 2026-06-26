using System;
using System.Collections.Generic;

namespace TiendaWeb.Infrastructure.Models;

public partial class DetalleDevolucione
{
    public int DetalleId { get; set; }

    public int DevolucionId { get; set; }

    public int ProductoId { get; set; }

    public string NombreProducto { get; set; } = null!;

    public decimal Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Total { get; set; }

    public bool? RegresaStock { get; set; }

    public virtual Devolucione Devolucion { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
