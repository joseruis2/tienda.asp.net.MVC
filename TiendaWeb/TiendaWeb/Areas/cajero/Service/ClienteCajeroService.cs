using TiendaWeb.Areas.cajero.Repository;
using TiendaWeb.Areas.cajero.ViewModel;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.cajero.Service
{
    public class ClienteCajeroService : IClienteCajeroService
    {
        private readonly IClienteCajeroRepository _repo;
        public ClienteCajeroService(IClienteCajeroRepository repo) => _repo = repo;

        public async Task<ClientesCajeroIndexViewModel> GetIndexDataAsync(string? busqueda)
        {
            var clientes = await _repo.BuscarAsync(busqueda);

            return new ClientesCajeroIndexViewModel
            {
                Lista = clientes.Select(c => new ClienteBusquedaDto
                {
                    ClienteId = c.ClienteId,
                    Codigo = c.Codigo,
                    TipoDocumento = c.TipoDocumento,
                    NumeroDocumento = c.NumeroDocumento,
                    NombreCompleto = !string.IsNullOrEmpty(c.RazonSocial)
                        ? c.RazonSocial
                        : $"{c.Nombres} {c.Apellidos}".Trim(),
                    Telefono = c.Telefono,
                    Email = c.Email,
                    PuntosAcumulados = c.PuntosAcumulados,
                    TotalCompras = c.TotalCompras
                }).ToList(),
                Busqueda = busqueda,
                Total = clientes.Count
            };
        }

        public async Task<ClienteRapidoEditViewModel?> GetForEditAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;
            return new ClienteRapidoEditViewModel
            {
                ClienteId = c.ClienteId,
                TipoDocumento = c.TipoDocumento ?? "DNI",
                NumeroDocumento = c.NumeroDocumento,
                Nombres = c.Nombres,
                Apellidos = c.Apellidos,
                RazonSocial = c.RazonSocial,
                Telefono = c.Telefono,
                Email = c.Email
            };
        }

        public async Task<(bool Success, string Message, int? ClienteId)> CreateAsync(
            ClienteRapidoCreateViewModel vm)
        {
            if (!string.IsNullOrWhiteSpace(vm.NumeroDocumento) &&
                vm.TipoDocumento != "NINGUNO" &&
                await _repo.ExistsByDocumentoAsync(vm.NumeroDocumento))
                return (false, "El documento ya está registrado.", null);

            var cliente = new Cliente
            {
                TipoDocumento = vm.TipoDocumento,
                NumeroDocumento = string.IsNullOrWhiteSpace(vm.NumeroDocumento)
                    ? null : vm.NumeroDocumento,
                Nombres = string.IsNullOrWhiteSpace(vm.Nombres)
                    ? null : vm.Nombres,
                Apellidos = string.IsNullOrWhiteSpace(vm.Apellidos)
                    ? null : vm.Apellidos,
                RazonSocial = string.IsNullOrWhiteSpace(vm.RazonSocial)
                    ? null : vm.RazonSocial,
                Telefono = string.IsNullOrWhiteSpace(vm.Telefono)
                    ? null : vm.Telefono,
                Email = string.IsNullOrWhiteSpace(vm.Email)
                    ? null : vm.Email,
                PuntosAcumulados = 0,
                TotalCompras = 0,
                Estado = true,
                FechaCreacion = DateTime.Now
            };

            await _repo.CreateAsync(cliente);
            return (true, "Cliente registrado correctamente.", cliente.ClienteId);
        }

        public async Task<(bool Success, string Message)> UpdateAsync(
            ClienteRapidoEditViewModel vm)
        {
            var c = await _repo.GetByIdAsync(vm.ClienteId);
            if (c == null) return (false, "Cliente no encontrado.");

            if (!string.IsNullOrWhiteSpace(vm.NumeroDocumento) &&
                vm.TipoDocumento != "NINGUNO" &&
                await _repo.ExistsByDocumentoAsync(vm.NumeroDocumento, vm.ClienteId))
                return (false, "El documento ya está registrado.");

            c.TipoDocumento = vm.TipoDocumento;
            c.NumeroDocumento = string.IsNullOrWhiteSpace(vm.NumeroDocumento)
                ? null : vm.NumeroDocumento;
            c.Nombres = string.IsNullOrWhiteSpace(vm.Nombres)
                ? null : vm.Nombres;
            c.Apellidos = string.IsNullOrWhiteSpace(vm.Apellidos)
                ? null : vm.Apellidos;
            c.RazonSocial = string.IsNullOrWhiteSpace(vm.RazonSocial)
                ? null : vm.RazonSocial;
            c.Telefono = string.IsNullOrWhiteSpace(vm.Telefono)
                ? null : vm.Telefono;
            c.Email = string.IsNullOrWhiteSpace(vm.Email)
                ? null : vm.Email;

            await _repo.UpdateAsync(c);
            return (true, "Cliente actualizado correctamente.");
        }
    }
}
