using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class FacturasFormasPago
{
    public int IdFacturaFormaPago { get; set; }

    public int NroFactura { get; set; }

    public int IdFormaPago { get; set; }

    public decimal MontoPagado { get; set; }

    public int? IdFinanciera { get; set; }

    public virtual Financiera? IdFinancieraNavigation { get; set; }

    public virtual FormasPago IdFormaPagoNavigation { get; set; } = null!;

    public virtual Factura NroFacturaNavigation { get; set; } = null!;
}
