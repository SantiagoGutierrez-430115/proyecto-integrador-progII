using Cine.Data.DTOs;
using Cine.Data.Exceptions;
using Cine.Data.Models;
using Cine.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Repositories.Implementations
{
    public class ProductoShopRepository : IProductoShopRepository
    {
        private readonly CineDbContext _context;

        public ProductoShopRepository(CineDbContext context)
        {
            _context = context;
        }

        private IQueryable<ProductosShop> ConDetalle() =>
            _context.ProductosShops
                .Include(p => p.IdMarcaNavigation)
                .Include(p => p.IdTipoProductoNavigation);

        public async Task<ResultadoPaginadoDto<ProductosShop>> GetAllAsync(
            string? nombre, int? idMarca, int? idTipoProducto, int pagina, int tamanio)
        {
            var query = ConDetalle();

            // Filtros opcionales
            if (!string.IsNullOrWhiteSpace(nombre))
                query = query.Where(p => p.Nombre.Contains(nombre));
            if (idMarca.HasValue)
                query = query.Where(p => p.IdMarca == idMarca.Value);
            if (idTipoProducto.HasValue)
                query = query.Where(p => p.IdTipoProducto == idTipoProducto.Value);

            // Total de productos que cumplen los filtros (antes de paginar)
            var total = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.IdProductoShop)
                .Skip((pagina - 1) * tamanio)
                .Take(tamanio)
                .ToListAsync();

            return new ResultadoPaginadoDto<ProductosShop>
            {
                Items = items,
                Total = total,
                Pagina = pagina,
                Tamanio = tamanio
            };
        }

        public Task<ProductosShop?> GetByIdAsync(int id) =>
            ConDetalle().FirstOrDefaultAsync(p => p.IdProductoShop == id);

        public async Task<ProductosShop> CreateAsync(ProductosShop producto)
        {
            _context.ProductosShops.Add(producto);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // La base rechazó el insert: la marca o el tipo de producto no existen
                throw new DatosInvalidosException("La marca o el tipo de producto indicado no existe.");
            }

            return producto;
        }

        public async Task<bool> UpdateAsync(int id, ProductosShop datos)
        {
            var producto = await _context.ProductosShops.FirstOrDefaultAsync(p => p.IdProductoShop == id);
            if (producto == null) return false;

            producto.Nombre = datos.Nombre;
            producto.Precio = datos.Precio;
            producto.Descripcion = datos.Descripcion;
            producto.Stock = datos.Stock;
            producto.IdMarca = datos.IdMarca;
            producto.IdTipoProducto = datos.IdTipoProducto;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // La marca o el tipo no existen, o el producto está en facturas pagadas o anuladas
                throw new DatosInvalidosException(
                    "No se pudo guardar el producto: verificá que la marca y el tipo existan, " +
                    "y que no se cambie nombre, marca o tipo de un producto de facturas pagadas o anuladas.");
            }

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var producto = await _context.ProductosShops.FirstOrDefaultAsync(p => p.IdProductoShop == id);
            if (producto == null) return false;

            _context.ProductosShops.Remove(producto);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ConflictoException("No se puede eliminar el producto porque está asociado a facturas, combos o promociones.");
            }

            return true;
        }
    }
}
