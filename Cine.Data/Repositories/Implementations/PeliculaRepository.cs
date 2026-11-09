using Cine.Data.Models;
using Cine.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cine.Data.Repositories.Implementations
{
    public class PeliculaRepository : IPeliculaRepository
    {
        private readonly CineDbContext _context;

        public PeliculaRepository(CineDbContext context)
        {
            _context = context;
        }

        private IQueryable<Pelicula> ConDetalle() =>
            _context.Peliculas
                .Include(p => p.IdPaisNavigation)
                .Include(p => p.IdFormatoPeliculaNavigation)
                .Include(p => p.IdClasificacionNavigation)
                .Include(p => p.IdDistribuidorNavigation)
                .Include(p => p.IdDirectorNavigation)
                .Include(p => p.PeliculasGeneros).ThenInclude(x => x.IdGeneroNavigation)
                .Include(p => p.PeliculasIdiomas).ThenInclude(x => x.IdIdiomaNavigation)
                .Include(p => p.PeliculasActores).ThenInclude(x => x.IdActorNavigation)
                .AsSplitQuery();

        public async Task<(List<Pelicula> Items, int Total)> GetAllAsync(
            string? titulo, int? idGenero, int? idClasificacion, int pagina, int tamanio)
        {
            var query = _context.Peliculas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(titulo))
                query = query.Where(p => p.Titulo.Contains(titulo));
            if (idGenero.HasValue)
                query = query.Where(p => p.PeliculasGeneros.Any(g => g.IdGenero == idGenero.Value));
            if (idClasificacion.HasValue)
                query = query.Where(p => p.IdClasificacion == idClasificacion.Value);

            var total = await query.CountAsync();

            var ids = await query
                .OrderBy(p => p.Titulo).ThenBy(p => p.IdPelicula)
                .Skip((pagina - 1) * tamanio)
                .Take(tamanio)
                .Select(p => p.IdPelicula)
                .ToListAsync();

            var items = await ConDetalle()
                .Where(p => ids.Contains(p.IdPelicula))
                .OrderBy(p => p.Titulo).ThenBy(p => p.IdPelicula)
                .ToListAsync();

            return (items, total);
        }

        public Task<Pelicula?> GetByIdAsync(int id) =>
            ConDetalle().FirstOrDefaultAsync(p => p.IdPelicula == id);

        public Task<bool> ExisteTituloAsync(string titulo, DateOnly fechaLanzamiento, int? excluirId = null) =>
            _context.Peliculas.AnyAsync(p =>
                p.Titulo == titulo &&
                p.FechaLanzamiento == fechaLanzamiento &&
                (excluirId == null || p.IdPelicula != excluirId));

        public async Task<bool> TieneDependenciasAsync(int id) =>
            await _context.Funciones.AnyAsync(f => f.IdPelicula == id) ||
            await _context.ReseniasPeliculas.AnyAsync(r => r.IdPelicula == id);

        public async Task<string?> ValidarReferenciasAsync(
            Pelicula p, IEnumerable<int> generos, IEnumerable<int> idiomas, IEnumerable<int> actores)
        {
            if (p.IdPais.HasValue && !await _context.Paises.AnyAsync(x => x.IdPais == p.IdPais))
                return "El país indicado no existe.";
            if (p.IdFormatoPelicula.HasValue && !await _context.FormatosPeliculas.AnyAsync(x => x.IdFormatoPelicula == p.IdFormatoPelicula))
                return "El formato indicado no existe.";
            if (p.IdClasificacion.HasValue && !await _context.Clasificaciones.AnyAsync(x => x.IdClasificacion == p.IdClasificacion))
                return "La clasificación indicada no existe.";
            if (p.IdDistribuidor.HasValue && !await _context.Distribuidores.AnyAsync(x => x.IdDistribuidor == p.IdDistribuidor))
                return "El distribuidor indicado no existe.";
            if (p.IdDirector.HasValue && !await _context.Directores.AnyAsync(x => x.IdDirector == p.IdDirector))
                return "El director indicado no existe.";

            var g = generos.Distinct().ToList();
            if (g.Count > 0 && await _context.Generos.CountAsync(x => g.Contains(x.IdGenero)) != g.Count)
                return "Alguno de los géneros indicados no existe.";

            var i = idiomas.Distinct().ToList();
            if (i.Count > 0 && await _context.Idiomas.CountAsync(x => i.Contains(x.IdIdioma)) != i.Count)
                return "Alguno de los idiomas indicados no existe.";

            var a = actores.Distinct().ToList();
            if (a.Count > 0 && await _context.Actores.CountAsync(x => a.Contains(x.IdActor)) != a.Count)
                return "Alguno de los actores indicados no existe.";

            return null;
        }

        public async Task<Pelicula> CreateAsync(
            Pelicula pelicula, IEnumerable<int> generos, IEnumerable<int> idiomas, IEnumerable<int> actores)
        {
            foreach (var id in generos.Distinct())
                pelicula.PeliculasGeneros.Add(new PeliculasGenero { IdGenero = id });
            foreach (var id in idiomas.Distinct())
                pelicula.PeliculasIdiomas.Add(new PeliculasIdioma { IdIdioma = id });
            foreach (var id in actores.Distinct())
                pelicula.PeliculasActores.Add(new PeliculasActore { IdActor = id });

            // Un solo SaveChanges: EF lo ejecuta en una transacción implícita
            _context.Peliculas.Add(pelicula);
            await _context.SaveChangesAsync();
            return pelicula;
        }

        public async Task<bool> UpdateAsync(
            int id, Pelicula datos, IEnumerable<int> generos, IEnumerable<int> idiomas, IEnumerable<int> actores)
        {
            var pelicula = await _context.Peliculas
                .Include(p => p.PeliculasGeneros)
                .Include(p => p.PeliculasIdiomas)
                .Include(p => p.PeliculasActores)
                .FirstOrDefaultAsync(p => p.IdPelicula == id);

            if (pelicula == null) return false;

            pelicula.Titulo = datos.Titulo;
            pelicula.Descripcion = datos.Descripcion;
            pelicula.FechaLanzamiento = datos.FechaLanzamiento;
            pelicula.IdPais = datos.IdPais;
            pelicula.IdFormatoPelicula = datos.IdFormatoPelicula;
            pelicula.IdClasificacion = datos.IdClasificacion;
            pelicula.IdDistribuidor = datos.IdDistribuidor;
            pelicula.IdDirector = datos.IdDirector;

            // Reemplaza las relaciones N:M
            _context.PeliculasGeneros.RemoveRange(pelicula.PeliculasGeneros);
            _context.PeliculasIdiomas.RemoveRange(pelicula.PeliculasIdiomas);
            _context.PeliculasActores.RemoveRange(pelicula.PeliculasActores);

            foreach (var gid in generos.Distinct())
                _context.PeliculasGeneros.Add(new PeliculasGenero { IdPelicula = id, IdGenero = gid });
            foreach (var iid in idiomas.Distinct())
                _context.PeliculasIdiomas.Add(new PeliculasIdioma { IdPelicula = id, IdIdioma = iid });
            foreach (var aid in actores.Distinct())
                _context.PeliculasActores.Add(new PeliculasActore { IdPelicula = id, IdActor = aid });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var pelicula = await _context.Peliculas
                .Include(p => p.PeliculasGeneros)
                .Include(p => p.PeliculasIdiomas)
                .Include(p => p.PeliculasActores)
                .FirstOrDefaultAsync(p => p.IdPelicula == id);

            if (pelicula == null) return false;

            _context.PeliculasGeneros.RemoveRange(pelicula.PeliculasGeneros);
            _context.PeliculasIdiomas.RemoveRange(pelicula.PeliculasIdiomas);
            _context.PeliculasActores.RemoveRange(pelicula.PeliculasActores);
            _context.Peliculas.Remove(pelicula);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
