using Cine.Data.DTOs;
using Cine.Data.Respuesta;
using Cine.Data.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TPI_Cine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Soporte de clientes: solo lo usan empleados (rol "Admin" en la tabla roles).
    // Sin esto, cualquiera podía ver y modificar los datos de todos los clientes.
    [Authorize(Roles = "Admin")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // GET api/clientes?filtro=perez
        [HttpGet]
        public async Task<IActionResult> BuscarClientes([FromQuery] string? filtro)
        {
            var resultado = await _clienteService.BuscarClientes(filtro);
            if (!resultado.Success)
            {
                return NotFound(new { mensaje = resultado.Mensaje });
            }
            return Ok(new { mensaje = resultado.Mensaje, clientes = resultado.Data });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerClientePorId(int id)
        {
            var cliente = await _clienteService.ObtenerClientePorId(id);
            if (cliente == null)
            {
                return NotFound(new { mensaje = "Cliente no encontrado." });
            }
            return Ok(cliente);
        }

        // Con [ApiController] el ModelState se valida solo: si el DTO no es válido
        // ASP.NET devuelve 400 automáticamente, por eso se quitó el if (!ModelState.IsValid).
        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] ActualizarClienteDto dto)
        {
            var resultado = await _clienteService.ActualizarCliente(id, dto);
            return resultado.Success
                ? Ok(new { mensaje = resultado.Mensaje })
                : RespuestaDeError(resultado);
        }

        [HttpGet("{id:int}/reservas")]
        public async Task<IActionResult> ObtenerReservasDeCliente(int id)
        {
            var resultado = await _clienteService.ObtenerReservasDeCliente(id);
            if (!resultado.Success)
            {
                return RespuestaDeError(resultado);
            }
            return Ok(new { mensaje = resultado.Mensaje, reservas = resultado.Data });
        }

        // Traduce el tipo de error del servicio al código HTTP correcto.
        // Antes todo error devolvía 404, incluso "el DNI ya pertenece a otro cliente".
        private IActionResult RespuestaDeError(OperationResult resultado)
        {
            var cuerpo = new { mensaje = resultado.Mensaje };
            return resultado.Error switch
            {
                TipoError.NoEncontrado => NotFound(cuerpo),   // 404
                TipoError.Conflicto => Conflict(cuerpo),      // 409
                _ => BadRequest(cuerpo)                       // 400
            };
        }
    }
}
