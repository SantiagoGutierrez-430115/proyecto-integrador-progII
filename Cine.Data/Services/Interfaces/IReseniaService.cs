using Cine.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Services.Interfaces
{
    public interface IReseniaService
    {
        Task<ReseniaDto> CreateAsync(ReseniaCreateDto dto, int idUsuario);

        Task<List<ReseniaDto>> ListarAsync();

        Task<ReseniaDto> ObtenerPorIdAsync(int idResenia);

        Task<List<ReseniaDto>> ListarPorPeliculaAsync(int idPelicula);

        Task<List<ReseniaDto>> ListarPorClienteAsync(int codCliente);
    }
}
