using Cine.Data.DTOs;
using Cine.Data.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TPI_Cine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FuncionController : ControllerBase
    {
        private readonly IFuncionService serv;

        public FuncionController(IFuncionService _serv)
        {
            this.serv = _serv;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<FuncionDto>> CrearFuncion([FromBody] FuncionCreateDto dto)
        {
            try
            {
                var funcionCreada = await serv.CreateAsync(dto);

                return StatusCode(201, funcionCreada);
            }
            catch (Exception ex)
            {

                return BadRequest(new
                {
                    mensaje = ex.Message,
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<FuncionDto>>> ListarFunciones()
        {
            var funciones = await serv.ListarAsync();

            return Ok(funciones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FuncionDto>> ObtenerFuncion(int id)
        {
            try
            {
                var funcion = await serv.ObtenerPorIdAsync(id);

                return Ok(funcion);
            }
            catch (Exception ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }

        [HttpGet("pelicula/{idPelicula}")]
        public async Task<ActionResult<List<FuncionDto>>> ListarPorPelicula(int idPelicula)
        {
            var funciones = await serv.ListarPorPeliculaAsync(idPelicula);

            return Ok(funciones);
        }

        [HttpGet("fecha")]
        public async Task<ActionResult<List<FuncionDto>>> ListarPorFecha([FromQuery] DateOnly fecha)
        {
            var funciones = await serv.ListarPorFechaAsync(fecha);

            return Ok(funciones);
        }
    }
}
