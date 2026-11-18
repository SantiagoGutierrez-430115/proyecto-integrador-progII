using Cine.Data.DTOs;
using Cine.Data.Exceptions;
using Cine.Data.Repositories.Interfaces;
using Cine.Data.Services.Interfaces;

namespace Cine.Data.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;

        public DashboardService(IDashboardRepository repo)
        {
            _repo = repo;
        }

        public Task<List<PeliculaMasVendidaDto>> GetPeliculasMasVendidasAsync(DateTime? desde, DateTime? hasta, int top)
        {
            ValidarFechas(desde, hasta);

            // El top tiene que estar entre 1 y 50
            if (top < 1) top = 1;
            if (top > 50) top = 50;

            return _repo.GetPeliculasMasVendidasAsync(desde, hasta, top);
        }

        public Task<List<HorarioPopularDto>> GetHorariosMasPopularesAsync(DateTime? desde, DateTime? hasta)
        {
            ValidarFechas(desde, hasta);
            return _repo.GetHorariosMasPopularesAsync(desde, hasta);
        }

        public Task<List<FormaPagoUsadaDto>> GetFormasPagoMasUsadasAsync(DateTime? desde, DateTime? hasta)
        {
            ValidarFechas(desde, hasta);
            return _repo.GetFormasPagoMasUsadasAsync(desde, hasta);
        }

        // Si mandan las dos fechas, "desde" no puede ser posterior a "hasta"
        private static void ValidarFechas(DateTime? desde, DateTime? hasta)
        {
            if (desde.HasValue && hasta.HasValue && desde.Value.Date > hasta.Value.Date)
            {
                throw new DatosInvalidosException("La fecha 'desde' no puede ser posterior a la fecha 'hasta'.");
            }
        }
    }
}
