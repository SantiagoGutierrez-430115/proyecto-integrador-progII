using Cine.Data.DTOs;
using Cine.Data.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TPI_Cine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReseniaController : ControllerBase
    {
        private readonly IReseniaService serv;

        public ReseniaController(IReseniaService _serv)
        {
            this.serv = _serv;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReseniaCreateDto dto)
        {
            try
            {
                var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(idUsuarioClaim, out int idUsuario))
                {
                    return Unauthorized("No se pudo identificar al usuario autenticado.");
                }

                var resenia = await serv.CreateAsync(dto, idUsuario);

                return CreatedAtAction(
                    nameof(ObtenerPorId),
                    new { idResenia = resenia.IdResenia },
                    resenia
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var resenias = await serv.ListarAsync();
                return Ok(resenias);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{idResenia}")]
        public async Task<IActionResult> ObtenerPorId(int idResenia)
        {
            try
            {
                var resenia = await serv.ObtenerPorIdAsync(idResenia);
                return Ok(resenia);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("pelicula/{idPelicula}")]
        public async Task<IActionResult> ListarPorPelicula(int idPelicula)
        {
            try
            {
                var resenias = await serv.ListarPorPeliculaAsync(idPelicula);
                return Ok(resenias);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("cliente/{codCliente}")]
        public async Task<IActionResult> ListarPorCliente(int codCliente)
        {
            try
            {
                var resenias = await serv.ListarPorClienteAsync(codCliente);
                return Ok(resenias);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
