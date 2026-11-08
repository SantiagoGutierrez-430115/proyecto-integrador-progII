using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Entrada
{
    public int IdEntrada { get; set; }

    public int IdFuncion { get; set; }

    public int IdButaca { get; set; }

    public decimal Precio { get; set; }

    public virtual DetalleFactura? DetalleFactura { get; set; }

    public virtual Butaca IdButacaNavigation { get; set; } = null!;

    public virtual Funcione IdFuncionNavigation { get; set; } = null!;

    public virtual ICollection<PromocionesArticulo> PromocionesArticulos { get; set; } = new List<PromocionesArticulo>();
}
