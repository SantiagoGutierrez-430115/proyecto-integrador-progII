using Cine.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Services.Interfaces
{
    public interface IProductoShopService
    {
            Task<ResultadoPaginadoDto<ProductoShopDto>> GetAllAsync(string? nombre, int? idMarca, int? idTipoProducto, int pagina, int tamanio);

            Task<ProductoShopDto?> GetByIdAsync(int id);

            Task<ProductoShopDto> CreateAsync(GuardarProductoShopDto dto);

            Task<bool> UpdateAsync(int id, GuardarProductoShopDto dto);

            Task<bool> DeleteAsync(int id);
        
    }
}
