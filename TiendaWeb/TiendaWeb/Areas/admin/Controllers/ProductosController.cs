using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiendaWeb.Areas.admin.service;
using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly IProductoService _svc;
        public ProductosController(IProductoService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Productos";
            var vm = await _svc.GetIndexDataAsync();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoCreateViewModel Crear)
        {
            if (!ModelState.IsValid)
            {
                var vm = await _svc.GetIndexDataAsync();
                vm.Crear = Crear;
                TempData["AbrirModalCrear"] = "true";
                return View("Index", vm);
            }

            var (ok, msg) = await _svc.CreateAsync(Crear);
            if (!ok)
            {
                ModelState.AddModelError(string.Empty, msg);
                var vm = await _svc.GetIndexDataAsync();
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
        public async Task<IActionResult> Edit(ProductoEditViewModel Editar)
        {
            if (!ModelState.IsValid)
            {
                var vm = await _svc.GetIndexDataAsync();
                vm.Editar = Editar;
                TempData["AbrirModalEditar"] = Editar.ProductoId.ToString();
                return View("Index", vm);
            }

            var (ok, msg) = await _svc.UpdateAsync(Editar);
            if (!ok)
            {
                ModelState.AddModelError(string.Empty, msg);
                var vm = await _svc.GetIndexDataAsync();
                vm.Editar = Editar;
                TempData["AbrirModalEditar"] = Editar.ProductoId.ToString();
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
