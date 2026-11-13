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
    public class ReseniaRepository : IReseniaRepository
    {
        private readonly CineDbContext db;

        public ReseniaRepository(CineDbContext _db)
        {
            this.db = _db;
        }

        public async Task<ReseniasPelicula> CreateAsync(ReseniasPelicula resenia)
        {
            await db.Set<ReseniasPelicula>().AddAsync(resenia);
            await db.SaveChangesAsync();

            return resenia;
        }

        public async Task<bool> ExistePeliculaAsync(int idPelicula)
        {
            return await db.Set<Pelicula>()
                .AnyAsync(p => p.IdPelicula == idPelicula);
        }

        public async Task<bool> ExistePuntuacionAsync(int puntuacion)
        {
            return await db.Set<Puntuacione>()
                .AnyAsync(p => p.Puntuacion == puntuacion);
        }

        public async Task<int?> ObtenerIdPuntuacionAsync(int puntuacion)
        {
            return await db.Set<Puntuacione>()
                .Where(p => p.Puntuacion == puntuacion)
                .Select(p => (int?)p.IdPuntuacion)
                .FirstOrDefaultAsync();
        }

        public async Task<int?> ObtenerCodClientePorUsuarioAsync(int idUsuario)
        {
            return await db.Set<Cliente>()
                .Where(c => c.IdUsuario == idUsuario)
                .Select(c => (int?)c.CodCliente)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> ExisteReseniaAsync(int codCliente, int idPelicula)
        {
            return await db.Set<ReseniasPelicula>()
                .AnyAsync(r => r.CodCliente == codCliente &&
                               r.IdPelicula == idPelicula);
        }

        public async Task<List<ReseniasPelicula>> ListarAsync()
        {
            return await db.Set<ReseniasPelicula>()
                .AsNoTracking()
                .Include(r => r.IdPuntuacionNavigation)
                .ToListAsync();
        }

        public async Task<ReseniasPelicula?> ObtenerPorIdAsync(int idResenia)
        {
            return await db.Set<ReseniasPelicula>()
                .AsNoTracking()
                .Include(r => r.IdPuntuacionNavigation)
                .FirstOrDefaultAsync(r => r.IdResenia == idResenia);
        }

        public async Task<List<ReseniasPelicula>> ListarPorPeliculaAsync(int idPelicula)
        {
            return await db.Set<ReseniasPelicula>()
                .AsNoTracking()
                .Include(r => r.IdPuntuacionNavigation)
                .Where(r => r.IdPelicula == idPelicula)
                .ToListAsync();
        }

        public async Task<List<ReseniasPelicula>> ListarPorClienteAsync(int codCliente)
        {
            return await db.Set<ReseniasPelicula>()
                .AsNoTracking()
                .Include(r => r.IdPuntuacionNavigation)
                .Where(r => r.CodCliente == codCliente)
                .ToListAsync();
        }
    }
}
