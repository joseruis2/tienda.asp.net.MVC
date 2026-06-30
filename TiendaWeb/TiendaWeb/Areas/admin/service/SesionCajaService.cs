using Microsoft.EntityFrameworkCore;
using TiendaWeb.Areas.admin.repository;
using TiendaWeb.Areas.admin.ViewModel;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.service
{
    public class SesionCajaService : ISesionCajaService
    {
        private readonly ISesionCajaRepository _repo;
        private readonly AppDbContext _db;

        public SesionCajaService(ISesionCajaRepository repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        public async Task<SesionesCajaIndexViewModel> GetIndexDataAsync(int usuarioId)
        {
            var historial = await _repo.GetAllWithRelationsAsync();
            var sesionActiva = await _repo.GetSesionAbiertaAsync();

            // Cajas disponibles (activas y cerradas)
            var cajasDisponibles = await _db.Cajas
                .Where(c => c.Activa == true && c.Estado == "CERRADA")
                .Select(c => new SelectItemViewModel
                {
                    Id = c.CajaId,
                    Nombre = $"Caja #{c.Numero} — {c.Nombre}"
                })
                .ToListAsync();

            return new SesionesCajaIndexViewModel
            {
                Historial = historial.Select(s => new SesionCajaListViewModel
                {
                    SesionId = s.SesionId,
                    CajaNombre = s.Caja?.Nombre ?? "",
                    CajaNumero = s.Caja?.Numero ?? 0,
                    UsuarioNombre = s.Usuario != null
                        ? $"{s.Usuario.Nombres} {s.Usuario.Apellidos}" : "",
                    MontoApertura = s.MontoApertura ?? 0,
                    MontoCierre = s.MontoCierre,
                    MontoSistema = s.MontoSistema,
                    Diferencia = s.Diferencia,
                    Estado = s.Estado ?? "CERRADA",
                    FechaApertura = s.FechaApertura ?? DateTime.Now,
                    FechaCierre = s.FechaCierre,
                    Observaciones = s.Observaciones
                }).ToList(),

                SesionActiva = sesionActiva == null ? null : new SesionCajaListViewModel
                {
                    SesionId = sesionActiva.SesionId,
                    CajaNombre = sesionActiva.Caja?.Nombre ?? "",
                    CajaNumero = sesionActiva.Caja?.Numero ?? 0,
                    UsuarioNombre = sesionActiva.Usuario != null
                        ? $"{sesionActiva.Usuario.Nombres} {sesionActiva.Usuario.Apellidos}" : "",
                    MontoApertura = sesionActiva.MontoApertura ?? 0,
                    Estado = sesionActiva.Estado ?? "ABIERTA",
                    FechaApertura = sesionActiva.FechaApertura ?? DateTime.Now
                },

                CajasDisponibles = cajasDisponibles
            };
        }

        public async Task<CerrarCajaViewModel?> GetCierreDataAsync(int sesionId)
        {
            var s = await _repo.GetByIdAsync(sesionId);
            if (s == null || s.Estado != "ABIERTA") return null;

            // Monto sistema = apertura + ventas en efectivo
            var ventasEfectivo = await _repo.GetVentasTotalesSesionAsync(sesionId);
            var montoSistema = (s.MontoApertura ?? 0) + ventasEfectivo;

            return new CerrarCajaViewModel
            {
                SesionId = s.SesionId,
                CajaNombre = s.Caja?.Nombre ?? "",
                CajaNumero = s.Caja?.Numero ?? 0,
                MontoApertura = s.MontoApertura ?? 0,
                MontoSistema = montoSistema,
                FechaApertura = s.FechaApertura ?? DateTime.Now,
                MontoCierre = montoSistema  // valor sugerido
            };
        }

        public async Task<(bool Success, string Message)> AbrirCajaAsync(
            AbrirCajaViewModel vm, int usuarioId)
        {
            // Verificar que no haya sesión abierta
            var sesionExistente = await _repo.GetSesionAbiertaAsync();
            if (sesionExistente != null)
                return (false, "Ya existe una sesión de caja abierta.");

            var caja = await _db.Cajas.FindAsync(vm.CajaId);
            if (caja == null || caja.Activa == false)
                return (false, "Caja no disponible.");

            // Crear sesión
            var sesion = new SesionesCaja
            {
                CajaId = vm.CajaId,
                UsuarioId = usuarioId,
                MontoApertura = vm.MontoApertura,
                Estado = "ABIERTA",
                FechaApertura = DateTime.Now,
                Observaciones = vm.Observaciones
            };

            await _repo.CreateAsync(sesion);

            // Actualizar estado de la caja
            caja.Estado = "ABIERTA";
            caja.SesionActual = sesion.SesionId;
            _db.Cajas.Update(caja);
            await _db.SaveChangesAsync();

            return (true, $"Caja #{caja.Numero} abierta correctamente.");
        }

        public async Task<(bool Success, string Message)> CerrarCajaAsync(CerrarCajaViewModel vm)
        {
            var sesion = await _repo.GetByIdAsync(vm.SesionId);
            if (sesion == null || sesion.Estado != "ABIERTA")
                return (false, "Sesión no encontrada o ya cerrada.");

            // Recalcular monto sistema
            var ventasEfectivo = await _repo.GetVentasTotalesSesionAsync(vm.SesionId);
            var montoSistema = (sesion.MontoApertura ?? 0) + ventasEfectivo;

            sesion.MontoCierre = vm.MontoCierre;
            sesion.MontoSistema = montoSistema;
            sesion.Diferencia = vm.MontoCierre - montoSistema;
            sesion.Estado = "CERRADA";
            sesion.FechaCierre = DateTime.Now;
            sesion.Observaciones = vm.Observaciones;

            await _repo.UpdateAsync(sesion);

            // Actualizar estado de la caja
            var caja = await _db.Cajas.FindAsync(sesion.CajaId);
            if (caja != null)
            {
                caja.Estado = "CERRADA";
                caja.SesionActual = null;
                _db.Cajas.Update(caja);
                await _db.SaveChangesAsync();
            }

            var diferencia = sesion.Diferencia ?? 0;
            var msg = diferencia == 0
                ? "Caja cerrada. Sin diferencia."
                : diferencia > 0
                    ? $"Caja cerrada. Sobrante: S/ {diferencia:N2}"
                    : $"Caja cerrada. Faltante: S/ {Math.Abs(diferencia):N2}";

            return (true, msg);
        }
    }
    }
