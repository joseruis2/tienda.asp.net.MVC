using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TiendaWeb.Areas.admin.repository;
using TiendaWeb.Areas.almacenero.repository;
using TiendaWeb.Areas.cajero.Repository;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.repositories;
using TiendaWeb.Infrastructure.service;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Login/Logout";
        options.AccessDeniedPath = "/Login/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.None
            : CookieSecurePolicy.Always;
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Dashboard Admin
builder.Services.AddScoped<TiendaWeb.Areas.admin.service.IDashboardService,
                            TiendaWeb.Areas.admin.service.DashboardService>();

// configuracion
builder.Services.AddScoped<IConfiguracionRepository, ConfiguracionRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.admin.service.IConfiguracionService,
                            TiendaWeb.Areas.admin.service.ConfiguracionService>();

//usuarios
builder.Services.AddScoped<IUsuarioAdminRepository, UsuarioAdminRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.admin.service.IUsuarioAdminService,
                            TiendaWeb.Areas.admin.service.UsuarioAdminService>();

//categoria
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.admin.service.ICategoriaService,
                            TiendaWeb.Areas.admin.service.CategoriaService>();

//provedor
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.admin.service.IProveedorService,
                            TiendaWeb.Areas.admin.service.ProveedorService>();

//producto
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.admin.service.IProductoService,
                            TiendaWeb.Areas.admin.service.ProductoService>();
//Cliente
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.admin.service.IClienteService,
                            TiendaWeb.Areas.admin.service.ClienteService>();

//CAJA
builder.Services.AddScoped<ICajaRepository, CajaRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.admin.service.ICajaService,
                            TiendaWeb.Areas.admin.service.CajaService>();

//venta
builder.Services.AddScoped<IVentaRepository, VentaRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.admin.service.IVentaService,
                            TiendaWeb.Areas.admin.service.VentaService>();
//KARDEX
builder.Services.AddScoped<IKardexRepository, KardexRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.admin.service.IKardexService,
                            TiendaWeb.Areas.admin.service.KardexService>();
//devolucion
builder.Services.AddScoped<IDevolucionRepository, DevolucionRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.admin.service.IDevolucionService,
                            TiendaWeb.Areas.admin.service.DevolucionService>();

//COMPRA 
builder.Services.AddScoped<ICompraRepository, CompraRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.admin.service.ICompraService,
                            TiendaWeb.Areas.admin.service.CompraService>();

//promecion
builder.Services.AddScoped<IPromocionRepository, PromocionRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.admin.service.IPromocionService,
                            TiendaWeb.Areas.admin.service.PromocionService>();

// areas almacen producto:
builder.Services.AddScoped<IProductoAlmaceneroRepository,
                            ProductoAlmaceneroRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.almacenero.service.IProductoAlmaceneroService,
                            TiendaWeb.Areas.almacenero.service.ProductoAlmaceneroService>();

builder.Services.AddScoped<IKardexAlmaceneroRepository, KardexAlmaceneroRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.almacenero.service.IKardexAlmaceneroService,
                            TiendaWeb.Areas.almacenero.service.KardexAlmaceneroService>();

//Almacen
builder.Services.AddScoped<IDashboardAlmaceneroRepository,
                            DashboardAlmaceneroRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.almacenero.service.IDashboardAlmaceneroService,
                            TiendaWeb.Areas.almacenero.service.DashboardAlmaceneroService>();

builder.Services.AddScoped<ICompraAlmaceneroRepository, CompraAlmaceneroRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.almacenero.service.ICompraAlmaceneroService,
                            TiendaWeb.Areas.almacenero.service.CompraAlmaceneroService>();

// Agregar en Program.cs
builder.Services.AddScoped<IClienteCajeroRepository, ClienteCajeroRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.cajero.Service.IClienteCajeroService,
                            TiendaWeb.Areas.cajero.Service.ClienteCajeroService>();

builder.Services.AddScoped<IVentaCajeroRepository, VentaCajeroRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.cajero.Service.IVentaCajeroService,
                            TiendaWeb.Areas.cajero.Service.VentaCajeroService>();

// SERVICES
builder.Services.AddSession(o => {
    o.IdleTimeout = TimeSpan.FromHours(8);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

//sesion
builder.Services.AddScoped<ISesionCajaRepository, SesionCajaRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.admin.service.ISesionCajaService,
                            TiendaWeb.Areas.admin.service.SesionCajaService>();

builder.Services.AddScoped<IDashboardCajeroRepository, DashboardCajeroRepository>();
builder.Services.AddScoped<TiendaWeb.Areas.cajero.Service.IDashboardCajeroService,
                            TiendaWeb.Areas.cajero.Service.DashboardCajeroService>();


builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseSession();          // ← debe ir después de Auth
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}"); // ← Login es la raíz

app.Run();