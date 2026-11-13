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
    public class ButacaRepository : IButacaRepository
    {
        private readonly CineDbContext db;
        public ButacaRepository(CineDbContext _db)
        {
            this.db = _db;
        }

        public async Task<Butaca> CreateAsync(Butaca b)
        {

            await db.Set<Butaca>().AddAsync(b);
            await db.SaveChangesAsync();
            return b;
        }

        public async Task<bool> ExisteButacaEnSalaAsync(int idSala, int nroButaca)
        {
            return await db.Set<Butaca>()
                .AnyAsync(b => b.IdSala == idSala &&
                b.NroButaca == nroButaca);
        }



        public async Task<bool> ExisteSalaAsync(int idSala)
        {
            return await db.Set<Sala>().AnyAsync(s => s.IdSala == idSala);
        }

        public async Task<bool> ExisteTipoButacaAsync(int idTipoButaca)
        {
            return await db.Set<TiposButaca>()
                .AnyAsync(t => t.IdTipoButacas == idTipoButaca);
        }

        public async Task<int?> ObtenerIdEstadoAsync()
        {
            return await db.Set<EstadosButaca>()
                .Where(e => e.EstadoButaca == "Disponible")
                .Select(e => (int?)e.IdEstadoButaca)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Butaca>> ListarAsync()
        {
            return await db.Set<Butaca>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Butaca?> ObtenerPorIdAsync(int idButaca)
        {
            return await db.Set<Butaca>()
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.IdButaca == idButaca);
        }

        public async Task<List<Butaca>> ListarPorSalaAsync(int idSala)
        {
            return await db.Set<Butaca>()
                .AsNoTracking()
                .Where(b => b.IdSala == idSala)
                .ToListAsync();
        }

        public async Task<List<Butaca>> ListarDisponiblesPorSalaAsync(int idSala)
        {
            return await db.Set<Butaca>()
                .AsNoTracking()
                .Where(b => b.IdSala == idSala &&
                            b.IdEstadoButacaNavigation.EstadoButaca == "Disponible")
                .ToListAsync();
        }

        public async Task<int> ObtenerSiguienteNumeroAsync(int idSala)
        {
            int ultimoNumero = await db.Set<Butaca>()
        .Where(b => b.IdSala == idSala)
        .Select(b => (int?)b.NroButaca)
        .MaxAsync() ?? 0;

            return ultimoNumero + 1;
        }
    }
}
