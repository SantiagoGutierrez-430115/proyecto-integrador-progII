using Cine.Data.DTOs;
using Cine.Data.Exceptions;
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
        public async Task<IActionResult> GetAll(
            [FromQuery] string? titulo, [FromQuery] int? idGenero, [FromQuery] int? idClasificacion,
            [FromQuery] int pagina = 1, [FromQuery] int tamanio = 10)
        {
            try
            {
                var resultado = await _peliculaService.GetAllAsync(titulo, idGenero, idClasificacion, pagina, tamanio);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener las películas: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var pelicula = await _peliculaService.GetByIdAsync(id);
                if (pelicula == null)
                {
                    return NotFound($"No existe la película {id}.");
                }

                return Ok(pelicula);
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
                var pelicula = await _peliculaService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = pelicula.IdPelicula }, pelicula);
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ConflictoException ex)
            {
                return Conflict(ex.Message);
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
                var actualizada = await _peliculaService.UpdateAsync(id, dto);
                if (!actualizada)
                {
                    return NotFound($"No existe la película {id}.");
                }

                return NoContent();
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ConflictoException ex)
            {
                return Conflict(ex.Message);
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
                var eliminada = await _peliculaService.DeleteAsync(id);
                if (!eliminada)
                {
                    return NotFound($"No existe la película {id}.");
                }

                return NoContent();
            }
            catch (ConflictoException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al eliminar la película: {ex.Message}");
            }
        }
    }
}
