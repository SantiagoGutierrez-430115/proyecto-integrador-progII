using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.DTOs
{
    public class FuncionCreateDto
    {
        [Required(ErrorMessage = "Debe seleccionar una película.")]
        public int? IdPelicula { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una sala.")]
        public int? IdSala { get; set; }

        [Required(ErrorMessage = "La fecha de la función es obligatoria.")]
        public DateOnly? Fecha { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un horario.")]
        public int? IdHorario { get; set; }
    }
}
