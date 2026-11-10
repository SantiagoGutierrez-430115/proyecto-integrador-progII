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
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }
        public async Task<List<ProductonDto>> ObtenerProductosShopAsync()
        {
            var productos = await _productoRepository.ObtenerProductosShopAsync();

            if(productos is null)
            {
                return null;
            }

            var resultado = new List<ProductonDto>();

            if (productos.Count == 0)
            {
                return resultado;
            }

            foreach (var producto in productos)
            {
                resultado.Add(new ProductonDto
                {
                    IdProducto = producto.IdProductoShop,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Descripcion = producto.Descripcion,
                    Stock = producto.Stock

                });
            }
            return resultado;


        }
    }
}
