using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class VwFacturasTotalPagado
{
    public int NroFactura { get; set; }

    public decimal? TotalPagado { get; set; }
}
