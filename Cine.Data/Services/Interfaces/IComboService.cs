using Cine.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Services.Interfaces
{
    public interface IComboService
    {
        Task<ResultadoPaginadoDto<ComboDto>> GetAllAsync(string? descripcion, int? idTipoCombo, int pagina, int tamanio);

        // Devuelve null si el combo no existe
        Task<ComboDto?> GetByIdAsync(int id);

        // Lanza DatosInvalidosException si algo está mal (producto inexistente, precio mayor a la suma de productos)
        Task<ComboDto> CreateAsync(GuardarComboDto dto);

        // Devuelve false si el combo no existe. Puede lanzar DatosInvalidosException
        Task<bool> UpdateAsync(int id, GuardarComboDto dto);

        // Borra el combo con sus productos. Devuelve false si no existe. Lanza ConflictoException si está en uso
        Task<bool> DeleteAsync(int id);
    }
}
