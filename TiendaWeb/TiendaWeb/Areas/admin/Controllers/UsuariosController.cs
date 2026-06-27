using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TiendaWeb.Areas.admin.service;
using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "ADMIN")]
    public class UsuariosController : Controller
    {
        private readonly IUsuarioAdminService _svc;
        public UsuariosController(IUsuarioAdminService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Usuarios";
            var vm = new UsuariosIndexViewModel
            {
                Lista = await _svc.GetAllAsync()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioCreateViewModel Crear)
        {
            if (!ModelState.IsValid)
            {
                var vm = new UsuariosIndexViewModel
                {
                    Lista = await _svc.GetAllAsync(),
                    Crear = Crear
                };
                TempData["AbrirModalCrear"] = "true";
                return View("Index", vm);
            }

            var creadoPor = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            var (ok, msg) = await _svc.CreateAsync(Crear, creadoPor);

            if (!ok)
            {
                ModelState.AddModelError(string.Empty, msg);
                var vm = new UsuariosIndexViewModel
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
        public async Task<IActionResult> Edit(UsuarioEditViewModel Editar)
        {
            if (string.IsNullOrWhiteSpace(Editar.NewPassword))
            {
                ModelState.Remove("NewPassword");
                ModelState.Remove("ConfirmNewPassword");
            }

            if (!ModelState.IsValid)
            {
                var vm = new UsuariosIndexViewModel
                {
                    Lista = await _svc.GetAllAsync(),
                    Editar = Editar
                };
                TempData["AbrirModalEditar"] = Editar.UsuarioId.ToString();
                return View("Index", vm);
            }

            var (ok, msg) = await _svc.UpdateAsync(Editar);

            if (!ok)
            {
                ModelState.AddModelError(string.Empty, msg);
                var vm = new UsuariosIndexViewModel
                {
                    Lista = await _svc.GetAllAsync(),
                    Editar = Editar
                };
                TempData["AbrirModalEditar"] = Editar.UsuarioId.ToString();
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
