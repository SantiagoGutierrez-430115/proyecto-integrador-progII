using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Financiera
{
    public int IdFinanciera { get; set; }

    public string Financiera1 { get; set; } = null!;

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual ICollection<Promocione> Promociones { get; set; } = new List<Promocione>();
}
