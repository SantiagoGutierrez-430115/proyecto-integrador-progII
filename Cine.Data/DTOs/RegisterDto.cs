using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.DTOs
{
    public class RegisterDto
    {
        // Autenticación
        public string NombreUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        

        // Información personal / del cliente
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        [Required(ErrorMessage = "El número de documento es obligatorio.")]
        [StringLength(20, ErrorMessage = "El número de documento no puede superar los 20 caracteres.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El número de documento solo puede tener números (sin puntos, letras ni espacios).")]
        public string NroDoc { get; set; } = string.Empty;
        public int IdTipoDoc { get; set; }
        public DateOnly? FechaNacimiento { get; set; }
    }
}
