using Cine.Data.DTOs;
using Cine.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TPI_Cine.Controllers
{
    // Listas para llenar los desplegables del front. Públicas y de solo lectura.
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
        public Task<ActionResult<List<ItemDto>>> Paises() => Responder(_catalogo.GetPaisesAsync);

        [HttpGet("formatos")]
        public Task<ActionResult<List<ItemDto>>> Formatos() => Responder(_catalogo.GetFormatosAsync);

        [HttpGet("clasificaciones")]
        public Task<ActionResult<List<ItemDto>>> Clasificaciones() => Responder(_catalogo.GetClasificacionesAsync);

        [HttpGet("distribuidores")]
        public Task<ActionResult<List<ItemDto>>> Distribuidores() => Responder(_catalogo.GetDistribuidoresAsync);

        [HttpGet("directores")]
        public Task<ActionResult<List<ItemDto>>> Directores() => Responder(_catalogo.GetDirectoresAsync);

        [HttpGet("generos")]
        public Task<ActionResult<List<ItemDto>>> Generos() => Responder(_catalogo.GetGenerosAsync);

        [HttpGet("idiomas")]
        public Task<ActionResult<List<ItemDto>>> Idiomas() => Responder(_catalogo.GetIdiomasAsync);

        [HttpGet("actores")]
        public Task<ActionResult<List<ItemDto>>> Actores() => Responder(_catalogo.GetActoresAsync);

        private async Task<ActionResult<List<ItemDto>>> Responder(Func<Task<List<ItemDto>>> obtener)
        {
            try
            {
                return Ok(await obtener());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener el catálogo: {ex.Message}");
            }
        }
    }
}
