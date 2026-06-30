using TiendaWeb.Areas.admin.repository;
using TiendaWeb.Areas.admin.ViewModel;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.service
{
    public class CajaService : ICajaService
    {
        private readonly ICajaRepository _repo;
        public CajaService(ICajaRepository repo) => _repo = repo;

        public async Task<List<CajaListViewModel>> GetAllAsync()
        {
            var lista = await _repo.GetAllAsync();
            return lista.Select(c => new CajaListViewModel
            {
                CajaId = c.CajaId,
                Nombre = c.Nombre,
                Numero = c.Numero,
                Activa = c.Activa,
                Estado = c.Estado ?? "CERRADA",
                SesionActual = c.SesionActual
            }).ToList();
        }

        public async Task<CajaEditViewModel?> GetForEditAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;
            return new CajaEditViewModel
            {
                CajaId = c.CajaId,
                Nombre = c.Nombre,
                Numero = c.Numero,
                Activa = c.Activa ?? true
            };
        }

        public async Task<(bool Success, string Message)> CreateAsync(CajaCreateViewModel vm)
        {
            if (await _repo.ExistsByNumeroAsync(vm.Numero))
                return (false, $"Ya existe una caja con el número {vm.Numero}.");

            var caja = new Caja
            {
                Nombre = vm.Nombre,
                Numero = vm.Numero,
                Activa = vm.Activa,
                Estado = "CERRADA"
            };

            await _repo.CreateAsync(caja);
            return (true, "Caja creada correctamente.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(CajaEditViewModel vm)
        {
            var c = await _repo.GetByIdAsync(vm.CajaId);
            if (c == null) return (false, "Caja no encontrada.");

            if (await _repo.ExistsByNumeroAsync(vm.Numero, vm.CajaId))
                return (false, $"Ya existe una caja con el número {vm.Numero}.");

            // No editar si está abierta
            if (c.Estado == "ABIERTA")
                return (false, "No se puede editar una caja que está abierta.");

            c.Nombre = vm.Nombre;
            c.Numero = vm.Numero;
            c.Activa = vm.Activa;

            await _repo.UpdateAsync(c);
            return (true, "Caja actualizada correctamente.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var ok = await _repo.DeleteAsync(id);
            return ok
                ? (true, "Caja deshabilitada correctamente.")
                : (false, "No se puede deshabilitar: la caja está abierta o no existe.");
        }
    }
}
