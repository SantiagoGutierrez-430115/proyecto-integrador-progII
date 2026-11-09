using Cine.Data.DTOs;

namespace Cine.Data.Services.Interfaces
{
    public interface IPeliculaService
    {
        Task<ResultadoPaginadoDto<PeliculaDto>> GetAllAsync(string? titulo, int? idGenero, int? idClasificacion, int pagina, int tamanio);

        // Devuelve null si la película no existe
        Task<PeliculaDto?> GetByIdAsync(int id);

        // Lanza DatosInvalidosException o ConflictoException si algo está mal
        Task<PeliculaDto> CreateAsync(GuardarPeliculaDto dto);

        // Devuelve false si la película no existe. Puede lanzar DatosInvalidosException o ConflictoException
        Task<bool> UpdateAsync(int id, GuardarPeliculaDto dto);

        // Devuelve false si la película no existe. Lanza ConflictoException si tiene funciones o reseñas
        Task<bool> DeleteAsync(int id);
    }
}
