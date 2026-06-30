using TiendaWeb.Areas.almacenero.ViewModel;

namespace TiendaWeb.Areas.almacenero.service
{
    public interface IDashboardAlmaceneroService
    {
        Task<DashboardAlmaceneroViewModel> GetDashboardDataAsync();
    }
}
