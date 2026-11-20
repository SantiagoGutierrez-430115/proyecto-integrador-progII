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

        Task<ComboDto?> GetByIdAsync(int id);

        Task<ComboDto> CreateAsync(GuardarComboDto dto);

        Task<bool> UpdateAsync(int id, GuardarComboDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
