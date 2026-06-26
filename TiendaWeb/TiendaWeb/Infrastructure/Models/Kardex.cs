using System;
using System.Collections.Generic;

namespace TiendaWeb.Infrastructure.Models;

public partial class Kardex
{
    public int KardexId { get; set; }

    public int ProductoId { get; set; }

    public int UsuarioId { get; set; }

    public string Tipo { get; set; } = null!;

    public string Origen { get; set; } = null!;

    public int? ReferenciaId { get; set; }

    public string? ReferenciaTipo { get; set; }

    public decimal? Entrada { get; set; }

    public decimal? Salida { get; set; }

    public decimal StockAnterior { get; set; }

    public decimal StockResultante { get; set; }

    public decimal? CostoUnitario { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaMovimiento { get; set; }

    public virtual Producto Producto { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
