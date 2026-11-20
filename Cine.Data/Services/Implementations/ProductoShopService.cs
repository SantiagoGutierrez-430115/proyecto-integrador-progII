using Cine.Data.DTOs;
using Cine.Data.Models;
using Cine.Data.Repositories.Interfaces;
using Cine.Data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Services.Implementations
{
    public class ProductoShopService : IProductoShopService
    {
        private readonly IProductoShopRepository _repo;

        public ProductoShopService(IProductoShopRepository repo)
        {
            _repo = repo;
        }

        public async Task<ResultadoPaginadoDto<ProductoShopDto>> GetAllAsync(
            string? nombre, int? idMarca, int? idTipoProducto, int pagina, int tamanio)
        {
            // La página mínima es 1, y el tamaño tiene que estar entre 1 y 100
            if (pagina < 1) pagina = 1;
            if (tamanio < 1) tamanio = 1;
            if (tamanio > 100) tamanio = 100;

            var resultado = await _repo.GetAllAsync(nombre, idMarca, idTipoProducto, pagina, tamanio);

            // Convierte cada Productos en un ProductoShopDto
            var dtos = new List<ProductoShopDto>();
            foreach (var producto in resultado.Items)
            {
                dtos.Add(ToDto(producto));
            }

            return new ResultadoPaginadoDto<ProductoShopDto>
            {
                Items = dtos,
                Total = resultado.Total,
                Pagina = pagina,
                Tamanio = tamanio
            };
        }

        public async Task<ProductoShopDto?> GetByIdAsync(int id)
        {
            var producto = await _repo.GetByIdAsync(id);
            if (producto == null) return null;

            return ToDto(producto);
        }

        public async Task<ProductoShopDto> CreateAsync(GuardarProductoShopDto dto)
        {
            var creado = await _repo.CreateAsync(ToEntity(dto));

            // Se vuelve a leer para devolver el producto con marca y tipo de producto
            var completo = await _repo.GetByIdAsync(creado.IdProductoShop);
            return ToDto(completo!);
        }

        public async Task<bool> UpdateAsync(int id, GuardarProductoShopDto dto)
        {
            return await _repo.UpdateAsync(id, ToEntity(dto));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        // Pasa los datos que llegan a una entidad de la base
        private static ProductosShop ToEntity(GuardarProductoShopDto dto)
        {
            return new ProductosShop
            {
                Nombre = dto.Nombre.Trim(),
                Precio = dto.Precio!.Value,
                Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim(),
                Stock = dto.Stock!.Value,
                IdMarca = dto.IdMarca!.Value,
                IdTipoProducto = dto.IdTipoProducto!.Value
            };
        }

        // Pasa una entidad de la base a lo que se devuelve en la API
        private static ProductoShopDto ToDto(ProductosShop p)
        {
            return new ProductoShopDto
            {
                IdProductoShop = p.IdProductoShop,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Descripcion = p.Descripcion,
                Stock = p.Stock,
                IdMarca = p.IdMarca,
                Marca = p.IdMarcaNavigation?.Marca1,
                IdTipoProducto = p.IdTipoProducto,
                TipoProducto = p.IdTipoProductoNavigation?.TipoProducto
            };
        }
    }
}
