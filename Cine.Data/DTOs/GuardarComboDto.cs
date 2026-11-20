using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.DTOs
{
    public class GuardarComboDto
    {
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(200, ErrorMessage = "La descripción no puede superar los 200 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0, 99999999.99, ErrorMessage = "El precio no puede ser negativo.")]
        public decimal? Precio { get; set; }

        [Required(ErrorMessage = "El tipo de combo es obligatorio.")]
        public int? IdTipoCombo { get; set; }

        [Required(ErrorMessage = "La lista de productos es obligatoria.")]
        [MinLength(1, ErrorMessage = "El combo tiene que tener al menos un producto.")]
        public List<GuardarComboProductoDto> Productos { get; set; } = new();

        // Rechaza elementos vacíos y productos repetidos dentro de la lista
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Productos.Any(p => p == null))
            {
                yield return new ValidationResult(
                    "La lista de productos no puede tener elementos vacíos.", new[] { nameof(Productos) });
                yield break;
            }

            var hayRepetidos = Productos
                .Where(p => p.IdProductoShop.HasValue)
                .GroupBy(p => p.IdProductoShop!.Value)
                .Any(g => g.Count() > 1);

            if (hayRepetidos)
            {
                yield return new ValidationResult(
                    "Un mismo producto no puede repetirse dentro del combo.", new[] { nameof(Productos) });
            }
        }
    }
}
