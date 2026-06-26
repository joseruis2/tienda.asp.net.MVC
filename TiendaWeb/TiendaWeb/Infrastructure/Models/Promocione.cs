using System;
using System.Collections.Generic;

namespace TiendaWeb.Infrastructure.Models;

public partial class Promocione
{
    public int PromoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string Tipo { get; set; } = null!;

    public decimal Valor { get; set; }

    public string? AplicaA { get; set; }

    public int? ProductoId { get; set; }

    public int? CategoriaId { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public TimeOnly? HoraInicio { get; set; }

    public TimeOnly? HoraFin { get; set; }

    public decimal? MontoMinimo { get; set; }

    public int? CantidadMinima { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Categoria? Categoria { get; set; }

    public virtual Producto? Producto { get; set; }
}
