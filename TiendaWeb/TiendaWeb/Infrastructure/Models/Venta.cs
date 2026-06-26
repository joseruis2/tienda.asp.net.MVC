using System;
using System.Collections.Generic;

namespace TiendaWeb.Infrastructure.Models;

public partial class Venta
{
    public int VentaId { get; set; }

    public string NumeroTicket { get; set; } = null!;

    public string? TipoComprobante { get; set; }

    public string? Serie { get; set; }

    public int NumeroCorrelativo { get; set; }

    public DateTime? FechaVenta { get; set; }

    public int? ClienteId { get; set; }

    public string? ClienteNombre { get; set; }

    public string? ClienteDocumento { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Igv { get; set; }

    public decimal? Descuento { get; set; }

    public decimal Total { get; set; }

    public string? MetodoPago { get; set; }

    public decimal? MontoPagado { get; set; }

    public decimal? Vuelto { get; set; }

    public int? UsuarioId { get; set; }

    public int? CajaId { get; set; }

    public int? SesionId { get; set; }

    public string? Estado { get; set; }

    public string? Observaciones { get; set; }

    public DateTime? FechaAnulacion { get; set; }

    public string? MotivoAnulacion { get; set; }

    public int? UsuarioAnulacion { get; set; }

    public virtual Caja? Caja { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual ICollection<Devolucione> Devoluciones { get; set; } = new List<Devolucione>();

    public virtual ICollection<MovimientosCaja> MovimientosCajas { get; set; } = new List<MovimientosCaja>();

    public virtual SesionesCaja? Sesion { get; set; }

    public virtual Usuario? Usuario { get; set; }

    public virtual Usuario? UsuarioAnulacionNavigation { get; set; }
}
