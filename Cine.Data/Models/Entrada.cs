using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Entrada
{
    public int NroEntrada { get; set; }

    public int IdFuncion { get; set; }

    public int IdButaca { get; set; }

    public decimal Precio { get; set; }

    public decimal PrecioUnitario { get; set; }

    public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; } = new List<DetalleFactura>();

    public virtual Butaca IdButacaNavigation { get; set; } = null!;

    public virtual Funcione IdFuncionNavigation { get; set; } = null!;
}
