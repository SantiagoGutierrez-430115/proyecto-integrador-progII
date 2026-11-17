using Cine.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Services.Interfaces
{
    public interface IButacaService
    {
        Task<ButacaDto> CreateAsync(ButacaCreateDto dto);

        Task<List<ButacaDto>> ListarAsync();

        Task<ButacaDto> ObtenerPorIdAsync(int idButaca);

        Task<List<ButacaDto>> ListarPorSalaAsync(int idSala);

        Task<List<ButacaDto>> ListarDisponiblesPorSalaAsync(int idSala);
    }
}
