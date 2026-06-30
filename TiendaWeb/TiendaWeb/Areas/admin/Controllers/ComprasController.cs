using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TiendaWeb.Areas.admin.service;
using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class ComprasController : Controller
    {
        private readonly ICompraService _svc;
        public ComprasController(ICompraService svc) => _svc = svc;

        private int GetUsuarioId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpGet]
        public async Task<IActionResult> Index(string? estado)
        {
            ViewData["Title"] = "Compras";
            var vm = await _svc.GetIndexDataAsync(estado);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] CompraCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Datos inválidos." });

            var (ok, msg) = await _svc.CreateAsync(vm, GetUsuarioId());
            return Json(new { success = ok, message = msg });
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var vm = await _svc.GetDetalleAsync(id);
            if (vm == null) return NotFound();
            return Json(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Recepcion(int id)
        {
            ViewData["Title"] = "Recepción de Mercadería";
            var vm = await _svc.GetRecepcionDataAsync(id);
            if (vm == null)
            {
                TempData["Error"] = "La orden no está disponible para recepción.";
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recepcion(RecepcionCompraViewModel vm)
        {
            var (ok, msg) = await _svc.RecibirAsync(vm, GetUsuarioId());
            TempData[ok ? "Success" : "Error"] = msg;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Anular(AnularCompraViewModel vm)
        {
            var (ok, msg) = await _svc.AnularAsync(vm, GetUsuarioId());
            TempData[ok ? "Success" : "Error"] = msg;
            return RedirectToAction("Index");
        }
    }
}
