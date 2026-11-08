using BCrypt.Net;
using Cine.Data.DTOs;
using Cine.Data.Models;
using Cine.Data.Repositories.Interfaces;
using Cine.Data.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Services.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public UsuarioService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var usuario = await _usuarioRepository.GetUsuarioByEmailAsync(dto.Email);
            if (usuario == null) return null;

            try
            {
                // Verifica el hash
                if (!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.Contraseña))
                {
                    return null; // Contraseña incorrecta
                }
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // Si el valor en la BD es texto plano o un hash corrupto,
                // se rechaza el login de forma controlada sin romper la API.
                return null;
            }

            return GenerarJwtToken(usuario);
        }

        private string? GenerarJwtToken(Usuario usuario)
        {
            var secretKey = _configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new Exception("El valor _configuration[\"Jwt:Key\"] devolvió null. El servicio no está leyendo el archivo actualizado.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Configuración de los claims del token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
                new Claim(ClaimTypes.Role, usuario.Rol.Descripcion)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> RegisterAsync(RegisterDto registerDto)
        {
            var ExisteUsuario = await _usuarioRepository.GetUsuarioByEmailAsync(registerDto.Email);
            if (ExisteUsuario != null)
            {
                return false;
            }

            var rolCliente = await _usuarioRepository.GetRoleByNameAsync("Cliente");
            if (rolCliente == null)
            {
                throw new Exception("No se encontró el rol 'Cliente' en la base de datos.");
            }

            // Mapeo de RegisterDto a Usuario y Cliente
            var usuario = new Usuario
            {
                NombreUsuario = registerDto.NombreUsuario,
                CorreoElectronico = registerDto.Email,
                Contraseña = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                IdRol = rolCliente.IdRol
            };

            var cliente = new Cliente
            {
                Nombre = registerDto.Nombre,
                Apellido = registerDto.Apellido,
                NroDoc = registerDto.NroDoc,
                IdTipoDoc = registerDto.IdTipoDoc,
                FechaNacimiento = registerDto.FechaNacimiento,
            };

            return await _usuarioRepository.RegisterUserAndClientAsync(usuario, cliente);
        }
    }
}
