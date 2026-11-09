using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.DTOs
{
    public class ClienteDto
    {
        public int CodCliente { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NroDoc { get; set; }
        public string TipoDocumento { get; set; }
        public string CorreoElectronico { get; set; }
    }

    // DTO para recibir los datos cuando el empleado edita un cliente
    public class ActualizarClienteDto
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NroDoc { get; set; }
        public string CorreoElectronico { get; set; }
    }

    // DTO para revisar las reservas del cliente en la sección de soporte
    public class ReservaClienteDto
    {
        public int IdReserva { get; set; }
        public DateTime FechaReserva { get; set; }
        public string Pelicula { get; set; }
        public string Horario { get; set; }
        public int NroSala { get; set; }
        public string EstadoReserva { get; set; }
        public decimal MontoReserva { get; set; }
    }
}
