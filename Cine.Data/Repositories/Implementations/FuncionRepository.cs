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
        private readonly CineDbContext db;

        public FuncionRepository(CineDbContext _db)
        {
            this.db = _db;
        }

        public async Task<List<Funcione>> ListarAsync()
        {
            return await db.Set<Funcione>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Funcione?> ObtenerPorIdAsync(int idFuncion)
        {
            return await db.Set<Funcione>()
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.IdFuncion == idFuncion);
        }

        public async Task<List<Funcione>> ListarPorPeliculaAsync(int idPelicula)
        {
            return await db.Set<Funcione>()
                .AsNoTracking()
                .Where(f => f.IdPelicula == idPelicula)
                .ToListAsync();
        }

        public async Task<List<Funcione>> ListarPorFechaAsync(DateOnly fecha)
        {
            return await db.Set<Funcione>()
                .AsNoTracking()
                .Where(f => f.Fecha == fecha)
                .ToListAsync();
        }

        public async Task<Funcione> CreateAsync(Funcione funcion)
        {
            await db.Set<Funcione>().AddAsync(funcion);
            await db.SaveChangesAsync();

            return funcion;
        }

        public async Task<bool> EsPeliculaActivaAsync(int idPelicula)
        {
            return await db.Set<Pelicula>()
                .AnyAsync(p =>
                    p.IdPelicula == idPelicula &&
                    db.Set<EstadosPelicula>()
                        .Any(e =>
                            e.IdEstadoPelicula == p.IdEstadoPelicula &&
                            e.Estado == "Activa"));
        }

        public async Task<bool> ExisteFuncionEnSalaAsync(int idSala, DateOnly fecha, int idHorario)
        {
            return await db.Set<Funcione>()
                 .AnyAsync(f =>
                     f.IdSala == idSala &&
                     f.Fecha == fecha &&
                     f.IdHorario == idHorario);
        }

        public async Task<bool> ExisteHorarioAsync(int idHorario)
        {
            return await db.Set<Horario>()
               .AnyAsync(h => h.IdHorario == idHorario);
        }

        public async Task<bool> ExistePeliculaAsync(int idPelicula)
        {
            return await db.Set<Pelicula>()
                .AnyAsync(p => p.IdPelicula == idPelicula);
        }

        public async Task<bool> ExisteSalaAsync(int idSala)
        {
            return await db.Set<Sala>()
                .AnyAsync(s => s.IdSala == idSala);
        }

        public async Task<List<AdicionalesButaca>> ObtenerAdicionalesPorTipoButacaAsync()
        {
            return await db.AdicionalesButacas.ToListAsync();
        }

        public async Task<List<Butaca>?> ObtenerButacasPorFuncionAsync(int idFuncion)
        {

            //Primera consulta encontrar la sala.
            var idSala = await db.Funciones
                .Where(funcion => funcion.IdFuncion == idFuncion)
                .Select(funcion => (int?)funcion.IdSala)
                .FirstOrDefaultAsync();

            if (idSala is null)
            {
                return null;
            }

            //Segunda consulta traer butacas.
            return await db.Butacas
                .Include(butaca => butaca.IdEstadoButacaNavigation)
                .Where(butaca => butaca.IdSala == idSala.Value)
                .OrderBy(butaca => butaca.NroButaca)
                .ToListAsync();
        }

        public async Task<List<int>> ObtenerIdsButacasOcupadasAsync(int idFuncion)
        {
            //Buscamos entradas con el id de la Función y nos quedamos con el id  de cada Butaca.
            var vendidas = db.Entradas
                .Where(entrada => entrada.IdFuncion == idFuncion)
                .Select(entrada => entrada.IdButaca);

            //Partimos de "ReservaDetalles" porque ahí está idButaca. Desde IdReservaNavigation llegamos a la Reserva, que tiene la función y el estado
            var reservadas = db.ReservaDetalles
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
            var idTipoSala = await db.Salas
                .Where(sala => sala.IdSala == idSala)
                .Select(sala => (int?)sala.IdTipoSala)
                .FirstOrDefaultAsync();

            if (idTipoSala is null)
            {
                return null;
            }

            //Buscamos el precio para ese tipo.
            return await db.TarifasEntradas
                .Where(tarifa => tarifa.IdTipoSala == idTipoSala.Value)
                .Select(tarifa => (decimal?)tarifa.Precio)
                .FirstOrDefaultAsync();
        }
    }
}
