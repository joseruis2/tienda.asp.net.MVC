using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TiendaWeb.Areas.almacenero.service;
using TiendaWeb.Areas.almacenero.ViewModel;

namespace TiendaWeb.Areas.almacenero.Controllers
{
    [Area("almacenero")]
    [Authorize(Roles = "ADMIN,ALMACENERO")]
    public class ComprasController : Controller
    {
        private readonly ICompraAlmaceneroService _svc;
        public ComprasController(ICompraAlmaceneroService svc) => _svc = svc;

        private int GetUsuarioId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpGet]
        public async Task<IActionResult> Index(string? estado)
        {
            ViewData["Title"] = "Órdenes de Compra";
            var vm = await _svc.GetIndexDataAsync(estado);
            return View(vm);
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
        public async Task<IActionResult> Recepcion(RecepcionAlmaceneroViewModel vm)
        {
            var (ok, msg) = await _svc.RecibirAsync(vm, GetUsuarioId());
            TempData[ok ? "Success" : "Error"] = msg;
            return RedirectToAction("Index");
        }
    }
}
