using TiendaWeb.Infrastructure.Models;
using TiendaWeb.Infrastructure.repositories;

namespace TiendaWeb.Infrastructure.service
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepo;

        public AuthService(IUsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        public async Task<Usuario?> ValidateUserAsync(string username, string password)
        {
            var usuario = await _usuarioRepo.GetByUsernameAsync(username);
            if (usuario == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(password, usuario.Password)) return null;

            usuario.UltimoAcceso = DateTime.Now;
            await _usuarioRepo.UpdateAsync(usuario);

            return usuario;
        }

        public async Task<(bool Success, string Message)> RegisterUserAsync(
            string username, string password,
            string nombres, string apellidos,
            string? dni, string? email, string? telefono,
            string rol, int? creadoPor)
        {
            if (await _usuarioRepo.ExistsByUsernameAsync(username))
                return (false, "El nombre de usuario ya está en uso.");

            if (!string.IsNullOrWhiteSpace(dni) && await _usuarioRepo.ExistsByDniAsync(dni))
                return (false, "El DNI ya está registrado.");

            var nuevoUsuario = new Usuario
            {
                Username = username,
                Password = HashPassword(password),
                Nombres = nombres,
                Apellidos = apellidos,
                Dni = string.IsNullOrWhiteSpace(dni) ? null : dni,
                Email = string.IsNullOrWhiteSpace(email) ? null : email,
                Telefono = string.IsNullOrWhiteSpace(telefono) ? null : telefono,
                Rol = rol,
                Estado = true,
                FechaCreacion = DateTime.Now,
                CreadoPor = creadoPor
            };

            await _usuarioRepo.CreateAsync(nuevoUsuario);
            return (true, "Usuario registrado correctamente.");
        }

        public string HashPassword(string password)
            => BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

        public bool VerifyPassword(string password, string hash)
            => BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
