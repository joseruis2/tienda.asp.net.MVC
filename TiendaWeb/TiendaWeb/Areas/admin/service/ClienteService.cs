using TiendaWeb.Areas.admin.repository;
using TiendaWeb.Areas.admin.ViewModel;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.service
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repo;
        public ClienteService(IClienteRepository repo) => _repo = repo;

        public async Task<List<ClienteListViewModel>> GetAllAsync()
        {
            var lista = await _repo.GetAllAsync();
            return lista.Select(c => new ClienteListViewModel
            {
                ClienteId = c.ClienteId,
                Codigo = c.Codigo,
                TipoDocumento = c.TipoDocumento,
                NumeroDocumento = c.NumeroDocumento,
                Nombres = c.Nombres,
                Apellidos = c.Apellidos,
                RazonSocial = c.RazonSocial,
                Telefono = c.Telefono,
                Email = c.Email,
                PuntosAcumulados = c.PuntosAcumulados,
                TotalCompras = c.TotalCompras,
                Estado = c.Estado,
                FechaCreacion = c.FechaCreacion
            }).ToList();
        }

        public async Task<ClienteEditViewModel?> GetForEditAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;
            return new ClienteEditViewModel
            {
                ClienteId = c.ClienteId,
                Codigo = c.Codigo,
                TipoDocumento = c.TipoDocumento ?? "DNI",
                NumeroDocumento = c.NumeroDocumento,
                Nombres = c.Nombres,
                Apellidos = c.Apellidos,
                RazonSocial = c.RazonSocial,
                Direccion = c.Direccion,
                Telefono = c.Telefono,
                Email = c.Email,
                FechaNacimiento = c.FechaNacimiento,
                Estado = c.Estado ?? true
            };
        }

        public async Task<(bool Success, string Message)> CreateAsync(ClienteCreateViewModel vm)
        {
            if (!string.IsNullOrWhiteSpace(vm.NumeroDocumento) &&
                vm.TipoDocumento != "NINGUNO" &&
                await _repo.ExistsByDocumentoAsync(vm.NumeroDocumento))
                return (false, "El número de documento ya está registrado.");

            if (!string.IsNullOrWhiteSpace(vm.Codigo) &&
                await _repo.ExistsByCodigoAsync(vm.Codigo))
                return (false, "El código ya está en uso.");

            var c = new Cliente
            {
                Codigo = string.IsNullOrWhiteSpace(vm.Codigo) ? null : vm.Codigo,
                TipoDocumento = vm.TipoDocumento,
                NumeroDocumento = string.IsNullOrWhiteSpace(vm.NumeroDocumento) ? null : vm.NumeroDocumento,
                Nombres = string.IsNullOrWhiteSpace(vm.Nombres) ? null : vm.Nombres,
                Apellidos = string.IsNullOrWhiteSpace(vm.Apellidos) ? null : vm.Apellidos,
                RazonSocial = string.IsNullOrWhiteSpace(vm.RazonSocial) ? null : vm.RazonSocial,
                Direccion = string.IsNullOrWhiteSpace(vm.Direccion) ? null : vm.Direccion,
                Telefono = string.IsNullOrWhiteSpace(vm.Telefono) ? null : vm.Telefono,
                Email = string.IsNullOrWhiteSpace(vm.Email) ? null : vm.Email,
                FechaNacimiento = vm.FechaNacimiento,
                PuntosAcumulados = 0,
                TotalCompras = 0,
                Estado = true,
                FechaCreacion = DateTime.Now
            };

            await _repo.CreateAsync(c);
            return (true, "Cliente registrado correctamente.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(ClienteEditViewModel vm)
        {
            var c = await _repo.GetByIdAsync(vm.ClienteId);
            if (c == null) return (false, "Cliente no encontrado.");

            if (!string.IsNullOrWhiteSpace(vm.NumeroDocumento) &&
                vm.TipoDocumento != "NINGUNO" &&
                await _repo.ExistsByDocumentoAsync(vm.NumeroDocumento, vm.ClienteId))
                return (false, "El número de documento ya está registrado.");

            if (!string.IsNullOrWhiteSpace(vm.Codigo) &&
                await _repo.ExistsByCodigoAsync(vm.Codigo, vm.ClienteId))
                return (false, "El código ya está en uso.");

            c.Codigo = string.IsNullOrWhiteSpace(vm.Codigo) ? null : vm.Codigo;
            c.TipoDocumento = vm.TipoDocumento;
            c.NumeroDocumento = string.IsNullOrWhiteSpace(vm.NumeroDocumento) ? null : vm.NumeroDocumento;
            c.Nombres = string.IsNullOrWhiteSpace(vm.Nombres) ? null : vm.Nombres;
            c.Apellidos = string.IsNullOrWhiteSpace(vm.Apellidos) ? null : vm.Apellidos;
            c.RazonSocial = string.IsNullOrWhiteSpace(vm.RazonSocial) ? null : vm.RazonSocial;
            c.Direccion = string.IsNullOrWhiteSpace(vm.Direccion) ? null : vm.Direccion;
            c.Telefono = string.IsNullOrWhiteSpace(vm.Telefono) ? null : vm.Telefono;
            c.Email = string.IsNullOrWhiteSpace(vm.Email) ? null : vm.Email;
            c.FechaNacimiento = vm.FechaNacimiento;
            c.Estado = vm.Estado;

            await _repo.UpdateAsync(c);
            return (true, "Cliente actualizado correctamente.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var ok = await _repo.DeleteAsync(id);
            return ok
                ? (true, "Cliente desactivado correctamente.")
                : (false, "Cliente no encontrado.");
        }
    }
}
