using Cine.Data.DTOs;

namespace Cine.Data.Repositories.Interfaces
{
    public interface ICatalogoRepository
    {
        Task<List<ItemDto>> GetPaisesAsync();
        Task<List<ItemDto>> GetFormatosAsync();
        Task<List<ItemDto>> GetClasificacionesAsync();
        Task<List<ItemDto>> GetDistribuidoresAsync();
        Task<List<ItemDto>> GetDirectoresAsync();
        Task<List<ItemDto>> GetGenerosAsync();
        Task<List<ItemDto>> GetIdiomasAsync();
        Task<List<ItemDto>> GetActoresAsync();
    }
}
