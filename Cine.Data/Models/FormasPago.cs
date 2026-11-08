using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class FormasPago
{
    public int IdFormaPago { get; set; }

    public string FormaPago { get; set; } = null!;

    public virtual ICollection<FacturasFormasPago> FacturasFormasPagos { get; set; } = new List<FacturasFormasPago>();
}
