using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.DTOs
{
    public class ButacaFuncionDto
    {
        public int IdButaca { get; set; }
        public int NroButaca { get; set; }
        public decimal Precio { get; set; }
        public bool Disponible { get; set; }
    }
}
