using TiendaWeb.Areas.admin.repository;
using TiendaWeb.Areas.admin.ViewModel;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.service
{
    public class UsuarioAdminService : IUsuarioAdminService
    {
        private readonly IUsuarioAdminRepository _repo;
        public UsuarioAdminService(IUsuarioAdminRepository repo) => _repo = repo;

        public async Task<List<UsuarioListViewModel>> GetAllAsync()
        {
            var lista = await _repo.GetAllAsync();
            return lista.Select(u => new UsuarioListViewModel
            {
                UsuarioId = u.UsuarioId,
                Username = u.Username,
                NombreCompleto = $"{u.Nombres} {u.Apellidos}",
                Dni = u.Dni,
                Email = u.Email,
                Telefono = u.Telefono,
                Rol = u.Rol,
                Estado = u.Estado,
                FechaCreacion = u.FechaCreacion,
                UltimoAcceso = u.UltimoAcceso
            }).ToList();
        }

        public async Task<UsuarioEditViewModel?> GetForEditAsync(int id)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) return null;
            return new UsuarioEditViewModel
            {
                UsuarioId = u.UsuarioId,
                Username = u.Username,
                Nombres = u.Nombres,
                Apellidos = u.Apellidos,
                Dni = u.Dni,
                Email = u.Email,
                Telefono = u.Telefono,
                Rol = u.Rol ?? "CAJERO",
                Estado = u.Estado ?? true
            };
        }

        public async Task<(bool Success, string Message)> CreateAsync(
            UsuarioCreateViewModel vm, int creadoPor)
        {
            if (await _repo.ExistsByUsernameAsync(vm.Username))
                return (false, "El username ya está en uso.");

            if (!string.IsNullOrWhiteSpace(vm.Dni) &&
                await _repo.ExistsByDniAsync(vm.Dni))
                return (false, "El DNI ya está registrado.");

            var usuario = new Usuario
            {
                Username = vm.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(vm.Password, workFactor: 12),
                Nombres = vm.Nombres,
                Apellidos = vm.Apellidos,
                Dni = string.IsNullOrWhiteSpace(vm.Dni) ? null : vm.Dni,
                Email = string.IsNullOrWhiteSpace(vm.Email) ? null : vm.Email,
                Telefono = string.IsNullOrWhiteSpace(vm.Telefono) ? null : vm.Telefono,
                Rol = vm.Rol,
                Estado = true,
                FechaCreacion = DateTime.Now,
                CreadoPor = creadoPor
            };

            await _repo.CreateAsync(usuario);
            return (true, "Usuario creado correctamente.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(UsuarioEditViewModel vm)
        {
            var u = await _repo.GetByIdAsync(vm.UsuarioId);
            if (u == null) return (false, "Usuario no encontrado.");

            if (await _repo.ExistsByUsernameAsync(vm.Username, vm.UsuarioId))
                return (false, "El username ya está en uso.");

            if (!string.IsNullOrWhiteSpace(vm.Dni) &&
                await _repo.ExistsByDniAsync(vm.Dni, vm.UsuarioId))
                return (false, "El DNI ya está registrado.");

            u.Username = vm.Username;
            u.Nombres = vm.Nombres;
            u.Apellidos = vm.Apellidos;
            u.Dni = string.IsNullOrWhiteSpace(vm.Dni) ? null : vm.Dni;
            u.Email = string.IsNullOrWhiteSpace(vm.Email) ? null : vm.Email;
            u.Telefono = string.IsNullOrWhiteSpace(vm.Telefono) ? null : vm.Telefono;
            u.Rol = vm.Rol;
            u.Estado = vm.Estado;

            if (!string.IsNullOrWhiteSpace(vm.NewPassword))
                u.Password = BCrypt.Net.BCrypt.HashPassword(vm.NewPassword, workFactor: 12);

            await _repo.UpdateAsync(u);
            return (true, "Usuario actualizado correctamente.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var ok = await _repo.DeleteAsync(id);
            return ok
                ? (true, "Usuario desactivado correctamente.")
                : (false, "Usuario no encontrado.");
        }
    }
}
