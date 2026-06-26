using System;
using System.Collections.Generic;

namespace TiendaWeb.Infrastructure.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? Dni { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? Rol { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? UltimoAcceso { get; set; }

    public int? CreadoPor { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual Usuario? CreadoPorNavigation { get; set; }

    public virtual ICollection<Devolucione> Devoluciones { get; set; } = new List<Devolucione>();

    public virtual ICollection<Usuario> InverseCreadoPorNavigation { get; set; } = new List<Usuario>();

    public virtual ICollection<Kardex> Kardices { get; set; } = new List<Kardex>();

    public virtual ICollection<MovimientosCaja> MovimientosCajas { get; set; } = new List<MovimientosCaja>();

    public virtual ICollection<SesionesCaja> SesionesCajas { get; set; } = new List<SesionesCaja>();

    public virtual ICollection<Venta> VentaUsuarioAnulacionNavigations { get; set; } = new List<Venta>();

    public virtual ICollection<Venta> VentaUsuarios { get; set; } = new List<Venta>();
}
