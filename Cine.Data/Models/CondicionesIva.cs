using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class CondicionesIva
{
    public int IdCondicionIva { get; set; }

    public string Condicion { get; set; } = null!;

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}
