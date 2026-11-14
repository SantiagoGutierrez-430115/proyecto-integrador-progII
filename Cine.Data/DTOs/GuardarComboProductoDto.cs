using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.DTOs
{
    public class GuardarComboProductoDto
    {
        [Required(ErrorMessage = "El producto es obligatorio.")]
        public int? IdProductoShop { get; set; }

        // Si no se envía, se toma el precio actual del producto del shop
        [Range(0, 99999999.99, ErrorMessage = "El precio unitario no puede ser negativo.")]
        public decimal? PrecioUnitario { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        public int Cantidad { get; set; } = 1;
    }
}
