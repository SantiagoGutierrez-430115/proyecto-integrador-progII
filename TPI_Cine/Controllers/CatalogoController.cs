using Microsoft.AspNetCore.Mvc;
using Cine.Data.Repositories.Interfaces;

namespace TPI_Cine.Controllers
{
    // Listas (id + nombre) para llenar los desplegables del front. Públicas y de solo lectura.
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoController : ControllerBase
    {
        private readonly ICatalogoRepository _catalogo;

        public CatalogoController(ICatalogoRepository catalogo)
        {
            _catalogo = catalogo;
        }

        [HttpGet("paises")]
        public async Task<IActionResult> Paises()
        {
            try
            {
                var lista = await _catalogo.GetPaisesAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener los países: {ex.Message}");
            }
        }

        [HttpGet("formatos")]
        public async Task<IActionResult> Formatos()
        {
            try
            {
                var lista = await _catalogo.GetFormatosAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener los formatos: {ex.Message}");
            }
        }

        [HttpGet("clasificaciones")]
        public async Task<IActionResult> Clasificaciones()
        {
            try
            {
                var lista = await _catalogo.GetClasificacionesAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener las clasificaciones: {ex.Message}");
            }
        }

        [HttpGet("distribuidores")]
        public async Task<IActionResult> Distribuidores()
        {
            try
            {
                var lista = await _catalogo.GetDistribuidoresAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener los distribuidores: {ex.Message}");
            }
        }

        [HttpGet("directores")]
        public async Task<IActionResult> Directores()
        {
            try
            {
                var lista = await _catalogo.GetDirectoresAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener los directores: {ex.Message}");
            }
        }

        [HttpGet("generos")]
        public async Task<IActionResult> Generos()
        {
            try
            {
                var lista = await _catalogo.GetGenerosAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener los géneros: {ex.Message}");
            }
        }

        [HttpGet("idiomas")]
        public async Task<IActionResult> Idiomas()
        {
            try
            {
                var lista = await _catalogo.GetIdiomasAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener los idiomas: {ex.Message}");
            }
        }

        [HttpGet("actores")]
        public async Task<IActionResult> Actores()
        {
            try
            {
                var lista = await _catalogo.GetActoresAsync();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener los actores: {ex.Message}");
            }
        }
    }
}
