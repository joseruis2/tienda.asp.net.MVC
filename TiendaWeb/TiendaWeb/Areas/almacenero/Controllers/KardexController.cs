using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiendaWeb.Areas.almacenero.service;

namespace TiendaWeb.Areas.almacenero.Controllers
{
    [Area("almacenero")]
    [Authorize(Roles = "ADMIN,ALMACENERO")]
    public class KardexController : Controller
    {
        private readonly IKardexAlmaceneroService _svc;
        public KardexController(IKardexAlmaceneroService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> Index(
            int? productoId, string? tipo,
            string? origen, string? fecha)
        {
            ViewData["Title"] = "Movimientos de Inventario";

            DateTime? fechaFiltro = null;
            if (!string.IsNullOrEmpty(fecha) &&
                DateTime.TryParse(fecha, out var f))
                fechaFiltro = f;

            var vm = await _svc.GetIndexDataAsync(productoId, tipo, origen, fechaFiltro);
            return View(vm);
        }
    }
}
