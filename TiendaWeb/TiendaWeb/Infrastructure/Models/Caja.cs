using System;
using System.Collections.Generic;

namespace TiendaWeb.Infrastructure.Models;

public partial class Caja
{
    public int CajaId { get; set; }

    public string Nombre { get; set; } = null!;

    public int Numero { get; set; }

    public bool? Activa { get; set; }

    public string? Estado { get; set; }

    public int? SesionActual { get; set; }

    public virtual ICollection<MovimientosCaja> MovimientosCajas { get; set; } = new List<MovimientosCaja>();

    public virtual SesionesCaja? SesionActualNavigation { get; set; }

    public virtual ICollection<SesionesCaja> SesionesCajas { get; set; } = new List<SesionesCaja>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
