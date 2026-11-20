using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.DTOs
{
    public class ProductoShopDto
    {
        public int IdProductoShop { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string? Descripcion { get; set; }
        public int Stock { get; set; }

        public int IdMarca { get; set; }
        public string? Marca { get; set; }
        public int IdTipoProducto { get; set; }
        public string? TipoProducto { get; set; }
    }
}
