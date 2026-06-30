using Microsoft.EntityFrameworkCore;
using TiendaWeb.Areas.admin.repository;
using TiendaWeb.Areas.admin.ViewModel;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.service
{
    public class PromocionService : IPromocionService
    {
        private readonly IPromocionRepository _repo;
        private readonly AppDbContext _db;

        public PromocionService(IPromocionRepository repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        public async Task<PromocionesIndexViewModel> GetIndexDataAsync()
        {
            var lista = await _repo.GetAllWithRelationsAsync();

            return new PromocionesIndexViewModel
            {
                Lista = lista.Select(p => new PromocionListViewModel
                {
                    PromoId = p.PromoId,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Tipo = p.Tipo,
                    Valor = p.Valor,
                    AplicaA = p.AplicaA ?? "PRODUCTO",
                    ProductoNombre = p.Producto?.Nombre,
                    CategoriaNombre = p.Categoria?.Nombre,
                    FechaInicio = p.FechaInicio,
                    FechaFin = p.FechaFin,
                    HoraInicio = p.HoraInicio,
                    HoraFin = p.HoraFin,
                    MontoMinimo = p.MontoMinimo ?? 0,
                    CantidadMinima = p.CantidadMinima ?? 1,
                    Estado = p.Estado
                }).ToList(),
                Productos = await _db.Productos
                    .Where(p => p.Estado == true)
                    .Select(p => new SelectItemViewModel
                    { Id = p.ProductoId, Nombre = p.Nombre })
                    .ToListAsync(),
                Categorias = await _db.Categorias
                    .Where(c => c.Estado == true)
                    .Select(c => new SelectItemViewModel
                    { Id = c.CategoriaId, Nombre = c.Nombre })
                    .ToListAsync()
            };
        }

        public async Task<PromocionEditViewModel?> GetForEditAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;
            return new PromocionEditViewModel
            {
                PromoId = p.PromoId,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Tipo = p.Tipo,
                Valor = p.Valor,
                AplicaA = p.AplicaA ?? "PRODUCTO",
                ProductoId = p.ProductoId,
                CategoriaId = p.CategoriaId,
                FechaInicio = p.FechaInicio,
                FechaFin = p.FechaFin,
                HoraInicio = p.HoraInicio,
                HoraFin = p.HoraFin,
                MontoMinimo = p.MontoMinimo ?? 0,
                CantidadMinima = p.CantidadMinima ?? 1,
                Estado = p.Estado ?? true
            };
        }

        public async Task<(bool Success, string Message)> CreateAsync(PromocionCreateViewModel vm)
        {
            if (await _repo.ExistsByNombreAsync(vm.Nombre))
                return (false, "Ya existe una promoción con ese nombre.");

            var p = new Promocione
            {
                Nombre = vm.Nombre,
                Descripcion = string.IsNullOrWhiteSpace(vm.Descripcion) ? null : vm.Descripcion,
                Tipo = vm.Tipo,
                Valor = vm.Valor,
                AplicaA = vm.AplicaA,
                ProductoId = vm.AplicaA == "PRODUCTO" ? vm.ProductoId : null,
                CategoriaId = vm.AplicaA == "CATEGORIA" ? vm.CategoriaId : null,
                FechaInicio = vm.FechaInicio,
                FechaFin = vm.FechaFin,
                HoraInicio = vm.HoraInicio,
                HoraFin = vm.HoraFin,
                MontoMinimo = vm.MontoMinimo,
                CantidadMinima = vm.CantidadMinima,
                Estado = true,
                FechaCreacion = DateTime.Now
            };

            await _repo.CreateAsync(p);
            return (true, "Promoción creada correctamente.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(PromocionEditViewModel vm)
        {
            var p = await _repo.GetByIdAsync(vm.PromoId);
            if (p == null) return (false, "Promoción no encontrada.");

            if (await _repo.ExistsByNombreAsync(vm.Nombre, vm.PromoId))
                return (false, "Ya existe una promoción con ese nombre.");

            p.Nombre = vm.Nombre;
            p.Descripcion = string.IsNullOrWhiteSpace(vm.Descripcion) ? null : vm.Descripcion;
            p.Tipo = vm.Tipo;
            p.Valor = vm.Valor;
            p.AplicaA = vm.AplicaA;
            p.ProductoId = vm.AplicaA == "PRODUCTO" ? vm.ProductoId : null;
            p.CategoriaId = vm.AplicaA == "CATEGORIA" ? vm.CategoriaId : null;
            p.FechaInicio = vm.FechaInicio;
            p.FechaFin = vm.FechaFin;
            p.HoraInicio = vm.HoraInicio;
            p.HoraFin = vm.HoraFin;
            p.MontoMinimo = vm.MontoMinimo;
            p.CantidadMinima = vm.CantidadMinima;
            p.Estado = vm.Estado;

            await _repo.UpdateAsync(p);
            return (true, "Promoción actualizada correctamente.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var ok = await _repo.DeleteAsync(id);
            return ok
                ? (true, "Promoción desactivada correctamente.")
                : (false, "Promoción no encontrada.");
        }
    }
}
