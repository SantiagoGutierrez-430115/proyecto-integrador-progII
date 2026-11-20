using Cine.Data.Models;

namespace Cine.Data.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> BuscarClientes(string? filtro);
        Task<Cliente?> ObtenerPorId(int codCliente);
        Task<bool> Existe(int codCliente);
        Task Actualizar(Cliente cliente);
        Task<IEnumerable<Reserva>> ObtenerReservasPorClienteId(int codCliente);
        Task<bool> ExisteDocumento(int idTipoDoc, string nroDoc, int codClienteExcluido);
        Task<bool> ExisteCorreo(string correo, int idUsuarioExcluido);
    }
}
