using System;
using System.Collections.Generic;

namespace TiendaWeb.Infrastructure.Models;

public partial class Devolucione
{
    public int DevolucionId { get; set; }

    public string Numero { get; set; } = null!;

    public int VentaId { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Motivo { get; set; }

    public decimal TotalDevuelto { get; set; }

    public string? TipoDevolucion { get; set; }

    public int? UsuarioId { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<DetalleDevolucione> DetalleDevoluciones { get; set; } = new List<DetalleDevolucione>();

    public virtual Usuario? Usuario { get; set; }

    public virtual Venta Venta { get; set; } = null!;
}
