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
    public class ButacaService : IButacaService
    {
        private readonly IButacaRepository repo;
        public ButacaService(IButacaRepository _repo)
        {
            this.repo = _repo;
        }

        public async Task<ButacaDto> CreateAsync(ButacaCreateDto dto)
        {
            if (!await repo.ExisteSalaAsync(dto.IdSala.Value))
            {
                throw new Exception("La sala indicada no existe.");
            }

            int nroButaca = await repo.ObtenerSiguienteNumeroAsync(dto.IdSala.Value);

            if (!await repo.ExisteTipoButacaAsync(dto.IdTipoButaca.Value))
            {
                throw new Exception("El tipo de butaca indicado no existe.");
            }

            var idEstadoDisponible = await repo.ObtenerIdEstadoAsync();
            if (idEstadoDisponible == null)
            {
                throw new Exception("No se encuentra registrado el estado Disponible.");
            }
            var butaca = new Butaca
            {

                IdSala = dto.IdSala.Value,
                NroButaca = nroButaca,
                IdTipoButacas = dto.IdTipoButaca.Value,
                IdEstadoButaca = idEstadoDisponible.Value
            };

            var butacacreada = await repo.CreateAsync(butaca);

            return new ButacaDto
            {
                IdButaca = butacacreada.IdButaca,
                IdSala = butacacreada.IdSala,
                NroButaca = butacacreada.NroButaca,
                IdTipoButaca = butacacreada.IdTipoButacas,
                IdEstadoButaca = butacacreada.IdEstadoButaca
            };


        }

        public async Task<List<ButacaDto>> ListarAsync()
        {
            var butacas = await repo.ListarAsync();

            return butacas.Select(b => new ButacaDto
            {
                IdButaca = b.IdButaca,
                IdSala = b.IdSala,
                NroButaca = b.NroButaca,
                IdTipoButaca = b.IdTipoButacas,
                IdEstadoButaca = b.IdEstadoButaca
            }).ToList();
        }

        public async Task<ButacaDto> ObtenerPorIdAsync(int idButaca)
        {
            var butaca = await repo.ObtenerPorIdAsync(idButaca);

            if (butaca == null)
                throw new Exception("La butaca solicitada no existe.");

            return new ButacaDto
            {
                IdButaca = butaca.IdButaca,
                IdSala = butaca.IdSala,
                NroButaca = butaca.NroButaca,
                IdTipoButaca = butaca.IdTipoButacas,
                IdEstadoButaca = butaca.IdEstadoButaca
            };
        }

        public async Task<List<ButacaDto>> ListarPorSalaAsync(int idSala)
        {
            var butacas = await repo.ListarPorSalaAsync(idSala);

            return butacas.Select(b => new ButacaDto
            {
                IdButaca = b.IdButaca,
                IdSala = b.IdSala,
                NroButaca = b.NroButaca,
                IdTipoButaca = b.IdTipoButacas,
                IdEstadoButaca = b.IdEstadoButaca
            }).ToList();
        }

        public async Task<List<ButacaDto>> ListarDisponiblesPorSalaAsync(int idSala)
        {
            var butacas = await repo.ListarDisponiblesPorSalaAsync(idSala);

            return butacas.Select(b => new ButacaDto
            {
                IdButaca = b.IdButaca,
                IdSala = b.IdSala,
                NroButaca = b.NroButaca,
                IdTipoButaca = b.IdTipoButacas,
                IdEstadoButaca = b.IdEstadoButaca
            }).ToList();
        }


    }
}
