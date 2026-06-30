using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiendaWeb.Areas.almacenero.service;

namespace TiendaWeb.Areas.almacenero.Controllers
{
    [Area("almacenero")]
    [Authorize(Roles = "ADMIN,ALMACENERO")]
    public class ProductosController : Controller
    {
        private readonly IProductoAlmaceneroService _svc;
        public ProductosController(IProductoAlmaceneroService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> Index(
            string? busqueda, string? categoria, string? filtroStock)
        {
            ViewData["Title"] = "Inventario de Productos";
            var vm = await _svc.GetIndexDataAsync(busqueda, categoria, filtroStock);
            return View(vm);
        }
    }
}
