using Cine.Data.DTOs;
using Cine.Data.Exceptions;
using Cine.Data.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TPI_Cine.Controllers
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

        // Alta de administradores: solo un Admin puede crear otro Admin.
        // POST api/usuario/registro-admin
        [HttpPost("registro-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegistroAdmin([FromBody] RegistroAdminDto dto)
        {
            try
            {
                var admin = await _usuarioService.RegisterAdminAsync(dto);
                return StatusCode(StatusCodes.Status201Created, new
                {
                    mensaje = $"Administrador '{admin.NombreUsuario}' creado correctamente.",
                    admin
                });
            }
            catch (ConflictoException ex)
            {
                return Conflict(new { mensaje = ex.Message });
            }
            catch (DbUpdateException)
            {
                // Lo rechazó un trigger o una restricción de la BD (identificadores repetidos, espacios, etc.)
                return Conflict(new { mensaje = "La base de datos rechazó el usuario: verificá que el nombre de usuario y el correo no estén en uso." });
            }
        }

        // GET api/usuario/admins
        [HttpGet("admins")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListarAdmins()
        {
            return Ok(await _usuarioService.ListarAdminsAsync());
        }
    }
}
