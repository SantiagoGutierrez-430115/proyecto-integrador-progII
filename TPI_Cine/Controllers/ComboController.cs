using Cine.Data.DTOs;
using Cine.Data.Exceptions;
using Cine.Data.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TPI_Cine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComboController : ControllerBase
    {
        private readonly IComboService _comboService;

        public ComboController(IComboService comboService)
        {
            _comboService = comboService;
        }

        // GET api/combo?descripcion=grande&idTipoCombo=1&pagina=1&tamanio=10
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? descripcion, [FromQuery] int? idTipoCombo,
            [FromQuery] int pagina = 1, [FromQuery] int tamanio = 10)
        {
            try
            {
                var resultado = await _comboService.GetAllAsync(descripcion, idTipoCombo, pagina, tamanio);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener los combos: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var combo = await _comboService.GetByIdAsync(id);
                if (combo == null)
                {
                    return NotFound($"No existe el combo {id}.");
                }

                return Ok(combo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener el combo: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] GuardarComboDto dto)
        {
            try
            {
                var combo = await _comboService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = combo.IdCombo }, combo);
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
                return StatusCode(500, $"Ocurrió un error al crear el combo: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] GuardarComboDto dto)
        {
            try
            {
                var actualizado = await _comboService.UpdateAsync(id, dto);
                if (!actualizado)
                {
                    return NotFound($"No existe el combo {id}.");
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
                return StatusCode(500, $"Ocurrió un error al actualizar el combo: {ex.Message}");
            }
        }

        // Baja física: borra el combo y sus productos. Si está en facturas o promociones responde 409
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _comboService.DeleteAsync(id);
                if (!eliminado)
                {
                    return NotFound($"No existe el combo {id}.");
                }

                return NoContent();
            }
            catch (ConflictoException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al eliminar el combo: {ex.Message}");
            }
        }
    }
}
