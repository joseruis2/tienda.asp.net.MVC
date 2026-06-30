using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TiendaWeb.Areas.admin.service;
using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class VentasController : Controller
    {
        private readonly IVentaService _svc;
        public VentasController(IVentaService svc) => _svc = svc;

        private int GetUsuarioId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        // ── POS ──
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Nueva Venta";
            var vm = await _svc.GetPosDataAsync();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Procesar([FromBody] VentaCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Datos inválidos." });

            var (ok, msg, ventaId) = await _svc.CreateAsync(vm, GetUsuarioId());
            return Json(new { success = ok, message = msg, ventaId });
        }

        // ── HISTORIAL ──
        [HttpGet]
        public async Task<IActionResult> Historial(
            string? fecha, string? estado, string? metodo)
        {
            ViewData["Title"] = "Historial de Ventas";
            DateTime? fechaFiltro = null;
            if (!string.IsNullOrEmpty(fecha) &&
                DateTime.TryParse(fecha, out var f))
                fechaFiltro = f;

            var vm = await _svc.GetHistorialAsync(fechaFiltro, estado, metodo);
            return View(vm);
        }

        // ── DETALLE (JSON para modal) ──
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var vm = await _svc.GetDetalleAsync(id);
            if (vm == null) return NotFound();
            return Json(vm);
        }

        // ── ANULAR ──
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Anular(AnularVentaViewModel vm)
        {
            var (ok, msg) = await _svc.AnularAsync(vm, GetUsuarioId());
            TempData[ok ? "Success" : "Error"] = msg;
            return RedirectToAction("Historial");
        }
    }
}
