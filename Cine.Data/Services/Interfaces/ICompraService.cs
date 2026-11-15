using System.Threading.Tasks;
using Cine.Data.DTOs;

namespace Cine.Data.Services.Interfaces
{
    public interface ICompraService
    {
        Task<(int NroFactura, decimal Total)> CrearCompraAsync(
            int idUsuario, CrearCompraDto dto);
    }
}

