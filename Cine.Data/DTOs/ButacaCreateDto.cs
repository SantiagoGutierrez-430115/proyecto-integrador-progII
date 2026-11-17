using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.DTOs
{
    public class ButacaCreateDto
    {
        [Required(ErrorMessage = "Debe asignar esta butaca a una sala.")]
        public int? IdSala { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tipo de butaca.")]
        public int? IdTipoButaca { get; set; }
    }
}
