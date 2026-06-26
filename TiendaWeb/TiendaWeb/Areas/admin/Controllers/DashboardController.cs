using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiendaWeb.Areas.admin.service;

namespace TiendaWeb.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _svc;

        public DashboardController(IDashboardService svc) => _svc = svc;

        public async Task<IActionResult> Index()
        {

            ViewData["Title"] = "Dashboard";
            var vm = await _svc.GetDashboardDataAsync();
            return View(vm);
        }
    }
}
