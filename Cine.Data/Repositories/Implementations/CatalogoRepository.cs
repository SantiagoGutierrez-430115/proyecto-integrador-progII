using Cine.Data.DTOs;
using Cine.Data.Models;
using Cine.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cine.Data.Repositories.Implementations
{
    // Listas id + nombre para los desplegables del front (solo lectura)
    public class CatalogoRepository : ICatalogoRepository
    {
        private readonly CineDbContext _context;

        public CatalogoRepository(CineDbContext context)
        {
            _context = context;
        }

        public Task<List<ItemDto>> GetPaisesAsync() =>
            _context.Paises.AsNoTracking().OrderBy(x => x.Pais)
                .Select(x => new ItemDto { Id = x.IdPais, Nombre = x.Pais }).ToListAsync();

        public Task<List<ItemDto>> GetFormatosAsync() =>
            _context.FormatosPeliculas.AsNoTracking().OrderBy(x => x.Formato)
                .Select(x => new ItemDto { Id = x.IdFormatoPelicula, Nombre = x.Formato }).ToListAsync();

        public Task<List<ItemDto>> GetClasificacionesAsync() =>
            _context.Clasificaciones.AsNoTracking().OrderBy(x => x.IdClasificacion)
                .Select(x => new ItemDto { Id = x.IdClasificacion, Nombre = x.Clasificacion }).ToListAsync();

        public Task<List<ItemDto>> GetDistribuidoresAsync() =>
            _context.Distribuidores.AsNoTracking().OrderBy(x => x.Distribuidor)
                .Select(x => new ItemDto { Id = x.IdDistribuidor, Nombre = x.Distribuidor }).ToListAsync();

        public Task<List<ItemDto>> GetDirectoresAsync() =>
            _context.Directores.AsNoTracking().OrderBy(x => x.Apellido).ThenBy(x => x.Nombre)
                .Select(x => new ItemDto { Id = x.IdDirector, Nombre = x.Nombre + " " + x.Apellido }).ToListAsync();

        public Task<List<ItemDto>> GetGenerosAsync() =>
            _context.Generos.AsNoTracking().OrderBy(x => x.Genero1)
                .Select(x => new ItemDto { Id = x.IdGenero, Nombre = x.Genero1 }).ToListAsync();

        public Task<List<ItemDto>> GetIdiomasAsync() =>
            _context.Idiomas.AsNoTracking().OrderBy(x => x.Idioma1)
                .Select(x => new ItemDto { Id = x.IdIdioma, Nombre = x.Idioma1 }).ToListAsync();

        public Task<List<ItemDto>> GetActoresAsync() =>
            _context.Actores.AsNoTracking().OrderBy(x => x.Apellido).ThenBy(x => x.Nombre)
                .Select(x => new ItemDto { Id = x.IdActor, Nombre = x.Nombre + " " + x.Apellido }).ToListAsync();
    }
}
