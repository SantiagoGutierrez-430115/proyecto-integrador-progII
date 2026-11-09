using Cine.Data.DTOs;

namespace Cine.Data.Services.Interfaces
{
    public enum ResultadoPelicula { Ok, NoEncontrada, Invalida, Duplicada, TieneDependencias }

    public interface IPeliculaService
    {
        Task<ResultadoPaginadoDto<PeliculaDto>> GetAllAsync(string? titulo, int? idGenero, int? idClasificacion, int pagina, int tamanio);
        Task<PeliculaDto?> GetByIdAsync(int id);
        Task<(ResultadoPelicula Resultado, string? Error, PeliculaDto? Pelicula)> CreateAsync(GuardarPeliculaDto dto);
        Task<(ResultadoPelicula Resultado, string? Error)> UpdateAsync(int id, GuardarPeliculaDto dto);
        Task<(ResultadoPelicula Resultado, string? Error)> DeleteAsync(int id);
    }
}
