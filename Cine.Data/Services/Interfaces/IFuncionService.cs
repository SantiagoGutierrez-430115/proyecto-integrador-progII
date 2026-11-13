using Cine.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Services.Interfaces
{
    public interface IFuncionService
    {
        Task<FuncionDto> CreateAsync(FuncionCreateDto dto);

        Task<List<FuncionDto>> ListarAsync();

        Task<FuncionDto> ObtenerPorIdAsync(int idFuncion);

        Task<List<FuncionDto>> ListarPorPeliculaAsync(int idPelicula);

        Task<List<FuncionDto>> ListarPorFechaAsync(DateOnly fecha);
    }
}
