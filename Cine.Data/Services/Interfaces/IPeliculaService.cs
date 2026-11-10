using Cine.Data.DTOs;

namespace Cine.Data.Services.Interfaces
{
    public interface IPeliculaService
    {
        Task<ResultadoPaginadoDto<PeliculaDto>> GetAllAsync(string? titulo, int? idGenero, int? idClasificacion, bool incluirInactivas, int pagina, int tamanio);

        // Devuelve null si la película no existe, o si está inactiva y no se pidió verlas
        Task<PeliculaDto?> GetByIdAsync(int id, bool incluirInactivas);

        // Lanza DatosInvalidosException o ConflictoException si algo está mal
        Task<PeliculaDto> CreateAsync(GuardarPeliculaDto dto);

        // Devuelve false si la película no existe. Puede lanzar DatosInvalidosException o ConflictoException
        Task<bool> UpdateAsync(int id, GuardarPeliculaDto dto);

        // Baja lógica: marca la película como Inactiva. Devuelve false si no existe
        Task<bool> DarDeBajaAsync(int id);

        // Vuelve a marcar la película como Activa. Devuelve false si no existe
        Task<bool> ReactivarAsync(int id);
    }
}
