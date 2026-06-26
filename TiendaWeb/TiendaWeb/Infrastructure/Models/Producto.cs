using System;
using System.Collections.Generic;

namespace TiendaWeb.Infrastructure.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public string Codigo { get; set; } = null!;

    public string? CodigoBarras { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int CategoriaId { get; set; }

    public int? ProveedorId { get; set; }

    public decimal? PrecioCosto { get; set; }

    public decimal? Precio { get; set; }

    public decimal? PrecioMayorista { get; set; }

    public int? StockActual { get; set; }

    public int? StockMinimo { get; set; }

    public int? StockMaximo { get; set; }

    public string? UnidadMedida { get; set; }

    public decimal? Peso { get; set; }

    public string? Ubicacion { get; set; }

    public DateOnly? FechaVencimiento { get; set; }

    public string? ImagenPrincipal { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual ICollection<DetalleCompra> DetalleCompras { get; set; } = new List<DetalleCompra>();

    public virtual ICollection<DetalleDevolucione> DetalleDevoluciones { get; set; } = new List<DetalleDevolucione>();

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual ICollection<Kardex> Kardices { get; set; } = new List<Kardex>();

    public virtual ICollection<Promocione> Promociones { get; set; } = new List<Promocione>();

    public virtual Proveedore? Proveedor { get; set; }
}
