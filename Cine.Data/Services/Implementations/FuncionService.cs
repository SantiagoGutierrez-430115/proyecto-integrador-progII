using Cine.Data.DTOs;
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
        private readonly IFuncionRepository _funcionRepository;

        public FuncionService(IFuncionRepository funcionRepository)
        {
            _funcionRepository = funcionRepository;
        }

        public async Task<List<ButacaFuncionDto>?> ObtenerButacasPorFuncionAsync(int idFuncion)
        {
            var butacas = await _funcionRepository.ObtenerButacasPorFuncionAsync(idFuncion);

            if (butacas is null)
            {
                return null;
            }

            var resultado = new List<ButacaFuncionDto>();

            if (butacas.Count == 0)
            {
                return resultado;
            }

            var idSala = butacas[0].IdSala;
            var tarifa = await _funcionRepository.ObtenerTarifaSalaAsync(idSala);

            if (tarifa is null)
            {
                throw new InvalidOperationException($"Falta configurar la tarifa de la sala {idSala}.");
            }

            var adicionales = await _funcionRepository.ObtenerAdicionalesPorTipoButacaAsync();
            var idsOcupadas = await _funcionRepository.ObtenerIdsButacasOcupadasAsync(idFuncion);

            foreach (var butaca in butacas)
            {
                var adicional = adicionales.FirstOrDefault(
                    a => a.IdTipoButacas == butaca.IdTipoButacas);

                if (adicional is null)
                {
                    throw new InvalidOperationException(
                        $"Falta configurar el adicional del tipo de butaca {butaca.IdTipoButacas}.");
                }

                resultado.Add(new ButacaFuncionDto
                {
                    IdButaca = butaca.IdButaca,
                    NroButaca = butaca.NroButaca,
                    Precio = tarifa.Value + adicional.Adicional,
                    Disponible = butaca.IdEstadoButacaNavigation.EstadoButaca == "Disponible"
                        && !idsOcupadas.Contains(butaca.IdButaca)
                });
            }

            return resultado;
        }
    }
}
