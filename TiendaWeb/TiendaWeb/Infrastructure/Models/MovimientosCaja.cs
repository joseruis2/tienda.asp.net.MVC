using System;
using System.Collections.Generic;

namespace TiendaWeb.Infrastructure.Models;

public partial class MovimientosCaja
{
    public int MovimientoId { get; set; }

    public int SesionId { get; set; }

    public int CajaId { get; set; }

    public string TipoMovimiento { get; set; } = null!;

    public int? VentaId { get; set; }

    public decimal Monto { get; set; }

    public decimal? MontoAnterior { get; set; }

    public decimal? MontoNuevo { get; set; }

    public string? Descripcion { get; set; }

    public int? UsuarioId { get; set; }

    public DateTime? FechaMovimiento { get; set; }

    public virtual Caja Caja { get; set; } = null!;

    public virtual SesionesCaja Sesion { get; set; } = null!;

    public virtual Usuario? Usuario { get; set; }

    public virtual Venta? Venta { get; set; }
}
