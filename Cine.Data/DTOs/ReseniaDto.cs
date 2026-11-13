using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.DTOs
{
    public class ReseniaDto
    {
        public int IdResenia { get; set; }
        public string? Comentario { get; set; }
        public DateTime FechaResenia { get; set; }
        public int IdPelicula { get; set; }
        public int Puntuacion { get; set; }
        public int CodCliente { get; set; }
    }
}
