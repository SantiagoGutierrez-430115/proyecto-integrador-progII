using Cine.Data.DTOs;

namespace Cine.Data.Services.Interfaces
{
    public interface IPeliculaService
    {
        Task<ResultadoPaginadoDto<PeliculaDto>> GetAllAsync(string? titulo, int? idGenero, int? idClasificacion, bool incluirInactivas, int pagina, int tamanio);

        Task<PeliculaDto?> GetByIdAsync(int id, bool incluirInactivas);

        Task<PeliculaDto> CreateAsync(GuardarPeliculaDto dto);

        Task<bool> UpdateAsync(int id, GuardarPeliculaDto dto);

        Task<bool> DarDeBajaAsync(int id);

        Task<bool> ReactivarAsync(int id);

        Task<(bool Existe, string? PortadaAnterior)> ActualizarPortadaAsync(int id, string? urlPortada);
    }
}
