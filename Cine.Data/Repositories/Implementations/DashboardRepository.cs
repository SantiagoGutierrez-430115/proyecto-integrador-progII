using Cine.Data.DTOs;
using Cine.Data.Models;
using Cine.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cine.Data.Repositories.Implementations
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly CineDbContext _context;

        public DashboardRepository(CineDbContext context)
        {
            _context = context;
        }

        public Task<List<PeliculaMasVendidaDto>> GetPeliculasMasVendidasAsync(DateTime? desde, DateTime? hasta, int top) =>
            _context.Database.SqlQuery<PeliculaMasVendidaDto>(
                $"EXEC dbo.SP_INDICADOR_PELICULAS_MAS_VENDIDAS @fecha_desde = {desde}, @fecha_hasta = {hasta}, @top = {top}")
                .ToListAsync();

        public Task<List<HorarioPopularDto>> GetHorariosMasPopularesAsync(DateTime? desde, DateTime? hasta) =>
            _context.Database.SqlQuery<HorarioPopularDto>(
                $"EXEC dbo.SP_INDICADOR_HORARIOS_MAS_POPULARES @fecha_desde = {desde}, @fecha_hasta = {hasta}")
                .ToListAsync();

        public Task<List<FormaPagoUsadaDto>> GetFormasPagoMasUsadasAsync(DateTime? desde, DateTime? hasta) =>
            _context.Database.SqlQuery<FormaPagoUsadaDto>(
                $"EXEC dbo.SP_INDICADOR_FORMAS_PAGO_MAS_USADAS @fecha_desde = {desde}, @fecha_hasta = {hasta}")
                .ToListAsync();
    }
}
