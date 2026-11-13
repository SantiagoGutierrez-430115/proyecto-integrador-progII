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
    }
}
