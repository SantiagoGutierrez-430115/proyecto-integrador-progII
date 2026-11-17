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
    public class ReseniaService : IReseniaService
    {
        private readonly IReseniaRepository repo;

        public ReseniaService(IReseniaRepository _repo)
        {
            this.repo = _repo;
        }

        public async Task<ReseniaDto> CreateAsync(ReseniaCreateDto dto, int idUsuario)
        {
            if (!await repo.ExistePeliculaAsync(dto.IdPelicula.Value))
            {
                throw new Exception("La película indicada no existe.");
            }

            int? codCliente = await repo.ObtenerCodClientePorUsuarioAsync(idUsuario);

            if (codCliente == null)
            {
                throw new Exception("El usuario autenticado no tiene un cliente asociado.");
            }

            if (!await repo.ExistePuntuacionAsync(dto.Puntuacion.Value))
            {
                throw new Exception("La puntuación indicada no es válida.");
            }

            if (await repo.ExisteReseniaAsync(codCliente.Value, dto.IdPelicula.Value))
            {
                throw new Exception("Ya has realizado una reseña para esta película.");
            }

            int? idPuntuacion = await repo.ObtenerIdPuntuacionAsync(dto.Puntuacion.Value);

            if (idPuntuacion == null)
            {
                throw new Exception("No se encuentra registrada la puntuación indicada.");
            }

            var resenia = new ReseniasPelicula
            {
                Comentario = dto.Comentario,
                FechaResenia = DateTime.Now,
                IdPelicula = dto.IdPelicula.Value,
                IdPuntuacion = idPuntuacion.Value,
                CodCliente = codCliente.Value
            };

            var reseniaCreada = await repo.CreateAsync(resenia);

            return new ReseniaDto
            {
                IdResenia = reseniaCreada.IdResenia,
                Comentario = reseniaCreada.Comentario,
                FechaResenia = reseniaCreada.FechaResenia,
                IdPelicula = reseniaCreada.IdPelicula,
                Puntuacion = dto.Puntuacion.Value,
                CodCliente = reseniaCreada.CodCliente
            };
        }

        public async Task<List<ReseniaDto>> ListarAsync()
        {
            var resenias = await repo.ListarAsync();

            return resenias.Select(r => new ReseniaDto
            {
                IdResenia = r.IdResenia,
                Comentario = r.Comentario,
                FechaResenia = r.FechaResenia,
                IdPelicula = r.IdPelicula,
                Puntuacion = r.IdPuntuacionNavigation.Puntuacion,
                CodCliente = r.CodCliente
            }).ToList();
        }

        public async Task<ReseniaDto> ObtenerPorIdAsync(int idResenia)
        {
            var resenia = await repo.ObtenerPorIdAsync(idResenia);

            if (resenia == null)
            {
                throw new Exception("La reseña solicitada no existe.");
            }

            return new ReseniaDto
            {
                IdResenia = resenia.IdResenia,
                Comentario = resenia.Comentario,
                FechaResenia = resenia.FechaResenia,
                IdPelicula = resenia.IdPelicula,
                Puntuacion = resenia.IdPuntuacionNavigation.Puntuacion,
                CodCliente = resenia.CodCliente
            };
        }

        public async Task<List<ReseniaDto>> ListarPorPeliculaAsync(int idPelicula)
        {
            var resenias = await repo.ListarPorPeliculaAsync(idPelicula);

            return resenias.Select(r => new ReseniaDto
            {
                IdResenia = r.IdResenia,
                Comentario = r.Comentario,
                FechaResenia = r.FechaResenia,
                IdPelicula = r.IdPelicula,
                Puntuacion = r.IdPuntuacionNavigation.Puntuacion,
                CodCliente = r.CodCliente
            }).ToList();
        }

        public async Task<List<ReseniaDto>> ListarPorClienteAsync(int codCliente)
        {
            var resenias = await repo.ListarPorClienteAsync(codCliente);

            return resenias.Select(r => new ReseniaDto
            {
                IdResenia = r.IdResenia,
                Comentario = r.Comentario,
                FechaResenia = r.FechaResenia,
                IdPelicula = r.IdPelicula,
                Puntuacion = r.IdPuntuacionNavigation.Puntuacion,
                CodCliente = r.CodCliente
            }).ToList();
        }
    }
}
