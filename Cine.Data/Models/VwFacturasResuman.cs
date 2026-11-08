using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class VwFacturasResuman
{
    public int NroFactura { get; set; }

    public string Estado { get; set; } = null!;

    public decimal? TotalFactura { get; set; }

    public decimal? TotalPagado { get; set; }

    public decimal? SaldoPendiente { get; set; }
}
