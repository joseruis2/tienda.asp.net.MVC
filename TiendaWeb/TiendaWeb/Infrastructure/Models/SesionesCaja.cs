using System;
using System.Collections.Generic;

namespace TiendaWeb.Infrastructure.Models;

public partial class SesionesCaja
{
    public int SesionId { get; set; }

    public int CajaId { get; set; }

    public int UsuarioId { get; set; }

    public decimal? MontoApertura { get; set; }

    public decimal? MontoCierre { get; set; }

    public decimal? MontoSistema { get; set; }

    public decimal? Diferencia { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaApertura { get; set; }

    public DateTime? FechaCierre { get; set; }

    public string? Observaciones { get; set; }

    public virtual Caja Caja { get; set; } = null!;

    public virtual ICollection<Caja> Cajas { get; set; } = new List<Caja>();

    public virtual ICollection<MovimientosCaja> MovimientosCajas { get; set; } = new List<MovimientosCaja>();

    public virtual Usuario Usuario { get; set; } = null!;

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
