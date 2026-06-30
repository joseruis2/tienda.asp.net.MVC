using Microsoft.EntityFrameworkCore;
using TiendaWeb.Areas.cajero.Repository;
using TiendaWeb.Areas.cajero.ViewModel;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.cajero.Service
{
    public class DashboardCajeroService : IDashboardCajeroService
    {
        private readonly IDashboardCajeroRepository _repo;
        private readonly AppDbContext _db;

        public DashboardCajeroService(
            IDashboardCajeroRepository repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        public async Task<DashboardCajeroViewModel> GetDashboardDataAsync(int usuarioId)
        {
            var sesion = await _repo.GetSesionAbiertaByUsuarioAsync(usuarioId);

            if (sesion == null)
            {
                var cajas = await _repo.GetCajasDisponiblesAsync();
                return new DashboardCajeroViewModel
                {
                    TieneSesionAbierta = false,
                    CajasDisponibles = cajas.Select(c => new CajaDisponibleViewModel
                    {
                        CajaId = c.CajaId,
                        Nombre = c.Nombre,
                        Numero = c.Numero
                    }).ToList()
                };
            }

            var ventas = await _repo.GetVentasDeSesionAsync(sesion.SesionId);

            var ventasEfectivo = ventas.Where(v => v.MetodoPago == "EFECTIVO").Sum(v => v.Total);
            var ventasTarjeta = ventas.Where(v => v.MetodoPago == "TARJETA").Sum(v => v.Total);
            var ventasYape = ventas.Where(v => v.MetodoPago == "YAPE").Sum(v => v.Total);
            var ventasPlin = ventas.Where(v => v.MetodoPago == "PLIN").Sum(v => v.Total);
            var ventasTransferencia = ventas.Where(v => v.MetodoPago == "TRANSFERENCIA").Sum(v => v.Total);

            return new DashboardCajeroViewModel
            {
                TieneSesionAbierta = true,
                SesionId = sesion.SesionId,
                CajaNombre = sesion.Caja?.Nombre,
                CajaNumero = sesion.Caja?.Numero,
                MontoApertura = sesion.MontoApertura ?? 0,
                FechaApertura = sesion.FechaApertura,

                VentasHoy = ventas.Sum(v => v.Total),
                TransaccionesHoy = ventas.Count,
                EfectivoEnCaja = (sesion.MontoApertura ?? 0) + ventasEfectivo,
                VentasEfectivo = ventasEfectivo,
                VentasTarjeta = ventasTarjeta,
                VentasYape = ventasYape,
                VentasPlin = ventasPlin,
                VentasTransferencia = ventasTransferencia,

                UltimasVentas = ventas.Take(10).Select(v => new UltimaVentaCajeroViewModel
                {
                    NumeroTicket = v.NumeroTicket,
                    ClienteNombre = v.ClienteNombre,
                    Total = v.Total,
                    MetodoPago = v.MetodoPago ?? "",
                    Estado = v.Estado ?? "",
                    FechaVenta = v.FechaVenta ?? DateTime.Now
                }).ToList()
            };
        }

        public async Task<(bool Success, string Message)> AbrirCajaAsync(
            AbrirCajaCajeroViewModel vm, int usuarioId)
        {
            var sesionExistente = await _repo.GetSesionAbiertaByUsuarioAsync(usuarioId);
            if (sesionExistente != null)
                return (false, "Ya tienes una sesión de caja abierta.");

            var caja = await _db.Cajas.FindAsync(vm.CajaId);
            if (caja == null || caja.Activa == false)
                return (false, "Caja no disponible.");

            if (caja.Estado == "ABIERTA")
                return (false, "Esta caja ya está siendo usada por otro usuario.");

            var sesion = new SesionesCaja
            {
                CajaId = vm.CajaId,
                UsuarioId = usuarioId,
                MontoApertura = vm.MontoApertura,
                Estado = "ABIERTA",
                FechaApertura = DateTime.Now,
                Observaciones = vm.Observaciones
            };

            _db.SesionesCajas.Add(sesion);
            await _db.SaveChangesAsync();

            caja.Estado = "ABIERTA";
            caja.SesionActual = sesion.SesionId;
            await _db.SaveChangesAsync();

            return (true, $"Caja #{caja.Numero} abierta correctamente.");
        }

        public async Task<CerrarCajaCajeroViewModel?> GetCierreDataAsync(int usuarioId)
        {
            var sesion = await _repo.GetSesionAbiertaByUsuarioAsync(usuarioId);
            if (sesion == null) return null;

            var ventas = await _repo.GetVentasDeSesionAsync(sesion.SesionId);
            var ventasEfectivo = ventas.Where(v => v.MetodoPago == "EFECTIVO").Sum(v => v.Total);
            var montoSistema = (sesion.MontoApertura ?? 0) + ventasEfectivo;

            return new CerrarCajaCajeroViewModel
            {
                SesionId = sesion.SesionId,
                CajaNombre = sesion.Caja?.Nombre ?? "",
                CajaNumero = sesion.Caja?.Numero ?? 0,
                MontoApertura = sesion.MontoApertura ?? 0,
                MontoSistema = montoSistema,
                FechaApertura = sesion.FechaApertura ?? DateTime.Now,
                MontoCierre = montoSistema
            };
        }

        public async Task<(bool Success, string Message)> CerrarCajaAsync(
            CerrarCajaCajeroViewModel vm)
        {
            var sesion = await _db.SesionesCajas
                .FirstOrDefaultAsync(s => s.SesionId == vm.SesionId);

            if (sesion == null || sesion.Estado != "ABIERTA")
                return (false, "Sesión no encontrada o ya cerrada.");

            var ventas = await _repo.GetVentasDeSesionAsync(sesion.SesionId);
            var ventasEfectivo = ventas.Where(v => v.MetodoPago == "EFECTIVO").Sum(v => v.Total);
            var montoSistema = (sesion.MontoApertura ?? 0) + ventasEfectivo;

            sesion.MontoCierre = vm.MontoCierre;
            sesion.MontoSistema = montoSistema;
            sesion.Diferencia = vm.MontoCierre - montoSistema;
            sesion.Estado = "CERRADA";
            sesion.FechaCierre = DateTime.Now;
            sesion.Observaciones = vm.Observaciones;

            await _db.SaveChangesAsync();

            var caja = await _db.Cajas.FindAsync(sesion.CajaId);
            if (caja != null)
            {
                caja.Estado = "CERRADA";
                caja.SesionActual = null;
                await _db.SaveChangesAsync();
            }

            var diferencia = sesion.Diferencia ?? 0;
            var msg = diferencia == 0
                ? "Caja cerrada correctamente. Sin diferencia."
                : diferencia > 0
                    ? $"Caja cerrada. Sobrante: S/ {diferencia:N2}"
                    : $"Caja cerrada. Faltante: S/ {Math.Abs(diferencia):N2}";

            return (true, msg);
        }
    }
}
