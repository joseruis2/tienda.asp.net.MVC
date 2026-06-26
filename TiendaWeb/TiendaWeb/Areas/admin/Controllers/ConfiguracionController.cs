using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiendaWeb.Areas.admin.service;
using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class ConfiguracionController : Controller
    {
        private readonly IConfiguracionService _svc;
        public ConfiguracionController(IConfiguracionService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Configuración del negocio";
            var vm = await _svc.GetAsync();
            if (vm == null) return NotFound("No hay configuración registrada.");
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConfiguracionViewModel vm)
        {
            ViewData["Title"] = "Configuración del negocio";
            if (!ModelState.IsValid) return View(vm);

            var (ok, msg) = await _svc.UpdateAsync(vm);

            TempData[ok ? "Success" : "Error"] = msg;
            return RedirectToAction("Index");
        }
    }
}
