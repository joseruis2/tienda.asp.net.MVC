using TiendaWeb.Areas.cajero.ViewModel;

namespace TiendaWeb.Areas.cajero.Service
{
    public interface IClienteCajeroService
    {
        Task<ClientesCajeroIndexViewModel> GetIndexDataAsync(string? busqueda);
        Task<ClienteRapidoEditViewModel?> GetForEditAsync(int id);
        Task<(bool Success, string Message, int? ClienteId)> CreateAsync(
            ClienteRapidoCreateViewModel vm);
        Task<(bool Success, string Message)> UpdateAsync(
            ClienteRapidoEditViewModel vm);
    }
}
