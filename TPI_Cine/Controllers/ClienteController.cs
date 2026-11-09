using Cine.Data.DTOs;
using Cine.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace TPI_Cine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarClientes([FromQuery] string filtro)
        {
            var clientes = await _clienteService.BuscarClientes(filtro);
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerClientePorId(int id)
        {
            var cliente = await _clienteService.ObtenerClientePorId(id);
            if (cliente == null)
            {
                return NotFound(new { mensaje = "Cliente no encontrado." }); 
            }
            return Ok(cliente); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] ActualizarClienteDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            var resultado = await _clienteService.ActualizarCliente(id, dto);

            if (!resultado.Success)
            {
                return NotFound(new { mensaje = resultado.Mensaje }); 
            }

            return Ok(new { mensaje = resultado.Mensaje }); 
        }

        [HttpGet("{id}/reservas")]
        public async Task<IActionResult> ObtenerReservasDeCliente(int id)
        {
            var cliente = await _clienteService.ObtenerClientePorId(id);
            if (cliente == null)
            {
                return NotFound(new { mensaje = "El cliente no existe, por lo tanto no tiene reservas." });
            }

            var reservas = await _clienteService.ObtenerReservasDeCliente(id);
            return Ok(reservas); 
        }
    }
}
