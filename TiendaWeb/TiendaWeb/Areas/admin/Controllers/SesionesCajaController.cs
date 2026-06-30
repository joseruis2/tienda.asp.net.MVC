using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TiendaWeb.Areas.admin.service;
using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "ADMIN,CAJERO")]
    public class SesionesCajaController : Controller
    {
        private readonly ISesionCajaService _svc;
        public SesionesCajaController(ISesionCajaService svc) => _svc = svc;

        private int GetUsuarioId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Sesiones de Caja";
            var vm = await _svc.GetIndexDataAsync(GetUsuarioId());
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Abrir(AbrirCajaViewModel Abrir)
        {
            if (!ModelState.IsValid)
            {
                var vm = await _svc.GetIndexDataAsync(GetUsuarioId());
                vm.Abrir = Abrir;
                TempData["AbrirModal"] = "true";
                return View("Index", vm);
            }

            var (ok, msg) = await _svc.AbrirCajaAsync(Abrir, GetUsuarioId());
            if (!ok)
            {
                ModelState.AddModelError(string.Empty, msg);
                var vm = await _svc.GetIndexDataAsync(GetUsuarioId());
                vm.Abrir = Abrir;
                TempData["AbrirModal"] = "true";
                return View("Index", vm);
            }

            TempData["Success"] = msg;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Cierre(int id)
        {
            ViewData["Title"] = "Cierre de Caja";
            var vm = await _svc.GetCierreDataAsync(id);
            if (vm == null)
            {
                TempData["Error"] = "Sesión no encontrada o ya cerrada.";
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cierre(CerrarCajaViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var (ok, msg) = await _svc.CerrarCajaAsync(vm);
            TempData[ok ? "Success" : "Error"] = msg;
            return RedirectToAction("Index");
        }
    }
}
