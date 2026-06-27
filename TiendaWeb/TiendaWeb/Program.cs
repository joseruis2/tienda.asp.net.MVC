using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TiendaWeb.Areas.admin.repository;
using TiendaWeb.Areas.admin.ViewModel;
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

// SERVICES
builder.Services.AddSession(o => {
    o.IdleTimeout = TimeSpan.FromHours(8);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});


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