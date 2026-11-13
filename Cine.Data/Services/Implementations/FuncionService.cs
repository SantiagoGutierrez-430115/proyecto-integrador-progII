using Cine.Data.DTOs;
using Cine.Data.Models;
using Cine.Data.Repositories.Interfaces;
using Cine.Data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Services.Implementations
{
    public class FuncionService : IFuncionService
    {
        private readonly IFuncionRepository repo;

        public FuncionService(IFuncionRepository _repo)
        {
            this.repo = _repo;
        }
        public async Task<FuncionDto> CreateAsync(FuncionCreateDto dto)
        {
            if (!await repo.ExistePeliculaAsync(dto.IdPelicula.Value))
                throw new Exception("La película seleccionada no existe.");

            if (!await repo.EsPeliculaActivaAsync(dto.IdPelicula.Value))
                throw new Exception("No se pueden programar funciones de películas inactivas.");

            if (!await repo.ExisteSalaAsync(dto.IdSala.Value))
                throw new Exception("La sala seleccionada no existe.");

            if (!await repo.ExisteHorarioAsync(dto.IdHorario.Value))
                throw new Exception("El horario seleccionado no existe.");

            if (dto.Fecha.Value < DateOnly.FromDateTime(DateTime.Today))
                throw new Exception("No se pueden programar funciones en fechas pasadas.");

            if (await repo.ExisteFuncionEnSalaAsync(dto.IdSala.Value, dto.Fecha.Value, dto.IdHorario.Value))
            {
                throw new Exception("Ya existe una función programada en esa sala, fecha y horario.");
            }

            var funcion = new Funcione
            {
                IdPelicula = dto.IdPelicula.Value,
                IdSala = dto.IdSala.Value,
                Fecha = dto.Fecha.Value,
                IdHorario = dto.IdHorario.Value
            };

            var creada = await repo.CreateAsync(funcion);

            // Devolver DTO
            return new FuncionDto
            {
                IdFuncion = creada.IdFuncion,
                IdPelicula = creada.IdPelicula,
                IdSala = creada.IdSala,
                Fecha = creada.Fecha,
                IdHorario = creada.IdHorario
            };
        }


        public async Task<List<FuncionDto>> ListarAsync()
        {
            var funciones = await repo.ListarAsync();

            return funciones.Select(f => new FuncionDto
            {
                IdFuncion = f.IdFuncion,
                IdPelicula = f.IdPelicula,
                IdSala = f.IdSala,
                Fecha = f.Fecha,
                IdHorario = f.IdHorario
            }).ToList();
        }

        public async Task<FuncionDto> ObtenerPorIdAsync(int idFuncion)
        {
            var funcion = await repo.ObtenerPorIdAsync(idFuncion);

            if (funcion == null)
            {
                throw new Exception("La función solicitada no existe.");

            }

            return new FuncionDto
            {
                IdFuncion = funcion.IdFuncion,
                IdPelicula = funcion.IdPelicula,
                IdSala = funcion.IdSala,
                Fecha = funcion.Fecha,
                IdHorario = funcion.IdHorario
            };
        }

        public async Task<List<FuncionDto>> ListarPorPeliculaAsync(int idPelicula)
        {
            var funciones = await repo.ListarPorPeliculaAsync(idPelicula);

            return funciones.Select(f => new FuncionDto
            {
                IdFuncion = f.IdFuncion,
                IdPelicula = f.IdPelicula,
                IdSala = f.IdSala,
                Fecha = f.Fecha,
                IdHorario = f.IdHorario
            }).ToList();
        }

        public async Task<List<FuncionDto>> ListarPorFechaAsync(DateOnly fecha)
        {
            var funciones = await repo.ListarPorFechaAsync(fecha);

            return funciones.Select(f => new FuncionDto
            {
                IdFuncion = f.IdFuncion,
                IdPelicula = f.IdPelicula,
                IdSala = f.IdSala,
                Fecha = f.Fecha,
                IdHorario = f.IdHorario
            }).ToList();
        }


    }
}
