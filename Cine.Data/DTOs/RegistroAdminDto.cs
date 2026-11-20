using System.ComponentModel.DataAnnotations;

namespace Cine.Data.DTOs
{
    // Datos para que un Admin cree otro Admin. Un administrador es solo un usuario:
    // no tiene fila en la tabla clientes (por eso no pide nombre, documento, etc.).
    public class RegistroAdminDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 50 caracteres.")]
        [RegularExpression(@"^\S+$", ErrorMessage = "El nombre de usuario no puede tener espacios.")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [StringLength(254, ErrorMessage = "El correo electrónico no puede superar los 254 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Password { get; set; } = string.Empty;
    }

    // Lo que se devuelve al listar administradores
    public class AdminDto
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
