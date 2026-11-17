using System.Collections.Generic;
using System.Threading.Tasks;
using Cine.Data.DTOs;
using Cine.Data.Models;

namespace Cine.Data.Repositories.Interfaces
{
    public interface ICompraRepository
    {
        Task<int?> ObtenerCodClientePorUsuarioAsync(int idUsuario);

        Task<Funcione?> ObtenerFuncionAsync(int idFuncion);

        Task<(int NroFactura, decimal Total)> RegistrarCompraAsync(int codCliente, int idSucursal, int idCajero, int idFuncion, List<ButacaFuncionDto> butacasSeleccionadas);

        Task<int?> ObtenerIdCajeroPorSucursalAsync(int idSucursal);

    }
}
