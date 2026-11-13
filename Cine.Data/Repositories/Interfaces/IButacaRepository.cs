using Cine.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Repositories.Interfaces
{
    public interface IButacaRepository
    {
        Task<Butaca> CreateAsync(Butaca b);

        Task<bool> ExisteSalaAsync(int idSala);

        Task<bool> ExisteButacaEnSalaAsync(int idSala, int nroButaca);

        Task<bool> ExisteTipoButacaAsync(int idTipoButaca);

        Task<int?> ObtenerIdEstadoAsync();

        Task<List<Butaca>> ListarAsync();

        Task<Butaca?> ObtenerPorIdAsync(int idButaca);

        Task<List<Butaca>> ListarPorSalaAsync(int idSala);

        Task<List<Butaca>> ListarDisponiblesPorSalaAsync(int idSala);

        Task<int> ObtenerSiguienteNumeroAsync(int idSala);
    }
}
