using Cine.Data.DTOs;
using Cine.Data.Respuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDto>> BuscarClientes(string filtro);
        Task<ClienteDto> ObtenerClientePorId(int codCliente);
        Task<OperationResult> ActualizarCliente(int codCliente, ActualizarClienteDto dto);
        Task<IEnumerable<ReservaClienteDto>> ObtenerReservasDeCliente(int codCliente);
    }
}
