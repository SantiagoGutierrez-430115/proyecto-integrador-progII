using Cine.Data.DTOs;
using Cine.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Repositories.Interfaces
{
    public interface IProductoShopRepository
    {
        Task<ResultadoPaginadoDto<ProductosShop>> GetAllAsync(string? nombre, int? idMarca, int? idTipoProducto, int pagina, int tamanio);
        Task<ProductosShop?> GetByIdAsync(int id);

        Task<ProductosShop> CreateAsync(ProductosShop producto);

        Task<bool> UpdateAsync(int id, ProductosShop datos);

        Task<bool> DeleteAsync(int id);
    }
}
