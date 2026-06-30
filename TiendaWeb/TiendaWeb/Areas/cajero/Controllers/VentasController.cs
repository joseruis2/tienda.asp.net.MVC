using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TiendaWeb.Areas.cajero.Service;
using TiendaWeb.Areas.cajero.ViewModel;

namespace TiendaWeb.Areas.cajero.Controllers
{
    [Area("cajero")]
    [Authorize(Roles = "ADMIN,CAJERO,VENDEDOR")]
    public class VentasController : Controller
    {
        private readonly IVentaCajeroService _svc;
        public VentasController(IVentaCajeroService svc) => _svc = svc;

        private int GetUsuarioId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Nueva Venta";
            var vm = await _svc.GetPosDataAsync(GetUsuarioId());

            if (!vm.TieneSesionAbierta)
            {
                TempData["Error"] = "Debes abrir una caja antes de vender.";
                return RedirectToAction("Index", "Dashboard");
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Procesar([FromBody] VentaCajeroCreateViewModel vm)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Datos inválidos." });

            var (ok, msg, ventaId) = await _svc.CreateAsync(vm, GetUsuarioId());
            return Json(new { success = ok, message = msg, ventaId });
        }
    }
}
