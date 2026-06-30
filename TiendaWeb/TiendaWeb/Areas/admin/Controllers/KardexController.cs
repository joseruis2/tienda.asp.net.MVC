using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TiendaWeb.Areas.admin.service;
using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class KardexController : Controller
    {
        private readonly IKardexService _svc;
        public KardexController(IKardexService svc) => _svc = svc;

        private int GetUsuarioId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpGet]
        public async Task<IActionResult> Index(
            int? productoId, string? tipo, string? origen, string? fecha)
        {
            ViewData["Title"] = "Kardex";
            DateTime? fechaFiltro = null;
            if (!string.IsNullOrEmpty(fecha) &&
                DateTime.TryParse(fecha, out var f))
                fechaFiltro = f;

            var vm = await _svc.GetIndexDataAsync(productoId, tipo, origen, fechaFiltro);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ajuste(AjusteManualViewModel Ajuste)
        {
            if (!ModelState.IsValid)
            {
                var vm = await _svc.GetIndexDataAsync(null, null, null, null);
                vm.Ajuste = Ajuste;
                TempData["AbrirModalAjuste"] = "true";
                return View("Index", vm);
            }

            var (ok, msg) = await _svc.AjusteManualAsync(Ajuste, GetUsuarioId());
            TempData[ok ? "Success" : "Error"] = msg;
            return RedirectToAction("Index");
        }
    }
}
