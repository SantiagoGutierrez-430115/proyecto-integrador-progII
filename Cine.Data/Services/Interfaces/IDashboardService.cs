using Cine.Data.DTOs;

namespace Cine.Data.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<List<PeliculaMasVendidaDto>> GetPeliculasMasVendidasAsync(DateTime? desde, DateTime? hasta, int top);
        Task<List<HorarioPopularDto>> GetHorariosMasPopularesAsync(DateTime? desde, DateTime? hasta);
        Task<List<FormaPagoUsadaDto>> GetFormasPagoMasUsadasAsync(DateTime? desde, DateTime? hasta);
    }
}
