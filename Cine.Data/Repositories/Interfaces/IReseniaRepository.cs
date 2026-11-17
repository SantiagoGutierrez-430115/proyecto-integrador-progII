using Cine.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Repositories.Interfaces
{
    public interface IReseniaRepository
    {
        Task<ReseniasPelicula> CreateAsync(ReseniasPelicula resenia);

        Task<bool> ExistePeliculaAsync(int idPelicula);

        Task<bool> ExistePuntuacionAsync(int puntuacion);

        Task<int?> ObtenerIdPuntuacionAsync(int puntuacion);

        Task<int?> ObtenerCodClientePorUsuarioAsync(int idUsuario);

        Task<bool> ExisteReseniaAsync(int codCliente, int idPelicula);

        Task<List<ReseniasPelicula>> ListarAsync();

        Task<ReseniasPelicula?> ObtenerPorIdAsync(int idResenia);

        Task<List<ReseniasPelicula>> ListarPorPeliculaAsync(int idPelicula);

        Task<List<ReseniasPelicula>> ListarPorClienteAsync(int codCliente);
    }
}
