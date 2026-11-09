using Cine.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> BuscarClientes(string filtro);
        Task<Cliente?> ObtenerPorId(int? codCliente);
        Task Actualizar(Cliente cliente);
        Task<IEnumerable<Reserva>> ObtenerReservasPorClienteId(int codCliente);
        Task<bool> ExisteDni(string nroDoc, int codClienteExcluido);

    }
}
