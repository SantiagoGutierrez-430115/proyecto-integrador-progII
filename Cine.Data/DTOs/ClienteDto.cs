using System.ComponentModel.DataAnnotations;

namespace Cine.Data.DTOs
{
    public class ClienteDto
    {
        public int CodCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string NroDoc { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
    }

    // DTO para recibir los datos cuando el empleado edita un cliente.
    // Todos los campos son opcionales: el que venga null (o no se envíe) no se modifica.
    // Los largos máximos coinciden con las columnas de la base de datos.
    public class ActualizarClienteDto
    {
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string? Nombre { get; set; }

        [StringLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres.")]
        public string? Apellido { get; set; }

        [StringLength(20, ErrorMessage = "El número de documento no puede superar los 20 caracteres.")]
        public string? NroDoc { get; set; }

        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [StringLength(254, ErrorMessage = "El correo electrónico no puede superar los 254 caracteres.")]
        public string? CorreoElectronico { get; set; }
    }

    // DTO para revisar las reservas del cliente en la sección de soporte
    public class ReservaClienteDto
    {
        public int IdReserva { get; set; }
        public DateTime FechaReserva { get; set; }
        public string Pelicula { get; set; } = string.Empty;
        public DateOnly FechaFuncion { get; set; }   // NUEVO: sin la fecha, el horario solo es ambiguo
        public string Horario { get; set; } = string.Empty;
        public int NroSala { get; set; }
        public string EstadoReserva { get; set; } = string.Empty;
        public decimal MontoReserva { get; set; }
    }
}
