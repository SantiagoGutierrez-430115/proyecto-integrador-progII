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
        Task<List<Butaca>?> ObtenerButacasPorFuncionAsync(int idFuncion);

        Task<List<int>> ObtenerIdsButacasOcupadasAsync(int idFuncion);

        Task<decimal?> ObtenerTarifaSalaAsync(int idSala);
        Task<List<AdicionalesButaca>> ObtenerAdicionalesPorTipoButacaAsync();
    }
}
