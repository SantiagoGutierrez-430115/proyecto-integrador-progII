using Cine.Data.DTOs;
using Cine.Data.Models;

namespace Cine.Data.Repositories.Interfaces
{
    public interface IPeliculaRepository
    {
        Task<ResultadoPaginadoDto<Pelicula>> GetAllAsync(string? titulo, int? idGenero, int? idClasificacion, int pagina, int tamanio);
        Task<Pelicula?> GetByIdAsync(int id);
        Task<bool> ExisteTituloAsync(string titulo, DateOnly fechaLanzamiento, int? excluirId = null);
        Task<bool> TieneDependenciasAsync(int id);

        // Devuelve un mensaje de error si alguna referencia no existe, o null si todo está bien
        Task<string?> ValidarReferenciasAsync(Pelicula p, IEnumerable<int> generos, IEnumerable<int> idiomas, IEnumerable<int> actores);

        Task<Pelicula> CreateAsync(Pelicula pelicula, IEnumerable<int> generos, IEnumerable<int> idiomas, IEnumerable<int> actores);
        Task<bool> UpdateAsync(int id, Pelicula datos, IEnumerable<int> generos, IEnumerable<int> idiomas, IEnumerable<int> actores);
        Task<bool> DeleteAsync(int id);
    }
}
