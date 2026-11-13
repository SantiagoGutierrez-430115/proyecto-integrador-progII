using Cine.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Repositories.Interfaces
{
    public interface IFuncionRepository
    {
        Task<Funcione> CreateAsync(Funcione funcion);

        Task<bool> ExistePeliculaAsync(int idPelicula);

        Task<bool> EsPeliculaActivaAsync(int idPelicula);

        Task<bool> ExisteSalaAsync(int idSala);

        Task<bool> ExisteHorarioAsync(int idHorario);

        Task<bool> ExisteFuncionEnSalaAsync(int idSala, DateOnly fecha, int idHorario);


        Task<List<Funcione>> ListarAsync();

        Task<Funcione?> ObtenerPorIdAsync(int idFuncion);

        Task<List<Funcione>> ListarPorPeliculaAsync(int idPelicula);

        Task<List<Funcione>> ListarPorFechaAsync(DateOnly fecha);
    }
}
