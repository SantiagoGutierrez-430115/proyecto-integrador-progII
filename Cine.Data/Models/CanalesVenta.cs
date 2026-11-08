using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class CanalesVenta
{
    public int IdCanalVenta { get; set; }

    public string CanalVenta { get; set; } = null!;

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual ICollection<Promocione> Promociones { get; set; } = new List<Promocione>();
}
