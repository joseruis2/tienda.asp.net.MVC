using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiendaWeb.Areas.cajero.Service;
using TiendaWeb.Areas.cajero.ViewModel;

namespace TiendaWeb.Areas.cajero.Controllers
{
    [Area("cajero")]
    [Authorize(Roles = "ADMIN,CAJERO,VENDEDOR")]
    public class ClientesController : Controller
    {
        private readonly IClienteCajeroService _svc;
        public ClientesController(IClienteCajeroService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> Index(string? busqueda)
        {
            ViewData["Title"] = "Clientes";
            var vm = await _svc.GetIndexDataAsync(busqueda);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteRapidoCreateViewModel Crear)
        {
            if (!ModelState.IsValid)
            {
                var vm = await _svc.GetIndexDataAsync(null);
                vm.Crear = Crear;
                TempData["AbrirModalCrear"] = "true";
                return View("Index", vm);
            }

            var (ok, msg, clienteId) = await _svc.CreateAsync(Crear);
            if (!ok)
            {
                ModelState.AddModelError(string.Empty, msg);
                var vm = await _svc.GetIndexDataAsync(null);
                vm.Crear = Crear;
                TempData["AbrirModalCrear"] = "true";
                return View("Index", vm);
            }

            TempData["Success"] = msg;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetEditar(int id)
        {
            var vm = await _svc.GetForEditAsync(id);
            if (vm == null) return NotFound();
            return Json(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ClienteRapidoEditViewModel Editar)
        {
            if (!ModelState.IsValid)
            {
                var vm = await _svc.GetIndexDataAsync(null);
                vm.Editar = Editar;
                TempData["AbrirModalEditar"] = Editar.ClienteId.ToString();
                return View("Index", vm);
            }

            var (ok, msg) = await _svc.UpdateAsync(Editar);
            if (!ok)
            {
                ModelState.AddModelError(string.Empty, msg);
                var vm = await _svc.GetIndexDataAsync(null);
                vm.Editar = Editar;
                TempData["AbrirModalEditar"] = Editar.ClienteId.ToString();
                return View("Index", vm);
            }

            TempData["Success"] = msg;
            return RedirectToAction("Index");
        }
    }
}
