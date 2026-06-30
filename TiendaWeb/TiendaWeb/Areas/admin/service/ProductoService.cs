using Microsoft.EntityFrameworkCore;
using TiendaWeb.Areas.admin.repository;
using TiendaWeb.Areas.admin.ViewModel;
using TiendaWeb.Infrastructure.Data;
using TiendaWeb.Infrastructure.Models;

namespace TiendaWeb.Areas.admin.service
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repo;
        private readonly AppDbContext _db;

        public ProductoService(IProductoRepository repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        public async Task<ProductosIndexViewModel> GetIndexDataAsync()
        {
            var productos = await _repo.GetAllWithRelationsAsync();

            return new ProductosIndexViewModel
            {
                Lista = productos.Select(p => new ProductoListViewModel
                {
                    ProductoId = p.ProductoId,
                    Codigo = p.Codigo,
                    CodigoBarras = p.CodigoBarras,
                    Nombre = p.Nombre,
                    CategoriaNombre = p.Categoria?.Nombre,
                    ProveedorNombre = p.Proveedor?.RazonSocial,
                    PrecioCosto = p.PrecioCosto ?? 0,
                    Precio = p.Precio ?? 0,
                    PrecioMayorista = p.PrecioMayorista ?? 0,
                    StockActual = p.StockActual ?? 0,
                    StockMinimo = p.StockMinimo ?? 5,
                    Estado = p.Estado
                }).ToList(),
                Categorias = await _db.Categorias
                    .Where(c => c.Estado == true)
                    .Select(c => new SelectItemViewModel
                    { Id = c.CategoriaId, Nombre = c.Nombre })
                    .ToListAsync(),
                Proveedores = await _db.Proveedores
                    .Where(p => p.Estado == true)
                    .Select(p => new SelectItemViewModel
                    { Id = p.ProveedorId, Nombre = p.RazonSocial })
                    .ToListAsync()
            };
        }

        public async Task<ProductoEditViewModel?> GetForEditAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;
            return new ProductoEditViewModel
            {
                ProductoId = p.ProductoId,
                Codigo = p.Codigo,
                CodigoBarras = p.CodigoBarras,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                CategoriaId = p.CategoriaId,
                ProveedorId = p.ProveedorId,
                PrecioCosto = p.PrecioCosto ?? 0,
                Precio = p.Precio ?? 0,
                PrecioMayorista = p.PrecioMayorista ?? 0,
                StockMinimo = p.StockMinimo ?? 5,
                StockMaximo = p.StockMaximo ?? 100,
                UnidadMedida = p.UnidadMedida ?? "Unidad",
                Peso = p.Peso ?? 0,
                Ubicacion = p.Ubicacion,
                FechaVencimiento = p.FechaVencimiento,
                ImagenPrincipal = p.ImagenPrincipal,
                Estado = p.Estado ?? true
            };
        }

        public async Task<(bool Success, string Message)> CreateAsync(ProductoCreateViewModel vm)
        {
            if (await _repo.ExistsByCodigoAsync(vm.Codigo))
                return (false, "El código ya está en uso.");

            if (!string.IsNullOrWhiteSpace(vm.CodigoBarras) &&
                await _repo.ExistsByCodigoBarrasAsync(vm.CodigoBarras))
                return (false, "El código de barras ya está registrado.");

            var p = new Producto
            {
                Codigo = vm.Codigo,
                CodigoBarras = string.IsNullOrWhiteSpace(vm.CodigoBarras) ? null : vm.CodigoBarras,
                Nombre = vm.Nombre,
                Descripcion = string.IsNullOrWhiteSpace(vm.Descripcion) ? null : vm.Descripcion,
                CategoriaId = vm.CategoriaId,
                ProveedorId = vm.ProveedorId,
                PrecioCosto = vm.PrecioCosto,
                Precio = vm.Precio,
                PrecioMayorista = vm.PrecioMayorista,
                StockActual = vm.StockActual,
                StockMinimo = vm.StockMinimo,
                StockMaximo = vm.StockMaximo,
                UnidadMedida = vm.UnidadMedida,
                Peso = vm.Peso,
                Ubicacion = string.IsNullOrWhiteSpace(vm.Ubicacion) ? null : vm.Ubicacion,
                FechaVencimiento = vm.FechaVencimiento,
                ImagenPrincipal = string.IsNullOrWhiteSpace(vm.ImagenPrincipal) ? null : vm.ImagenPrincipal,
                Estado = true,
                FechaCreacion = DateTime.Now
            };

            await _repo.CreateAsync(p);
            return (true, "Producto creado correctamente.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(ProductoEditViewModel vm)
        {
            var p = await _repo.GetByIdAsync(vm.ProductoId);
            if (p == null) return (false, "Producto no encontrado.");

            if (await _repo.ExistsByCodigoAsync(vm.Codigo, vm.ProductoId))
                return (false, "El código ya está en uso.");

            if (!string.IsNullOrWhiteSpace(vm.CodigoBarras) &&
                await _repo.ExistsByCodigoBarrasAsync(vm.CodigoBarras, vm.ProductoId))
                return (false, "El código de barras ya está registrado.");

            p.Codigo = vm.Codigo;
            p.CodigoBarras = string.IsNullOrWhiteSpace(vm.CodigoBarras) ? null : vm.CodigoBarras;
            p.Nombre = vm.Nombre;
            p.Descripcion = string.IsNullOrWhiteSpace(vm.Descripcion) ? null : vm.Descripcion;
            p.CategoriaId = vm.CategoriaId;
            p.ProveedorId = vm.ProveedorId;
            p.PrecioCosto = vm.PrecioCosto;
            p.Precio = vm.Precio;
            p.PrecioMayorista = vm.PrecioMayorista;
            p.StockMinimo = vm.StockMinimo;
            p.StockMaximo = vm.StockMaximo;
            p.UnidadMedida = vm.UnidadMedida;
            p.Peso = vm.Peso;
            p.Ubicacion = string.IsNullOrWhiteSpace(vm.Ubicacion) ? null : vm.Ubicacion;
            p.FechaVencimiento = vm.FechaVencimiento;
            p.ImagenPrincipal = string.IsNullOrWhiteSpace(vm.ImagenPrincipal) ? null : vm.ImagenPrincipal;
            p.Estado = vm.Estado;

            await _repo.UpdateAsync(p);
            return (true, "Producto actualizado correctamente.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var ok = await _repo.DeleteAsync(id);
            return ok
                ? (true, "Producto desactivado correctamente.")
                : (false, "Producto no encontrado.");
        }
    }
}
