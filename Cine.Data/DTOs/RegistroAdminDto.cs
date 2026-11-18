using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cine.Data.DTOs
{
    public class RegistroAdminDto
    {
        [Required, StringLength(50)]
        public string NombreUsuario { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string CorreoElectronico { get; set; } = string.Empty;
        [Required, MinLength(8)]
        public string Contraseña { get; set; } = string.Empty;
    }
}
