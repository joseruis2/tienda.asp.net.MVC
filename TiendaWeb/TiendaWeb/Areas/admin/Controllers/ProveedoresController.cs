using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiendaWeb.Areas.admin.service;
using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class ProveedoresController : Controller
    {
        private readonly IProveedorService _svc;
        public ProveedoresController(IProveedorService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Proveedores";
            var vm = new ProveedoresIndexViewModel
            {
                Lista = await _svc.GetAllAsync()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProveedorCreateViewModel Crear)
        {
            if (!ModelState.IsValid)
            {
                var vm = new ProveedoresIndexViewModel
                {
                    Lista = await _svc.GetAllAsync(),
                    Crear = Crear
                };
                TempData["AbrirModalCrear"] = "true";
                return View("Index", vm);
            }

            var (ok, msg) = await _svc.CreateAsync(Crear);
            if (!ok)
            {
                ModelState.AddModelError(string.Empty, msg);
                var vm = new ProveedoresIndexViewModel
                {
                    Lista = await _svc.GetAllAsync(),
                    Crear = Crear
                };
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
        public async Task<IActionResult> Edit(ProveedorEditViewModel Editar)
        {
            if (!ModelState.IsValid)
            {
                var vm = new ProveedoresIndexViewModel
                {
                    Lista = await _svc.GetAllAsync(),
                    Editar = Editar
                };
                TempData["AbrirModalEditar"] = Editar.ProveedorId.ToString();
                return View("Index", vm);
            }

            var (ok, msg) = await _svc.UpdateAsync(Editar);
            if (!ok)
            {
                ModelState.AddModelError(string.Empty, msg);
                var vm = new ProveedoresIndexViewModel
                {
                    Lista = await _svc.GetAllAsync(),
                    Editar = Editar
                };
                TempData["AbrirModalEditar"] = Editar.ProveedorId.ToString();
                return View("Index", vm);
            }

            TempData["Success"] = msg;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var (ok, msg) = await _svc.DeleteAsync(id);
            TempData[ok ? "Success" : "Error"] = msg;
            return RedirectToAction("Index");
        }
    }
}
