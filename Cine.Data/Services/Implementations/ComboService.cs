using Cine.Data.DTOs;
using Cine.Data.Exceptions;
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
    public class ComboService : IComboService
    {
        private readonly IComboRepository _repo;

        public ComboService(IComboRepository repo)
        {
            _repo = repo;
        }

        public async Task<ResultadoPaginadoDto<ComboDto>> GetAllAsync(
            string? descripcion, int? idTipoCombo, int pagina, int tamanio)
        {
            // La página mínima es 1, y el tamaño tiene que estar entre 1 y 100
            if (pagina < 1) pagina = 1;
            if (tamanio < 1) tamanio = 1;
            if (tamanio > 100) tamanio = 100;

            var resultado = await _repo.GetAllAsync(descripcion, idTipoCombo, pagina, tamanio);

            // Convierte cada Combo (entidad) en un ComboDto
            var dtos = new List<ComboDto>();
            foreach (var combo in resultado.Items)
            {
                dtos.Add(ToDto(combo));
            }

            return new ResultadoPaginadoDto<ComboDto>
            {
                Items = dtos,
                Total = resultado.Total,
                Pagina = pagina,
                Tamanio = tamanio
            };
        }

        public async Task<ComboDto?> GetByIdAsync(int id)
        {
            var combo = await _repo.GetByIdAsync(id);
            if (combo == null) return null;

            return ToDto(combo);
        }

        public async Task<ComboDto> CreateAsync(GuardarComboDto dto)
        {
            var combo = await ToEntityAsync(dto);

            var creado = await _repo.CreateAsync(combo);

            // Se vuelve a leer para devolver el combo con tipo y nombres de productos
            var completo = await _repo.GetByIdAsync(creado.IdCombo);
            return ToDto(completo!);
        }

        public async Task<bool> UpdateAsync(int id, GuardarComboDto dto)
        {
            var existente = await _repo.GetByIdAsync(id);
            if (existente == null) return false;

            var datos = await ToEntityAsync(dto);

            return await _repo.UpdateAsync(id, datos);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        private async Task<Combo> ToEntityAsync(GuardarComboDto dto)
        {
            var precios = await _repo.GetPreciosProductosAsync(dto.Productos.Select(p => p.IdProductoShop!.Value));

            var combo = new Combo
            {
                Descripcion = dto.Descripcion.Trim(),
                Precio = dto.Precio!.Value,
                IdTipoCombo = dto.IdTipoCombo!.Value
            };

            decimal suma = 0;
            foreach (var item in dto.Productos)
            {
                var idProducto = item.IdProductoShop!.Value;

                if (!precios.TryGetValue(idProducto, out var precioActual))
                    throw new DatosInvalidosException($"El producto {idProducto} no existe.");

                var precioUnitario = item.PrecioUnitario ?? precioActual;
                suma += precioUnitario * item.Cantidad;

                combo.ProductosCombos.Add(new ProductosCombo
                {
                    IdProductoShop = idProducto,
                    PrecioUnitario = precioUnitario,
                    Cantidad = item.Cantidad
                });
            }

            if (combo.Precio > suma)
                throw new DatosInvalidosException(
                    $"El precio del combo ({combo.Precio:0.00}) no puede superar la suma de sus productos ({suma:0.00}).");

            return combo;
        }

        // Pasa una entidad de la base a lo que se devuelve en la API (DTO)
        private static ComboDto ToDto(Combo c)
        {
            return new ComboDto
            {
                IdCombo = c.IdCombo,
                Descripcion = c.Descripcion,
                Precio = c.Precio,
                SumaProductos = c.ProductosCombos.Sum(x => x.PrecioUnitario * x.Cantidad),
                IdTipoCombo = c.IdTipoCombo,
                TipoCombo = c.IdTipoComboNavigation?.Descripcion,
                Productos = c.ProductosCombos
                    .Select(x => new ComboProductoDto
                    {
                        IdProductoShop = x.IdProductoShop,
                        Producto = x.IdProductoShopNavigation?.Nombre,
                        PrecioUnitario = x.PrecioUnitario,
                        Cantidad = x.Cantidad
                    })
                    .OrderBy(x => x.IdProductoShop)
                    .ToList()
            };
        }
    }
}
