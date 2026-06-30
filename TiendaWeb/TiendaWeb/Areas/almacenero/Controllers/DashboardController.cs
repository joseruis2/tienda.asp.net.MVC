using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiendaWeb.Areas.almacenero.service;

namespace TiendaWeb.Areas.almacenero.Controllers
{
    [Area("almacenero")]
    [Authorize(Roles = "ADMIN,ALMACENERO")]
    public class DashboardController : Controller
    {
        private readonly IDashboardAlmaceneroService _svc;
        public DashboardController(IDashboardAlmaceneroService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Dashboard Almacén";
            var vm = await _svc.GetDashboardDataAsync();
            return View(vm);
        }
    }
}
