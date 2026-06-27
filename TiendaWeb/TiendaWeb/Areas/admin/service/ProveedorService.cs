using TiendaWeb.Areas.admin.repository;
using TiendaWeb.Areas.admin.ViewModel;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.service
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _repo;
        public ProveedorService(IProveedorRepository repo) => _repo = repo;

        public async Task<List<ProveedorListViewModel>> GetAllAsync()
        {
            var lista = await _repo.GetAllAsync();
            return lista.Select(p => new ProveedorListViewModel
            {
                ProveedorId = p.ProveedorId,
                Codigo = p.Codigo,
                RazonSocial = p.RazonSocial,
                NombreComercial = p.NombreComercial,
                TipoDocumento = p.TipoDocumento,
                RucDni = p.RucDni,
                Telefono = p.Telefono,
                Email = p.Email,
                ContactoNombre = p.ContactoNombre,
                ContactoTelefono = p.ContactoTelefono,
                DiasCredito = p.DiasCredito,
                LimiteCredito = p.LimiteCredito,
                SaldoCredito = p.SaldoCredito,
                Estado = p.Estado,
                FechaCreacion = p.FechaCreacion
            }).ToList();
        }

        public async Task<ProveedorEditViewModel?> GetForEditAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;
            return new ProveedorEditViewModel
            {
                ProveedorId = p.ProveedorId,
                Codigo = p.Codigo,
                RazonSocial = p.RazonSocial,
                NombreComercial = p.NombreComercial,
                TipoDocumento = p.TipoDocumento ?? "RUC",
                RucDni = p.RucDni,
                Direccion = p.Direccion,
                Telefono = p.Telefono,
                Email = p.Email,
                ContactoNombre = p.ContactoNombre,
                ContactoTelefono = p.ContactoTelefono,
                DiasCredito = p.DiasCredito ?? 0,
                LimiteCredito = p.LimiteCredito ?? 0,
                Estado = p.Estado ?? true
            };
        }

        public async Task<(bool Success, string Message)> CreateAsync(ProveedorCreateViewModel vm)
        {
            if (await _repo.ExistsByCodigoAsync(vm.Codigo))
                return (false, "El código ya está en uso.");

            if (await _repo.ExistsByRucDniAsync(vm.RucDni))
                return (false, "El RUC/DNI ya está registrado.");

            var p = new Proveedore
            {
                Codigo = vm.Codigo,
                RazonSocial = vm.RazonSocial,
                NombreComercial = string.IsNullOrWhiteSpace(vm.NombreComercial) ? null : vm.NombreComercial,
                TipoDocumento = vm.TipoDocumento,
                RucDni = vm.RucDni,
                Direccion = string.IsNullOrWhiteSpace(vm.Direccion) ? null : vm.Direccion,
                Telefono = string.IsNullOrWhiteSpace(vm.Telefono) ? null : vm.Telefono,
                Email = string.IsNullOrWhiteSpace(vm.Email) ? null : vm.Email,
                ContactoNombre = string.IsNullOrWhiteSpace(vm.ContactoNombre) ? null : vm.ContactoNombre,
                ContactoTelefono = string.IsNullOrWhiteSpace(vm.ContactoTelefono) ? null : vm.ContactoTelefono,
                DiasCredito = vm.DiasCredito,
                LimiteCredito = vm.LimiteCredito,
                SaldoCredito = 0,
                Estado = true,
                FechaCreacion = DateTime.Now
            };

            await _repo.CreateAsync(p);
            return (true, "Proveedor creado correctamente.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(ProveedorEditViewModel vm)
        {
            var p = await _repo.GetByIdAsync(vm.ProveedorId);
            if (p == null) return (false, "Proveedor no encontrado.");

            if (await _repo.ExistsByCodigoAsync(vm.Codigo, vm.ProveedorId))
                return (false, "El código ya está en uso.");

            if (await _repo.ExistsByRucDniAsync(vm.RucDni, vm.ProveedorId))
                return (false, "El RUC/DNI ya está registrado.");

            p.Codigo = vm.Codigo;
            p.RazonSocial = vm.RazonSocial;
            p.NombreComercial = string.IsNullOrWhiteSpace(vm.NombreComercial) ? null : vm.NombreComercial;
            p.TipoDocumento = vm.TipoDocumento;
            p.RucDni = vm.RucDni;
            p.Direccion = string.IsNullOrWhiteSpace(vm.Direccion) ? null : vm.Direccion;
            p.Telefono = string.IsNullOrWhiteSpace(vm.Telefono) ? null : vm.Telefono;
            p.Email = string.IsNullOrWhiteSpace(vm.Email) ? null : vm.Email;
            p.ContactoNombre = string.IsNullOrWhiteSpace(vm.ContactoNombre) ? null : vm.ContactoNombre;
            p.ContactoTelefono = string.IsNullOrWhiteSpace(vm.ContactoTelefono) ? null : vm.ContactoTelefono;
            p.DiasCredito = vm.DiasCredito;
            p.LimiteCredito = vm.LimiteCredito;
            p.Estado = vm.Estado;

            await _repo.UpdateAsync(p);
            return (true, "Proveedor actualizado correctamente.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var ok = await _repo.DeleteAsync(id);
            return ok
                ? (true, "Proveedor desactivado correctamente.")
                : (false, "Proveedor no encontrado.");
        }
    }
}
