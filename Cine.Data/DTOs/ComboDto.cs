using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.DTOs
{
    public class ComboDto
    {
        public int IdCombo { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }

        public decimal SumaProductos { get; set; }

        public int IdTipoCombo { get; set; }
        public string? TipoCombo { get; set; }

        public List<ComboProductoDto> Productos { get; set; } = new();
    }
}
