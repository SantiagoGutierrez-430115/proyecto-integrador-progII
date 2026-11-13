using Cine.Data.DTOs;
using Cine.Data.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace TPI_Cine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ButacaController : ControllerBase
    {
        private readonly IButacaService serv;
        public ButacaController(IButacaService _serv)
        {
            this.serv = _serv;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ButacaDto>> CrearButaca([FromBody] ButacaCreateDto dto)
        {
            try
            {
                var butacaCreada = await serv.CreateAsync(dto);

                return StatusCode(201, butacaCreada);
            }
            catch (Exception ex)
            {

                return BadRequest(
                    new
                    {
                        mensaje = ex.Message,
                    });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<ButacaDto>>> ListarButacas()
        {
            var butacas = await serv.ListarAsync();

            return Ok(butacas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ButacaDto>> ObtenerButaca(int id)
        {
            try
            {
                var butaca = await serv.ObtenerPorIdAsync(id);

                return Ok(butaca);
            }
            catch (Exception ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }

        [HttpGet("sala/{idSala}")]
        public async Task<ActionResult<List<ButacaDto>>> ListarPorSala(int idSala)
        {
            var butacas = await serv.ListarPorSalaAsync(idSala);

            return Ok(butacas);
        }

        [HttpGet("sala/{idSala}/disponibles")]
        public async Task<ActionResult<List<ButacaDto>>> ListarDisponibles(int idSala)
        {
            var butacas = await serv.ListarDisponiblesPorSalaAsync(idSala);

            return Ok(butacas);
        }
    }
}
