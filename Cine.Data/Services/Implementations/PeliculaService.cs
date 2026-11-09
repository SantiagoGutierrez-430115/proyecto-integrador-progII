using Cine.Data.DTOs;
using Cine.Data.Models;
using Cine.Data.Repositories.Interfaces;
using Cine.Data.Services.Interfaces;

namespace Cine.Data.Services.Implementations
{
    public class PeliculaService : IPeliculaService
    {
        private readonly IPeliculaRepository _repo;

        public PeliculaService(IPeliculaRepository repo)
        {
            _repo = repo;
        }

        public async Task<ResultadoPaginadoDto<PeliculaDto>> GetAllAsync(
            string? titulo, int? idGenero, int? idClasificacion, int pagina, int tamanio)
        {
            pagina = Math.Max(1, pagina);
            tamanio = Math.Clamp(tamanio, 1, 100);

            var (items, total) = await _repo.GetAllAsync(titulo, idGenero, idClasificacion, pagina, tamanio);

            return new ResultadoPaginadoDto<PeliculaDto>
            {
                Items = items.Select(ToDto).ToList(),
                Total = total,
                Pagina = pagina,
                Tamanio = tamanio
            };
        }

        public async Task<PeliculaDto?> GetByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            return p == null ? null : ToDto(p);
        }

        public async Task<(ResultadoPelicula, string?, PeliculaDto?)> CreateAsync(GuardarPeliculaDto dto)
        {
            var pelicula = ToEntity(dto);

            var error = await _repo.ValidarReferenciasAsync(pelicula, dto.IdsGeneros, dto.IdsIdiomas, dto.IdsActores);
            if (error != null) return (ResultadoPelicula.Invalida, error, null);

            if (await _repo.ExisteTituloAsync(pelicula.Titulo, pelicula.FechaLanzamiento))
                return (ResultadoPelicula.Duplicada, "Ya existe una película con ese título y fecha de lanzamiento.", null);

            var creada = await _repo.CreateAsync(pelicula, dto.IdsGeneros, dto.IdsIdiomas, dto.IdsActores);
            var completa = await _repo.GetByIdAsync(creada.IdPelicula);
            return (ResultadoPelicula.Ok, null, completa == null ? null : ToDto(completa));
        }

        public async Task<(ResultadoPelicula, string?)> UpdateAsync(int id, GuardarPeliculaDto dto)
        {
            if (await _repo.GetByIdAsync(id) == null)
                return (ResultadoPelicula.NoEncontrada, null);

            var datos = ToEntity(dto);

            var error = await _repo.ValidarReferenciasAsync(datos, dto.IdsGeneros, dto.IdsIdiomas, dto.IdsActores);
            if (error != null) return (ResultadoPelicula.Invalida, error);

            if (await _repo.ExisteTituloAsync(datos.Titulo, datos.FechaLanzamiento, id))
                return (ResultadoPelicula.Duplicada, "Ya existe otra película con ese título y fecha de lanzamiento.");

            var ok = await _repo.UpdateAsync(id, datos, dto.IdsGeneros, dto.IdsIdiomas, dto.IdsActores);
            return ok ? (ResultadoPelicula.Ok, null) : (ResultadoPelicula.NoEncontrada, null);
        }

        public async Task<(ResultadoPelicula, string?)> DeleteAsync(int id)
        {
            if (await _repo.GetByIdAsync(id) == null)
                return (ResultadoPelicula.NoEncontrada, null);

            if (await _repo.TieneDependenciasAsync(id))
                return (ResultadoPelicula.TieneDependencias,
                    "No se puede eliminar: la película tiene funciones o reseñas asociadas.");

            var ok = await _repo.DeleteAsync(id);
            return ok ? (ResultadoPelicula.Ok, null) : (ResultadoPelicula.NoEncontrada, null);
        }

        private static Pelicula ToEntity(GuardarPeliculaDto dto) => new()
        {
            Titulo = dto.Titulo.Trim(),
            Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion.Trim(),
            FechaLanzamiento = dto.FechaLanzamiento!.Value,
            IdPais = dto.IdPais,
            IdFormatoPelicula = dto.IdFormatoPelicula,
            IdClasificacion = dto.IdClasificacion,
            IdDistribuidor = dto.IdDistribuidor,
            IdDirector = dto.IdDirector
        };

        private static PeliculaDto ToDto(Pelicula p) => new()
        {
            IdPelicula = p.IdPelicula,
            Titulo = p.Titulo,
            Descripcion = p.Descripcion,
            FechaLanzamiento = p.FechaLanzamiento,
            IdPais = p.IdPais,
            Pais = p.IdPaisNavigation?.Pais,
            IdFormatoPelicula = p.IdFormatoPelicula,
            Formato = p.IdFormatoPeliculaNavigation?.Formato,
            IdClasificacion = p.IdClasificacion,
            Clasificacion = p.IdClasificacionNavigation?.Clasificacion,
            IdDistribuidor = p.IdDistribuidor,
            Distribuidor = p.IdDistribuidorNavigation?.Distribuidor,
            IdDirector = p.IdDirector,
            Director = p.IdDirectorNavigation == null ? null
                : $"{p.IdDirectorNavigation.Nombre} {p.IdDirectorNavigation.Apellido}",
            Generos = p.PeliculasGeneros
                .Select(x => new ItemDto { Id = x.IdGenero, Nombre = x.IdGeneroNavigation.Genero1 })
                .OrderBy(x => x.Nombre).ToList(),
            Idiomas = p.PeliculasIdiomas
                .Select(x => new ItemDto { Id = x.IdIdioma, Nombre = x.IdIdiomaNavigation.Idioma1 })
                .OrderBy(x => x.Nombre).ToList(),
            Actores = p.PeliculasActores
                .Select(x => new ItemDto { Id = x.IdActor, Nombre = $"{x.IdActorNavigation.Nombre} {x.IdActorNavigation.Apellido}" })
                .OrderBy(x => x.Nombre).ToList()
        };
    }
}
