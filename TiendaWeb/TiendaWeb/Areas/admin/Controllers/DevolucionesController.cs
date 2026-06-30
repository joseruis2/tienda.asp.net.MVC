using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TiendaWeb.Areas.admin.service;
using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class DevolucionesController : Controller
    {
        private readonly IDevolucionService _svc;
        public DevolucionesController(IDevolucionService svc) => _svc = svc;

        private int GetUsuarioId() =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpGet]
        public async Task<IActionResult> Index(string? fecha)
        {
            ViewData["Title"] = "Devoluciones";
            DateTime? fechaFiltro = null;
            if (!string.IsNullOrEmpty(fecha) &&
                DateTime.TryParse(fecha, out var f))
                fechaFiltro = f;

            var vm = await _svc.GetIndexDataAsync(fechaFiltro);
            return View(vm);
        }

        // ── Buscar venta para devolver ──
        [HttpGet]
        public async Task<IActionResult> Nueva(string? ticket)
        {
            ViewData["Title"] = "Nueva Devolución";

            if (string.IsNullOrEmpty(ticket))
                return View(new DevolucionCreateViewModel());

            var venta = await _svc.BuscarVentaAsync(ticket);
            if (venta == null)
            {
                TempData["Error"] = $"No se encontró la venta {ticket} o ya está anulada.";
                return View(new DevolucionCreateViewModel());
            }

            var vm = new DevolucionCreateViewModel
            {
                VentaId = venta.VentaId,
                NumeroTicket = venta.NumeroTicket,
                ClienteNombre = venta.ClienteNombre,
                TotalVenta = venta.Total,
                Items = venta.Items.Select(i => new DevolucionItemViewModel
                {
                    ProductoId = i.ProductoId,
                    NombreProducto = i.NombreProducto,
                    CantidadVendida = i.Cantidad,
                    PrecioUnitario = i.PrecioUnitario,
                    CantidadDevolver = 0,
                    RegresaStock = true
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Nueva(DevolucionCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var (ok, msg) = await _svc.CreateAsync(vm, GetUsuarioId());
            TempData[ok ? "Success" : "Error"] = msg;

            return ok
                ? RedirectToAction("Index")
                : View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var vm = await _svc.GetDetalleAsync(id);
            if (vm == null) return NotFound();
            return Json(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Anular(int id)
        {
            var (ok, msg) = await _svc.AnularAsync(id, GetUsuarioId());
            TempData[ok ? "Success" : "Error"] = msg;
            return RedirectToAction("Index");
        }
    }
}
