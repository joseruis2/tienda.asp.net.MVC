using TiendaWeb.Areas.admin.repository;
using TiendaWeb.Areas.admin.ViewModel;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.service
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repo;
        public CategoriaService(ICategoriaRepository repo) => _repo = repo;

        public async Task<List<CategoriaListViewModel>> GetAllAsync()
        {
            var lista = await _repo.GetAllAsync();
            return lista.Select(c => new CategoriaListViewModel
            {
                CategoriaId = c.CategoriaId,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                Imagen = c.Imagen,
                Estado = c.Estado,
                FechaCreacion = c.FechaCreacion
            }).ToList();
        }

        public async Task<CategoriaEditViewModel?> GetForEditAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;
            return new CategoriaEditViewModel
            {
                CategoriaId = c.CategoriaId,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                Imagen = c.Imagen,
                Estado = c.Estado ?? true
            };
        }

        public async Task<(bool Success, string Message)> CreateAsync(CategoriaCreateViewModel vm)
        {
            if (await _repo.ExistsByNombreAsync(vm.Nombre))
                return (false, "Ya existe una categoría con ese nombre.");

            var categoria = new Categoria
            {
                Nombre = vm.Nombre,
                Descripcion = string.IsNullOrWhiteSpace(vm.Descripcion) ? null : vm.Descripcion,
                Imagen = string.IsNullOrWhiteSpace(vm.Imagen) ? null : vm.Imagen,
                Estado = true,
                FechaCreacion = DateTime.Now
            };

            await _repo.CreateAsync(categoria);
            return (true, "Categoría creada correctamente.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(CategoriaEditViewModel vm)
        {
            var c = await _repo.GetByIdAsync(vm.CategoriaId);
            if (c == null) return (false, "Categoría no encontrada.");

            if (await _repo.ExistsByNombreAsync(vm.Nombre, vm.CategoriaId))
                return (false, "Ya existe una categoría con ese nombre.");

            c.Nombre = vm.Nombre;
            c.Descripcion = string.IsNullOrWhiteSpace(vm.Descripcion) ? null : vm.Descripcion;
            c.Imagen = string.IsNullOrWhiteSpace(vm.Imagen) ? null : vm.Imagen;
            c.Estado = vm.Estado;

            await _repo.UpdateAsync(c);
            return (true, "Categoría actualizada correctamente.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var ok = await _repo.DeleteAsync(id);
            return ok
                ? (true, "Categoría desactivada correctamente.")
                : (false, "Categoría no encontrada.");
        }
    }
}
