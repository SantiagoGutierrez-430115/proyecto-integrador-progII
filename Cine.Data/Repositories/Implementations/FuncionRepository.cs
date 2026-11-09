using Cine.Data.Models;
using Cine.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Repositories.Implementations
{
    public class FuncionRepository : IFuncionRepository
    {
        private readonly CineDbContext _context;

        public FuncionRepository(CineDbContext context)
        {
            _context = context;
        }

        public async Task<List<AdicionalesButaca>> ObtenerAdicionalesPorTipoButacaAsync()
        {
            return await _context.AdicionalesButacas.ToListAsync();
        }

        public async Task<List<Butaca>?> ObtenerButacasPorFuncionAsync(int idFuncion)
        {

            //Primera consulta encontrar la sala.
            var idSala = await _context.Funciones
                .Where(funcion => funcion.IdFuncion == idFuncion)
                .Select(funcion => (int?)funcion.IdSala)
                .FirstOrDefaultAsync();

            if (idSala is null)
            {
                return null;
            }

            //Segunda consulta traer butacas.
            return await _context.Butacas
                .Include(butaca => butaca.IdEstadoButacaNavigation)
                .Where(butaca => butaca.IdSala == idSala.Value)
                .OrderBy(butaca => butaca.NroButaca)
                .ToListAsync();
        }

        public async Task<List<int>> ObtenerIdsButacasOcupadasAsync(int idFuncion)
        {
            //Buscamos entradas con el id de la Función y nos quedamos con el id  de cada Butaca.
            var vendidas = _context.Entradas
                .Where(entrada => entrada.IdFuncion == idFuncion)
                .Select(entrada => entrada.IdButaca);

            //Partimos de "ReservaDetalles" porque ahí está idButaca. Desde IdReservaNavigation llegamos a la Reserva, que tiene la función y el estado
            var reservadas = _context.ReservaDetalles
                .Where(detalle =>
                    detalle.IdReservaNavigation.IdFuncion == idFuncion &&
                    (detalle.IdReservaNavigation.IdEstadoReservaNavigation.Estado == "Pendiente" ||
                     detalle.IdReservaNavigation.IdEstadoReservaNavigation.Estado == "Confirmada"))
                .Select(detalle => detalle.IdButaca);

            //Juntamos los IDs sin repetirlos mediante Union.
            return await vendidas.Union(reservadas).ToListAsync();
        }

        public async Task<decimal?> ObtenerTarifaSalaAsync(int idSala)
        {
            //Buscamos el tipo de sala
            var idTipoSala = await _context.Salas
                .Where(sala => sala.IdSala == idSala)
                .Select(sala => (int?)sala.IdTipoSala)
                .FirstOrDefaultAsync();

            if (idTipoSala is null)
            {
                return null;
            }

            //Buscamos el precio para ese tipo.
            return await _context.TarifasEntradas
                .Where(tarifa => tarifa.IdTipoSala == idTipoSala.Value)
                .Select(tarifa => (decimal?)tarifa.Precio)
                .FirstOrDefaultAsync();
        }
    }
}
