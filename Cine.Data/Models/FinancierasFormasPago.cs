using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class FinancierasFormasPago
{
    public int IdFinancieraFormaPago { get; set; }

    public int IdFinanciera { get; set; }

    public int IdFormaPago { get; set; }

    public virtual Financiera IdFinancieraNavigation { get; set; } = null!;

    public virtual FormasPago IdFormaPagoNavigation { get; set; } = null!;
}
