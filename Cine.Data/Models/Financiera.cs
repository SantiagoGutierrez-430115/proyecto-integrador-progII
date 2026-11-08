using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Financiera
{
    public int IdFinanciera { get; set; }

    public string Financiera1 { get; set; } = null!;

    public virtual ICollection<FacturasFormasPago> FacturasFormasPagos { get; set; } = new List<FacturasFormasPago>();

    public virtual ICollection<FinancierasFormasPago> FinancierasFormasPagos { get; set; } = new List<FinancierasFormasPago>();

    public virtual ICollection<Promocione> Promociones { get; set; } = new List<Promocione>();
}
