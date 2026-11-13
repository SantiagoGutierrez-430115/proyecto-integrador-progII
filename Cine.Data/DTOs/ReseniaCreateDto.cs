using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.DTOs
{
    public class ReseniaCreateDto
    {
        [Required(ErrorMessage = "Debe seleccionar una película.")]
        public int? IdPelicula { get; set; }

        [Required(ErrorMessage = "Debe ingresar una puntuación.")]
        [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5.")]
        public int? Puntuacion { get; set; }

        public string? Comentario { get; set; }
    }
}
