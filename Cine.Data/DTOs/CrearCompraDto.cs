using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cine.Data.DTOs
{
    public class CrearCompraDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la función debe ser mayor a cero.")]
        public int IdFuncion { get; set; }

        [Required(ErrorMessage = "Debés indicar las butacas de la compra.")]
        [MinLength(1, ErrorMessage = "Debés elegir al menos una butaca.")]
        public List<int> IdsButacas { get; set; } = new List<int>();
    }
}
