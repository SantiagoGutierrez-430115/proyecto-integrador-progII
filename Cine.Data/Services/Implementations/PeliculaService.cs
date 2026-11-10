using Cine.Data.Constants;
using Cine.Data.DTOs;
using Cine.Data.Exceptions;
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
            string? titulo, int? idGenero, int? idClasificacion, bool incluirInactivas, int pagina, int tamanio)
        {
            // La página mínima es 1, y el tamaño tiene que estar entre 1 y 100
            if (pagina < 1) pagina = 1;
            if (tamanio < 1) tamanio = 1;
            if (tamanio > 100) tamanio = 100;

            var resultado = await _repo.GetAllAsync(titulo, idGenero, idClasificacion, incluirInactivas, pagina, tamanio);

            // Convierte cada Pelicula (entidad) en un PeliculaDto
            var dtos = new List<PeliculaDto>();
            foreach (var pelicula in resultado.Items)
            {
                dtos.Add(ToDto(pelicula));
            }

            return new ResultadoPaginadoDto<PeliculaDto>
            {
                Items = dtos,
                Total = resultado.Total,
                Pagina = pagina,
                Tamanio = tamanio
            };
        }

        public async Task<PeliculaDto?> GetByIdAsync(int id, bool incluirInactivas)
        {
            var pelicula = await _repo.GetByIdAsync(id);
            if (pelicula == null) return null;

            // Una película inactiva solo la ve quien lo pidió (el Admin)
            if (!incluirInactivas && pelicula.IdEstadoPeliculaNavigation.Estado != EstadosPeliculaNombres.Activa)
                return null;

            return ToDto(pelicula);
        }

        public async Task<PeliculaDto> CreateAsync(GuardarPeliculaDto dto)
        {
            var pelicula = ToEntity(dto);

            var error = await _repo.ValidarReferenciasAsync(pelicula, dto.IdsGeneros, dto.IdsIdiomas, dto.IdsActores);
            if (error != null)
                throw new DatosInvalidosException(error);

            if (await _repo.ExisteTituloAsync(pelicula.Titulo, pelicula.FechaLanzamiento))
                throw new ConflictoException("Ya existe una película con ese título y fecha de lanzamiento.");

            var creada = await _repo.CreateAsync(pelicula, dto.IdsGeneros, dto.IdsIdiomas, dto.IdsActores);

            // Se vuelve a leer para devolver la película con país, director, géneros, etc.
            var completa = await _repo.GetByIdAsync(creada.IdPelicula);
            return ToDto(completa!);
        }

        public async Task<bool> UpdateAsync(int id, GuardarPeliculaDto dto)
        {
            var existente = await _repo.GetByIdAsync(id);
            if (existente == null) return false;

            var datos = ToEntity(dto);

            var error = await _repo.ValidarReferenciasAsync(datos, dto.IdsGeneros, dto.IdsIdiomas, dto.IdsActores);
            if (error != null)
                throw new DatosInvalidosException(error);

            if (await _repo.ExisteTituloAsync(datos.Titulo, datos.FechaLanzamiento, id))
                throw new ConflictoException("Ya existe otra película con ese título y fecha de lanzamiento.");

            return await _repo.UpdateAsync(id, datos, dto.IdsGeneros, dto.IdsIdiomas, dto.IdsActores);
        }

        public async Task<bool> DarDeBajaAsync(int id)
        {
            return await _repo.CambiarEstadoAsync(id, EstadosPeliculaNombres.Inactiva);
        }

        public async Task<bool> ReactivarAsync(int id)
        {
            return await _repo.CambiarEstadoAsync(id, EstadosPeliculaNombres.Activa);
        }

        // Pasa los datos que llegan (DTO) a una entidad de la base
        private static Pelicula ToEntity(GuardarPeliculaDto dto)
        {
            return new Pelicula
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
        }

        // Pasa una entidad de la base a lo que se devuelve en la API (DTO)
        private static PeliculaDto ToDto(Pelicula p)
        {
            return new PeliculaDto
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
                Director = p.IdDirectorNavigation == null
                    ? null
                    : p.IdDirectorNavigation.Nombre + " " + p.IdDirectorNavigation.Apellido,
                IdEstadoPelicula = p.IdEstadoPelicula,
                Estado = p.IdEstadoPeliculaNavigation?.Estado,
                Generos = p.PeliculasGeneros
                    .Select(x => new ItemDto { Id = x.IdGenero, Nombre = x.IdGeneroNavigation.Genero1 })
                    .OrderBy(x => x.Nombre)
                    .ToList(),
                Idiomas = p.PeliculasIdiomas
                    .Select(x => new ItemDto { Id = x.IdIdioma, Nombre = x.IdIdiomaNavigation.Idioma1 })
                    .OrderBy(x => x.Nombre)
                    .ToList(),
                Actores = p.PeliculasActores
                    .Select(x => new ItemDto { Id = x.IdActor, Nombre = x.IdActorNavigation.Nombre + " " + x.IdActorNavigation.Apellido })
                    .OrderBy(x => x.Nombre)
                    .ToList()
            };
        }
    }
}
