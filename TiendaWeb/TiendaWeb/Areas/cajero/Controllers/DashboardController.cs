using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TiendaWeb.Areas.cajero.Service;
using TiendaWeb.Areas.cajero.ViewModel;

namespace TiendaWeb.Areas.cajero.Controllers
{
    [Area("cajero")]
    [Authorize(Roles = "ADMIN,CAJERO,VENDEDOR")]
    public class DashboardController : Controller
    {
        private readonly IDashboardCajeroService _svc;
        public DashboardController(IDashboardCajeroService svc) => _svc = svc;

        private int GetUsuarioId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Dashboard";
            var vm = await _svc.GetDashboardDataAsync(GetUsuarioId());
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AbrirCaja(AbrirCajaCajeroViewModel vm)
        {
            var (ok, msg) = await _svc.AbrirCajaAsync(vm, GetUsuarioId());
            TempData[ok ? "Success" : "Error"] = msg;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Cierre()
        {
            ViewData["Title"] = "Cierre de Caja";
            var vm = await _svc.GetCierreDataAsync(GetUsuarioId());
            if (vm == null)
            {
                TempData["Error"] = "No tienes una sesión de caja abierta.";
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cierre(CerrarCajaCajeroViewModel vm)
        {
            var (ok, msg) = await _svc.CerrarCajaAsync(vm);
            TempData[ok ? "Success" : "Error"] = msg;
            return RedirectToAction("Index");
        }
    }
}
