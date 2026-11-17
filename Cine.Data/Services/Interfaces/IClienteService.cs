using Cine.Data.DTOs;
using Cine.Data.Respuesta;

namespace Cine.Data.Services.Interfaces
{
    public interface IClienteService
    {
        Task<OperationResult<IEnumerable<ClienteDto>>> BuscarClientes(string? filtro);
        Task<ClienteDto?> ObtenerClientePorId(int codCliente);
        Task<OperationResult> ActualizarCliente(int codCliente, ActualizarClienteDto dto);
        Task<OperationResult<IEnumerable<ReservaClienteDto>>> ObtenerReservasDeCliente(int codCliente);
    }
}
