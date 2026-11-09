using Cine.Data.DTOs;
using Cine.Data.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TPI_Cine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculaController : ControllerBase
    {
        private readonly IPeliculaService _peliculaService;

        public PeliculaController(IPeliculaService peliculaService)
        {
            _peliculaService = peliculaService;
        }

        // GET api/pelicula?titulo=avatar&idGenero=2&idClasificacion=1&pagina=1&tamanio=10
        [HttpGet]
        public async Task<ActionResult<ResultadoPaginadoDto<PeliculaDto>>> GetAll(
            [FromQuery] string? titulo, [FromQuery] int? idGenero, [FromQuery] int? idClasificacion,
            [FromQuery] int pagina = 1, [FromQuery] int tamanio = 10)
        {
            try
            {
                return Ok(await _peliculaService.GetAllAsync(titulo, idGenero, idClasificacion, pagina, tamanio));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener las películas: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PeliculaDto>> GetById(int id)
        {
            try
            {
                var pelicula = await _peliculaService.GetByIdAsync(id);
                return pelicula == null ? NotFound($"No existe la película {id}.") : Ok(pelicula);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener la película: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] GuardarPeliculaDto dto)
        {
            try
            {
                var (resultado, error, pelicula) = await _peliculaService.CreateAsync(dto);
                return resultado switch
                {
                    ResultadoPelicula.Ok => CreatedAtAction(nameof(GetById), new { id = pelicula!.IdPelicula }, pelicula),
                    ResultadoPelicula.Duplicada => Conflict(error),
                    _ => BadRequest(error)
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al crear la película: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] GuardarPeliculaDto dto)
        {
            try
            {
                var (resultado, error) = await _peliculaService.UpdateAsync(id, dto);
                return resultado switch
                {
                    ResultadoPelicula.Ok => NoContent(),
                    ResultadoPelicula.NoEncontrada => NotFound($"No existe la película {id}."),
                    ResultadoPelicula.Duplicada => Conflict(error),
                    _ => BadRequest(error)
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al actualizar la película: {ex.Message}");
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var (resultado, error) = await _peliculaService.DeleteAsync(id);
                return resultado switch
                {
                    ResultadoPelicula.Ok => NoContent(),
                    ResultadoPelicula.NoEncontrada => NotFound($"No existe la película {id}."),
                    ResultadoPelicula.TieneDependencias => Conflict(error),
                    _ => BadRequest(error)
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al eliminar la película: {ex.Message}");
            }
        }
    }
}
