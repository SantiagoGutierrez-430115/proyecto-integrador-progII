using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cine.Data.DTOs;
using Cine.Data.Exceptions;
using Cine.Data.Repositories.Interfaces;
using Cine.Data.Services.Interfaces;

namespace Cine.Data.Services.Implementations
{
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _compraRepository;
        private readonly IFuncionService _funcionService;

        public CompraService(ICompraRepository compraRepository, IFuncionService funcionService)
        {
            _compraRepository = compraRepository;
            _funcionService = funcionService;
        }

        public async Task<(int NroFactura, decimal Total)> CrearCompraAsync(
            int idUsuario, CrearCompraDto dto)
        {
            if (dto.IdsButacas is null || dto.IdsButacas.Count == 0)
            {
                throw new DatosInvalidosException("Debés elegir al menos una butaca.");
            }

            if (dto.IdsButacas.Distinct().Count() != dto.IdsButacas.Count)
            {
                throw new DatosInvalidosException("La selección contiene butacas repetidas.");
            }

            var codCliente = await _compraRepository.ObtenerCodClientePorUsuarioAsync(idUsuario);
            if (codCliente is null)
            {
                throw new UnauthorizedAccessException("El usuario autenticado no tiene un cliente asociado.");
            }

            var funcion = await _compraRepository.ObtenerFuncionAsync(dto.IdFuncion);
            if (funcion is null)
            {
                throw new KeyNotFoundException("La función indicada no existe.");
            }

            var inicio = funcion.Fecha.ToDateTime(funcion.IdHorarioNavigation.Horario1);
            if (inicio <= DateTime.Now)
            {
                throw new ConflictoException("La función ya empezó.");
            }

            var butacasDeLaFuncion = await _funcionService.ObtenerButacasPorFuncionAsync(dto.IdFuncion);
            if (butacasDeLaFuncion is null)
            {
                throw new KeyNotFoundException("La función indicada no existe.");
            }

            var butacasSeleccionadas = new List<ButacaFuncionDto>();
            foreach (var idButaca in dto.IdsButacas)
            {
                var butaca = butacasDeLaFuncion.FirstOrDefault(b => b.IdButaca == idButaca);
                if (butaca is null)
                {
                    throw new DatosInvalidosException($"La butaca con ID {idButaca} no pertenece a la sala de la función.");
                }

                if (!butaca.Disponible)
                {
                    throw new ConflictoException($"La butaca {butaca.NroButaca} ya no está disponible.");
                }

                butacasSeleccionadas.Add(butaca);
            }

            var idSucursal = funcion.IdSalaNavigation.IdSucursal;
            var idCajero = await _compraRepository.ObtenerIdCajeroPorSucursalAsync(idSucursal);
            if (idCajero is null)
            {
                throw new InvalidOperationException("La sucursal de la función no tiene un cajero cargado.");
            }

            return await _compraRepository.RegistrarCompraAsync(
                codCliente.Value, idSucursal, idCajero.Value, funcion.IdFuncion, butacasSeleccionadas);
        }
    }
}
