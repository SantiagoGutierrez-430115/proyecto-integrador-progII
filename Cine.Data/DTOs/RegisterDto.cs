using System;
using System.Collections.Generic;
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
        public string NroDoc { get; set; } = string.Empty;
        public int IdTipoDoc { get; set; }
        public DateOnly? FechaNacimiento { get; set; }
    }
}
