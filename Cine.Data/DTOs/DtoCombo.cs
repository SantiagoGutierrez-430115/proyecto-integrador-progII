using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.DTOs
{
    internal class DtoCombo
    {

        public string Descripcion { get; set; } = null!;

        public int IdTipoCombo { get; set; }

        public decimal Precio { get; set; }

    }
}
