using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.DTOs
{
    public class GuardarProductoShopDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0, 99999999.99, ErrorMessage = "El precio no puede ser negativo.")]
        public decimal? Precio { get; set; }

        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int? Stock { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria.")]
        public int? IdMarca { get; set; }

        [Required(ErrorMessage = "El tipo de producto es obligatorio.")]
        public int? IdTipoProducto { get; set; }
    }
}
