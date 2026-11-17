using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.DTOs
{
    public class FuncionDto
    {
        public int IdFuncion { get; set; }

        public int IdPelicula { get; set; }

        public int IdSala { get; set; }

        public DateOnly Fecha { get; set; }

        public int IdHorario { get; set; }
    }
}
