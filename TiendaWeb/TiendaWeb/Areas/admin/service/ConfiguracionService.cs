using TiendaWeb.Areas.admin.ViewModel;

namespace TiendaWeb.Areas.admin.service
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly IConfiguracionRepository _repo;
        public ConfiguracionService(IConfiguracionRepository repo) => _repo = repo;

        public async Task<ConfiguracionViewModel?> GetAsync()
        {
            var c = await _repo.GetAsync();
            if (c == null) return null;

            return new ConfiguracionViewModel
            {
                ConfigId = c.ConfigId,
                NombreNegocio = c.NombreNegocio,
                Ruc = c.Ruc,
                Direccion = c.Direccion,
                Distrito = c.Distrito,
                Provincia = c.Provincia,
                Departamento = c.Departamento,
                Telefono = c.Telefono,
                Email = c.Email,
                MensajeTicket = c.MensajeTicket,
                IgvPorcentaje = c.IgvPorcentaje ?? 18,
                Moneda = c.Moneda ?? "PEN",
                SimboloMoneda = c.SimboloMoneda ?? "S/",
                SerieTicket = c.SerieTicket ?? "T001",
                SerieBoleta = c.SerieBoleta ?? "B001",
                SerieFactura = c.SerieFactura ?? "F001",
                SerieOrdenCompra = c.SerieOrdenCompra ?? "OC01",
                DiasDevolucion = c.DiasDevolucion ?? 7,
            };
        }

        public async Task<(bool Success, string Message)> UpdateAsync(ConfiguracionViewModel vm)
        {
            var c = await _repo.GetAsync();
            if (c == null) return (false, "No se encontró la configuración.");

            c.NombreNegocio = vm.NombreNegocio;
            c.Ruc = vm.Ruc;
            c.Direccion = vm.Direccion;
            c.Distrito = vm.Distrito;
            c.Provincia = vm.Provincia;
            c.Departamento = vm.Departamento;
            c.Telefono = vm.Telefono;
            c.Email = vm.Email;
            c.MensajeTicket = vm.MensajeTicket;
            c.IgvPorcentaje = vm.IgvPorcentaje;
            c.Moneda = vm.Moneda;
            c.SimboloMoneda = vm.SimboloMoneda;
            c.SerieTicket = vm.SerieTicket;
            c.SerieBoleta = vm.SerieBoleta;
            c.SerieFactura = vm.SerieFactura;
            c.SerieOrdenCompra = vm.SerieOrdenCompra;
            c.DiasDevolucion = vm.DiasDevolucion;

            await _repo.UpdateAsync(c);
            return (true, "Configuración guardada correctamente.");
        }
    }
}
