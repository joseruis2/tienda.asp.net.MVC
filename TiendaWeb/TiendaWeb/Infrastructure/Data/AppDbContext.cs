using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Infrastructure.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Caja> Cajas { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<ConfiguracionNegocio> ConfiguracionNegocios { get; set; }

    public virtual DbSet<DetalleCompra> DetalleCompras { get; set; }

    public virtual DbSet<DetalleDevolucione> DetalleDevoluciones { get; set; }

    public virtual DbSet<DetalleVenta> DetalleVentas { get; set; }

    public virtual DbSet<Devolucione> Devoluciones { get; set; }

    public virtual DbSet<Kardex> Kardices { get; set; }

    public virtual DbSet<MovimientosCaja> MovimientosCajas { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Promocione> Promociones { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<SesionesCaja> SesionesCajas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3307;database=tienda;user=root;password=123456", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.45-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Caja>(entity =>
        {
            entity.HasKey(e => e.CajaId).HasName("PRIMARY");

            entity.ToTable("cajas");

            entity.HasIndex(e => e.SesionActual, "fk_caja_sesion");

            entity.HasIndex(e => e.Numero, "numero").IsUnique();

            entity.Property(e => e.CajaId).HasColumnName("caja_id");
            entity.Property(e => e.Activa)
                .HasDefaultValueSql("'1'")
                .HasColumnName("activa");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'CERRADA'")
                .HasColumnType("enum('ABIERTA','CERRADA')")
                .HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Numero).HasColumnName("numero");
            entity.Property(e => e.SesionActual).HasColumnName("sesion_actual");

            entity.HasOne(d => d.SesionActualNavigation).WithMany(p => p.Cajas)
                .HasForeignKey(d => d.SesionActual)
                .HasConstraintName("fk_caja_sesion");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PRIMARY");

            entity.ToTable("categorias");

            entity.HasIndex(e => e.Nombre, "nombre").IsUnique();

            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(256)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'1'")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.Imagen)
                .HasMaxLength(255)
                .HasColumnName("imagen");
            entity.Property(e => e.Nombre)
                .HasMaxLength(256)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PRIMARY");

            entity.ToTable("clientes");

            entity.HasIndex(e => e.Codigo, "codigo").IsUnique();

            entity.HasIndex(e => e.NumeroDocumento, "idx_clientes_documento");

            entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .HasColumnName("apellidos");
            entity.Property(e => e.Codigo)
                .HasMaxLength(20)
                .HasColumnName("codigo");
            entity.Property(e => e.Direccion)
                .HasMaxLength(256)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'1'")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .HasColumnName("nombres");
            entity.Property(e => e.NumeroDocumento)
                .HasMaxLength(20)
                .HasColumnName("numero_documento");
            entity.Property(e => e.PuntosAcumulados)
                .HasDefaultValueSql("'0'")
                .HasColumnName("puntos_acumulados");
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(256)
                .HasColumnName("razon_social");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
            entity.Property(e => e.TipoDocumento)
                .HasDefaultValueSql("'DNI'")
                .HasColumnType("enum('DNI','RUC','CE','PASAPORTE','NINGUNO')")
                .HasColumnName("tipo_documento");
            entity.Property(e => e.TotalCompras)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("total_compras");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.CompraId).HasName("PRIMARY");

            entity.ToTable("compras");

            entity.HasIndex(e => e.Estado, "idx_compras_estado");

            entity.HasIndex(e => e.FechaCompra, "idx_compras_fecha");

            entity.HasIndex(e => e.ProveedorId, "idx_compras_proveedor");

            entity.HasIndex(e => e.NumeroOrden, "numero_orden").IsUnique();

            entity.HasIndex(e => e.UsuarioId, "usuario_id");

            entity.Property(e => e.CompraId).HasColumnName("compra_id");
            entity.Property(e => e.CondicionPago)
                .HasDefaultValueSql("'CONTADO'")
                .HasColumnType("enum('CONTADO','CREDITO')")
                .HasColumnName("condicion_pago");
            entity.Property(e => e.Descuento)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("descuento");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'PENDIENTE'")
                .HasColumnType("enum('PENDIENTE','RECIBIDA','PARCIAL','ANULADA')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCompra)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_compra");
            entity.Property(e => e.FechaEntrega).HasColumnName("fecha_entrega");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.Igv)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("igv");
            entity.Property(e => e.NroDocProveedor)
                .HasMaxLength(30)
                .HasColumnName("nro_doc_proveedor");
            entity.Property(e => e.NumeroCorrelativo).HasColumnName("numero_correlativo");
            entity.Property(e => e.NumeroOrden)
                .HasMaxLength(20)
                .HasColumnName("numero_orden");
            entity.Property(e => e.Observaciones)
                .HasColumnType("text")
                .HasColumnName("observaciones");
            entity.Property(e => e.ProveedorId).HasColumnName("proveedor_id");
            entity.Property(e => e.Serie)
                .HasMaxLength(4)
                .HasDefaultValueSql("'OC01'")
                .HasColumnName("serie");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("subtotal");
            entity.Property(e => e.TipoDocProveedor)
                .HasMaxLength(20)
                .HasColumnName("tipo_doc_proveedor");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Compras)
                .HasForeignKey(d => d.ProveedorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("compras_ibfk_1");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Compras)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("compras_ibfk_2");
        });

        modelBuilder.Entity<ConfiguracionNegocio>(entity =>
        {
            entity.HasKey(e => e.ConfigId).HasName("PRIMARY");

            entity.ToTable("configuracion_negocio");

            entity.Property(e => e.ConfigId).HasColumnName("config_id");
            entity.Property(e => e.CorrelativoBoleta)
                .HasDefaultValueSql("'1'")
                .HasColumnName("correlativo_boleta");
            entity.Property(e => e.CorrelativoFactura)
                .HasDefaultValueSql("'1'")
                .HasColumnName("correlativo_factura");
            entity.Property(e => e.CorrelativoTicket)
                .HasDefaultValueSql("'1'")
                .HasColumnName("correlativo_ticket");
            entity.Property(e => e.Departamento)
                .HasMaxLength(100)
                .HasColumnName("departamento");
            entity.Property(e => e.DiasDevolucion)
                .HasDefaultValueSql("'7'")
                .HasColumnName("dias_devolucion");
            entity.Property(e => e.Direccion)
                .HasMaxLength(256)
                .HasColumnName("direccion");
            entity.Property(e => e.Distrito)
                .HasMaxLength(100)
                .HasColumnName("distrito");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.FechaActualizacion)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_actualizacion");
            entity.Property(e => e.IgvPorcentaje)
                .HasPrecision(5, 2)
                .HasDefaultValueSql("'18.00'")
                .HasColumnName("igv_porcentaje");
            entity.Property(e => e.LogoUrl)
                .HasMaxLength(256)
                .HasColumnName("logo_url");
            entity.Property(e => e.MensajeTicket)
                .HasColumnType("text")
                .HasColumnName("mensaje_ticket");
            entity.Property(e => e.Moneda)
                .HasMaxLength(3)
                .HasDefaultValueSql("'PEN'")
                .HasColumnName("moneda");
            entity.Property(e => e.NombreNegocio)
                .HasMaxLength(256)
                .HasColumnName("nombre_negocio");
            entity.Property(e => e.Provincia)
                .HasMaxLength(100)
                .HasColumnName("provincia");
            entity.Property(e => e.Ruc)
                .HasMaxLength(11)
                .HasColumnName("ruc");
            entity.Property(e => e.SerieBoleta)
                .HasMaxLength(4)
                .HasDefaultValueSql("'B001'")
                .HasColumnName("serie_boleta");
            entity.Property(e => e.SerieFactura)
                .HasMaxLength(4)
                .HasDefaultValueSql("'F001'")
                .HasColumnName("serie_factura");
            entity.Property(e => e.SerieOrdenCompra)
                .HasMaxLength(4)
                .HasDefaultValueSql("'OC01'")
                .HasColumnName("serie_orden_compra");
            entity.Property(e => e.SerieTicket)
                .HasMaxLength(4)
                .HasDefaultValueSql("'T001'")
                .HasColumnName("serie_ticket");
            entity.Property(e => e.SimboloMoneda)
                .HasMaxLength(5)
                .HasDefaultValueSql("'S/'")
                .HasColumnName("simbolo_moneda");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<DetalleCompra>(entity =>
        {
            entity.HasKey(e => e.DetalleId).HasName("PRIMARY");

            entity.ToTable("detalle_compras");

            entity.HasIndex(e => e.CompraId, "compra_id");

            entity.HasIndex(e => e.ProductoId, "producto_id");

            entity.Property(e => e.DetalleId).HasColumnName("detalle_id");
            entity.Property(e => e.Cantidad)
                .HasPrecision(10, 3)
                .HasColumnName("cantidad");
            entity.Property(e => e.CantidadRecibida)
                .HasPrecision(10, 3)
                .HasDefaultValueSql("'0.000'")
                .HasColumnName("cantidad_recibida");
            entity.Property(e => e.CompraId).HasColumnName("compra_id");
            entity.Property(e => e.Descuento)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("descuento");
            entity.Property(e => e.Igv)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("igv");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(256)
                .HasColumnName("nombre_producto");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("precio_unitario");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 2)
                .HasColumnName("subtotal");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.Compra).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.CompraId)
                .HasConstraintName("detalle_compras_ibfk_1");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetalleCompras)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("detalle_compras_ibfk_2");
        });

        modelBuilder.Entity<DetalleDevolucione>(entity =>
        {
            entity.HasKey(e => e.DetalleId).HasName("PRIMARY");

            entity.ToTable("detalle_devoluciones");

            entity.HasIndex(e => e.DevolucionId, "devolucion_id");

            entity.HasIndex(e => e.ProductoId, "producto_id");

            entity.Property(e => e.DetalleId).HasColumnName("detalle_id");
            entity.Property(e => e.Cantidad)
                .HasPrecision(10, 3)
                .HasColumnName("cantidad");
            entity.Property(e => e.DevolucionId).HasColumnName("devolucion_id");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(256)
                .HasColumnName("nombre_producto");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("precio_unitario");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.RegresaStock)
                .HasDefaultValueSql("'1'")
                .HasColumnName("regresa_stock");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");

            entity.HasOne(d => d.Devolucion).WithMany(p => p.DetalleDevoluciones)
                .HasForeignKey(d => d.DevolucionId)
                .HasConstraintName("detalle_devoluciones_ibfk_1");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetalleDevoluciones)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("detalle_devoluciones_ibfk_2");
        });

        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.HasKey(e => e.DetalleId).HasName("PRIMARY");

            entity.ToTable("detalle_ventas");

            entity.HasIndex(e => e.ProductoId, "idx_detalle_ventas_producto");

            entity.HasIndex(e => e.VentaId, "idx_detalle_ventas_venta");

            entity.Property(e => e.DetalleId).HasColumnName("detalle_id");
            entity.Property(e => e.Cantidad)
                .HasPrecision(10, 3)
                .HasDefaultValueSql("'1.000'")
                .HasColumnName("cantidad");
            entity.Property(e => e.CodigoBarras)
                .HasMaxLength(50)
                .HasColumnName("codigo_barras");
            entity.Property(e => e.Descuento)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("descuento");
            entity.Property(e => e.Igv)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("igv");
            entity.Property(e => e.NombreProducto)
                .HasMaxLength(256)
                .HasColumnName("nombre_producto");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("precio_unitario");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 2)
                .HasColumnName("subtotal");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");
            entity.Property(e => e.VentaId).HasColumnName("venta_id");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("detalle_ventas_ibfk_2");

            entity.HasOne(d => d.Venta).WithMany(p => p.DetalleVenta)
                .HasForeignKey(d => d.VentaId)
                .HasConstraintName("detalle_ventas_ibfk_1");
        });

        modelBuilder.Entity<Devolucione>(entity =>
        {
            entity.HasKey(e => e.DevolucionId).HasName("PRIMARY");

            entity.ToTable("devoluciones");

            entity.HasIndex(e => e.Numero, "numero").IsUnique();

            entity.HasIndex(e => e.UsuarioId, "usuario_id");

            entity.HasIndex(e => e.VentaId, "venta_id");

            entity.Property(e => e.DevolucionId).HasColumnName("devolucion_id");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'PROCESADA'")
                .HasColumnType("enum('PROCESADA','ANULADA')")
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha");
            entity.Property(e => e.Motivo)
                .HasMaxLength(256)
                .HasColumnName("motivo");
            entity.Property(e => e.Numero)
                .HasMaxLength(20)
                .HasColumnName("numero");
            entity.Property(e => e.TipoDevolucion)
                .HasDefaultValueSql("'EFECTIVO'")
                .HasColumnType("enum('EFECTIVO','CAMBIO_PRODUCTO','NOTA_CREDITO')")
                .HasColumnName("tipo_devolucion");
            entity.Property(e => e.TotalDevuelto)
                .HasPrecision(10, 2)
                .HasColumnName("total_devuelto");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            entity.Property(e => e.VentaId).HasColumnName("venta_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Devoluciones)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("devoluciones_ibfk_2");

            entity.HasOne(d => d.Venta).WithMany(p => p.Devoluciones)
                .HasForeignKey(d => d.VentaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("devoluciones_ibfk_1");
        });

        modelBuilder.Entity<Kardex>(entity =>
        {
            entity.HasKey(e => e.KardexId).HasName("PRIMARY");

            entity.ToTable("kardex");

            entity.HasIndex(e => e.FechaMovimiento, "idx_kardex_fecha");

            entity.HasIndex(e => new { e.Origen, e.ReferenciaId }, "idx_kardex_origen");

            entity.HasIndex(e => e.ProductoId, "idx_kardex_producto");

            entity.HasIndex(e => e.UsuarioId, "usuario_id");

            entity.Property(e => e.KardexId).HasColumnName("kardex_id");
            entity.Property(e => e.CostoUnitario)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("costo_unitario");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .HasColumnName("descripcion");
            entity.Property(e => e.Entrada)
                .HasPrecision(10, 3)
                .HasDefaultValueSql("'0.000'")
                .HasColumnName("entrada");
            entity.Property(e => e.FechaMovimiento)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_movimiento");
            entity.Property(e => e.Origen)
                .HasColumnType("enum('COMPRA','VENTA','AJUSTE_MANUAL','DEVOLUCION','INICIAL')")
                .HasColumnName("origen");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.ReferenciaId).HasColumnName("referencia_id");
            entity.Property(e => e.ReferenciaTipo)
                .HasMaxLength(20)
                .HasColumnName("referencia_tipo");
            entity.Property(e => e.Salida)
                .HasPrecision(10, 3)
                .HasDefaultValueSql("'0.000'")
                .HasColumnName("salida");
            entity.Property(e => e.StockAnterior)
                .HasPrecision(10, 3)
                .HasColumnName("stock_anterior");
            entity.Property(e => e.StockResultante)
                .HasPrecision(10, 3)
                .HasColumnName("stock_resultante");
            entity.Property(e => e.Tipo)
                .HasColumnType("enum('ENTRADA','SALIDA','AJUSTE')")
                .HasColumnName("tipo");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Producto).WithMany(p => p.Kardices)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kardex_ibfk_1");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Kardices)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("kardex_ibfk_2");
        });

        modelBuilder.Entity<MovimientosCaja>(entity =>
        {
            entity.HasKey(e => e.MovimientoId).HasName("PRIMARY");

            entity.ToTable("movimientos_caja");

            entity.HasIndex(e => e.CajaId, "caja_id");

            entity.HasIndex(e => e.SesionId, "sesion_id");

            entity.HasIndex(e => e.UsuarioId, "usuario_id");

            entity.HasIndex(e => e.VentaId, "venta_id");

            entity.Property(e => e.MovimientoId).HasColumnName("movimiento_id");
            entity.Property(e => e.CajaId).HasColumnName("caja_id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(256)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaMovimiento)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_movimiento");
            entity.Property(e => e.Monto)
                .HasPrecision(10, 2)
                .HasColumnName("monto");
            entity.Property(e => e.MontoAnterior)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("monto_anterior");
            entity.Property(e => e.MontoNuevo)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("monto_nuevo");
            entity.Property(e => e.SesionId).HasColumnName("sesion_id");
            entity.Property(e => e.TipoMovimiento)
                .HasColumnType("enum('APERTURA','VENTA','RETIRO','INGRESO','CIERRE')")
                .HasColumnName("tipo_movimiento");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            entity.Property(e => e.VentaId).HasColumnName("venta_id");

            entity.HasOne(d => d.Caja).WithMany(p => p.MovimientosCajas)
                .HasForeignKey(d => d.CajaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movimientos_caja_ibfk_2");

            entity.HasOne(d => d.Sesion).WithMany(p => p.MovimientosCajas)
                .HasForeignKey(d => d.SesionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movimientos_caja_ibfk_1");

            entity.HasOne(d => d.Usuario).WithMany(p => p.MovimientosCajas)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("movimientos_caja_ibfk_4");

            entity.HasOne(d => d.Venta).WithMany(p => p.MovimientosCajas)
                .HasForeignKey(d => d.VentaId)
                .HasConstraintName("movimientos_caja_ibfk_3");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PRIMARY");

            entity.ToTable("productos");

            entity.HasIndex(e => e.Codigo, "codigo").IsUnique();

            entity.HasIndex(e => e.CodigoBarras, "codigo_barras").IsUnique();

            entity.HasIndex(e => e.CategoriaId, "idx_productos_categoria");

            entity.HasIndex(e => e.Estado, "idx_productos_estado");

            entity.HasIndex(e => e.ProveedorId, "idx_productos_proveedor");

            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.Codigo)
                .HasMaxLength(50)
                .HasColumnName("codigo");
            entity.Property(e => e.CodigoBarras)
                .HasMaxLength(50)
                .HasColumnName("codigo_barras");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(1000)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'1'")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.ImagenPrincipal)
                .HasMaxLength(256)
                .HasColumnName("imagen_principal");
            entity.Property(e => e.Nombre)
                .HasMaxLength(256)
                .HasColumnName("nombre");
            entity.Property(e => e.Peso)
                .HasPrecision(8, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("peso");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("precio");
            entity.Property(e => e.PrecioCosto)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("precio_costo");
            entity.Property(e => e.PrecioMayorista)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("precio_mayorista");
            entity.Property(e => e.ProveedorId).HasColumnName("proveedor_id");
            entity.Property(e => e.StockActual)
                .HasDefaultValueSql("'0'")
                .HasColumnName("stock_actual");
            entity.Property(e => e.StockMaximo)
                .HasDefaultValueSql("'100'")
                .HasColumnName("stock_maximo");
            entity.Property(e => e.StockMinimo)
                .HasDefaultValueSql("'5'")
                .HasColumnName("stock_minimo");
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(50)
                .HasColumnName("ubicacion");
            entity.Property(e => e.UnidadMedida)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Unidad'")
                .HasColumnName("unidad_medida");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("productos_ibfk_1");

            entity.HasOne(d => d.Proveedor).WithMany(p => p.Productos)
                .HasForeignKey(d => d.ProveedorId)
                .HasConstraintName("productos_ibfk_2");
        });

        modelBuilder.Entity<Promocione>(entity =>
        {
            entity.HasKey(e => e.PromoId).HasName("PRIMARY");

            entity.ToTable("promociones");

            entity.HasIndex(e => e.CategoriaId, "categoria_id");

            entity.HasIndex(e => e.ProductoId, "producto_id");

            entity.Property(e => e.PromoId).HasColumnName("promo_id");
            entity.Property(e => e.AplicaA)
                .HasDefaultValueSql("'PRODUCTO'")
                .HasColumnType("enum('PRODUCTO','CATEGORIA','TODA_VENTA')")
                .HasColumnName("aplica_a");
            entity.Property(e => e.CantidadMinima)
                .HasDefaultValueSql("'1'")
                .HasColumnName("cantidad_minima");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(256)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'1'")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.FechaFin).HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.HoraFin)
                .HasColumnType("time")
                .HasColumnName("hora_fin");
            entity.Property(e => e.HoraInicio)
                .HasColumnType("time")
                .HasColumnName("hora_inicio");
            entity.Property(e => e.MontoMinimo)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("monto_minimo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.ProductoId).HasColumnName("producto_id");
            entity.Property(e => e.Tipo)
                .HasColumnType("enum('PORCENTAJE','MONTO_FIJO','2X1','COMBO')")
                .HasColumnName("tipo");
            entity.Property(e => e.Valor)
                .HasPrecision(10, 2)
                .HasColumnName("valor");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Promociones)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("promociones_ibfk_2");

            entity.HasOne(d => d.Producto).WithMany(p => p.Promociones)
                .HasForeignKey(d => d.ProductoId)
                .HasConstraintName("promociones_ibfk_1");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.ProveedorId).HasName("PRIMARY");

            entity.ToTable("proveedores");

            entity.HasIndex(e => e.Codigo, "codigo").IsUnique();

            entity.Property(e => e.ProveedorId).HasColumnName("proveedor_id");
            entity.Property(e => e.Codigo)
                .HasMaxLength(20)
                .HasColumnName("codigo");
            entity.Property(e => e.ContactoNombre)
                .HasMaxLength(100)
                .HasColumnName("contacto_nombre");
            entity.Property(e => e.ContactoTelefono)
                .HasMaxLength(20)
                .HasColumnName("contacto_telefono");
            entity.Property(e => e.DiasCredito)
                .HasDefaultValueSql("'0'")
                .HasColumnName("dias_credito");
            entity.Property(e => e.Direccion)
                .HasMaxLength(256)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'1'")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.LimiteCredito)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("limite_credito");
            entity.Property(e => e.NombreComercial)
                .HasMaxLength(256)
                .HasColumnName("nombre_comercial");
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(256)
                .HasColumnName("razon_social");
            entity.Property(e => e.RucDni)
                .HasMaxLength(20)
                .HasColumnName("ruc_dni");
            entity.Property(e => e.SaldoCredito)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("saldo_credito");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
            entity.Property(e => e.TipoDocumento)
                .HasDefaultValueSql("'RUC'")
                .HasColumnType("enum('RUC','DNI')")
                .HasColumnName("tipo_documento");
        });

        modelBuilder.Entity<SesionesCaja>(entity =>
        {
            entity.HasKey(e => e.SesionId).HasName("PRIMARY");

            entity.ToTable("sesiones_caja");

            entity.HasIndex(e => e.CajaId, "caja_id");

            entity.HasIndex(e => e.UsuarioId, "usuario_id");

            entity.Property(e => e.SesionId).HasColumnName("sesion_id");
            entity.Property(e => e.CajaId).HasColumnName("caja_id");
            entity.Property(e => e.Diferencia)
                .HasPrecision(10, 2)
                .HasColumnName("diferencia");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'ABIERTA'")
                .HasColumnType("enum('ABIERTA','CERRADA')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaApertura)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_apertura");
            entity.Property(e => e.FechaCierre)
                .HasColumnType("timestamp")
                .HasColumnName("fecha_cierre");
            entity.Property(e => e.MontoApertura)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("monto_apertura");
            entity.Property(e => e.MontoCierre)
                .HasPrecision(10, 2)
                .HasColumnName("monto_cierre");
            entity.Property(e => e.MontoSistema)
                .HasPrecision(10, 2)
                .HasColumnName("monto_sistema");
            entity.Property(e => e.Observaciones)
                .HasColumnType("text")
                .HasColumnName("observaciones");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Caja).WithMany(p => p.SesionesCajas)
                .HasForeignKey(d => d.CajaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sesiones_caja_ibfk_1");

            entity.HasOne(d => d.Usuario).WithMany(p => p.SesionesCajas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sesiones_caja_ibfk_2");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PRIMARY");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.CreadoPor, "creado_por");

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .HasColumnName("apellidos");
            entity.Property(e => e.CreadoPor).HasColumnName("creado_por");
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .HasColumnName("dni");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'1'")
                .HasColumnName("estado");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .HasColumnName("nombres");
            entity.Property(e => e.Password)
                .HasMaxLength(512)
                .HasColumnName("password");
            entity.Property(e => e.Rol)
                .HasDefaultValueSql("'CAJERO'")
                .HasColumnType("enum('ADMIN','CAJERO','VENDEDOR','ALMACENERO')")
                .HasColumnName("rol");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");
            entity.Property(e => e.UltimoAcceso)
                .HasColumnType("timestamp")
                .HasColumnName("ultimo_acceso");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.CreadoPorNavigation).WithMany(p => p.InverseCreadoPorNavigation)
                .HasForeignKey(d => d.CreadoPor)
                .HasConstraintName("usuarios_ibfk_1");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.VentaId).HasName("PRIMARY");

            entity.ToTable("ventas");

            entity.HasIndex(e => e.CajaId, "idx_ventas_caja");

            entity.HasIndex(e => e.ClienteId, "idx_ventas_cliente");

            entity.HasIndex(e => e.Estado, "idx_ventas_estado");

            entity.HasIndex(e => e.FechaVenta, "idx_ventas_fecha");

            entity.HasIndex(e => e.UsuarioId, "idx_ventas_usuario");

            entity.HasIndex(e => e.NumeroTicket, "numero_ticket").IsUnique();

            entity.HasIndex(e => e.SesionId, "sesion_id");

            entity.HasIndex(e => e.UsuarioAnulacion, "usuario_anulacion");

            entity.Property(e => e.VentaId).HasColumnName("venta_id");
            entity.Property(e => e.CajaId).HasColumnName("caja_id");
            entity.Property(e => e.ClienteDocumento)
                .HasMaxLength(20)
                .HasColumnName("cliente_documento");
            entity.Property(e => e.ClienteId).HasColumnName("cliente_id");
            entity.Property(e => e.ClienteNombre)
                .HasMaxLength(200)
                .HasColumnName("cliente_nombre");
            entity.Property(e => e.Descuento)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("descuento");
            entity.Property(e => e.Estado)
                .HasDefaultValueSql("'COMPLETADA'")
                .HasColumnType("enum('COMPLETADA','ANULADA','PENDIENTE')")
                .HasColumnName("estado");
            entity.Property(e => e.FechaAnulacion)
                .HasColumnType("timestamp")
                .HasColumnName("fecha_anulacion");
            entity.Property(e => e.FechaVenta)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("fecha_venta");
            entity.Property(e => e.Igv)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("igv");
            entity.Property(e => e.MetodoPago)
                .HasDefaultValueSql("'EFECTIVO'")
                .HasColumnType("enum('EFECTIVO','TARJETA','YAPE','PLIN','TRANSFERENCIA','MIXTO')")
                .HasColumnName("metodo_pago");
            entity.Property(e => e.MontoPagado)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("monto_pagado");
            entity.Property(e => e.MotivoAnulacion)
                .HasMaxLength(256)
                .HasColumnName("motivo_anulacion");
            entity.Property(e => e.NumeroCorrelativo).HasColumnName("numero_correlativo");
            entity.Property(e => e.NumeroTicket)
                .HasMaxLength(20)
                .HasColumnName("numero_ticket");
            entity.Property(e => e.Observaciones)
                .HasColumnType("text")
                .HasColumnName("observaciones");
            entity.Property(e => e.Serie)
                .HasMaxLength(4)
                .HasDefaultValueSql("'T001'")
                .HasColumnName("serie");
            entity.Property(e => e.SesionId).HasColumnName("sesion_id");
            entity.Property(e => e.Subtotal)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("subtotal");
            entity.Property(e => e.TipoComprobante)
                .HasDefaultValueSql("'TICKET'")
                .HasColumnType("enum('TICKET','BOLETA','FACTURA')")
                .HasColumnName("tipo_comprobante");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasColumnName("total");
            entity.Property(e => e.UsuarioAnulacion).HasColumnName("usuario_anulacion");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            entity.Property(e => e.Vuelto)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'")
                .HasColumnName("vuelto");

            entity.HasOne(d => d.Caja).WithMany(p => p.Venta)
                .HasForeignKey(d => d.CajaId)
                .HasConstraintName("ventas_ibfk_3");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Venta)
                .HasForeignKey(d => d.ClienteId)
                .HasConstraintName("ventas_ibfk_1");

            entity.HasOne(d => d.Sesion).WithMany(p => p.Venta)
                .HasForeignKey(d => d.SesionId)
                .HasConstraintName("ventas_ibfk_4");

            entity.HasOne(d => d.UsuarioAnulacionNavigation).WithMany(p => p.VentaUsuarioAnulacionNavigations)
                .HasForeignKey(d => d.UsuarioAnulacion)
                .HasConstraintName("ventas_ibfk_5");

            entity.HasOne(d => d.Usuario).WithMany(p => p.VentaUsuarios)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("ventas_ibfk_2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
