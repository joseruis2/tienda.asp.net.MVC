using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Infrastructure.service
{
    public interface IAuthService
    {
        Task<Usuario?> ValidateUserAsync(string username, string password);
        Task<(bool Success, string Message)> RegisterUserAsync(string username, string password,string nombres, string apellidos,
            string? dni, string? email, string? telefono,string rol, int? creadoPor);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}
