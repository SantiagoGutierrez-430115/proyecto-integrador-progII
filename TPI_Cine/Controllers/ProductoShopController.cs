using Cine.Data.DTOs;
using Cine.Data.Exceptions;
using Cine.Data.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TPI_Cine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoShopController : ControllerBase
    {
        private readonly IProductoShopService _productoShopService;

        public ProductoShopController(IProductoShopService productoShopService)
        {
            _productoShopService = productoShopService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? nombre, [FromQuery] int? idMarca, [FromQuery] int? idTipoProducto,
            [FromQuery] int pagina = 1, [FromQuery] int tamanio = 10)
        {
            try
            {
                var resultado = await _productoShopService.GetAllAsync(nombre, idMarca, idTipoProducto, pagina, tamanio);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener los productos: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var producto = await _productoShopService.GetByIdAsync(id);
                if (producto == null)
                {
                    return NotFound($"No existe el producto {id}.");
                }

                return Ok(producto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener el producto: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] GuardarProductoShopDto dto)
        {
            try
            {
                var producto = await _productoShopService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = producto.IdProductoShop }, producto);
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
                return StatusCode(500, $"Ocurrió un error al crear el producto: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] GuardarProductoShopDto dto)
        {
            try
            {
                var actualizado = await _productoShopService.UpdateAsync(id, dto);
                if (!actualizado)
                {
                    return NotFound($"No existe el producto {id}.");
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
                return StatusCode(500, $"Ocurrió un error al actualizar el producto: {ex.Message}");
            }
        }

        // Baja física: si el producto está en facturas, combos o promociones responde 409
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var eliminado = await _productoShopService.DeleteAsync(id);
                if (!eliminado)
                {
                    return NotFound($"No existe el producto {id}.");
                }

                return NoContent();
            }
            catch (ConflictoException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al eliminar el producto: {ex.Message}");
            }
        }
    }
}
