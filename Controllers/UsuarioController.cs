using Cine.Data.DTOs;
using Cine.Data.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cine_TPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] RegisterDto registerDto)
        {
            try
            {

                var result = await _usuarioService.RegisterAsync(registerDto);
                if (!result)
                {
                    return BadRequest("El correo electrónico ya está en uso.");
                }

                return Ok("Usuario registrado correctamente.");

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Ocurrió un error al registrar el usuario: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _usuarioService.LoginAsync(loginDto);
                if (token == null)
                {
                    return Unauthorized("Credenciales inválidas.");
                }

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al iniciar sesión: {ex.Message}");
            }
        }
    }
}
