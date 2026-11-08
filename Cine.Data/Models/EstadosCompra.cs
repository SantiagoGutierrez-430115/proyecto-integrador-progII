using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class EstadosCompra
{
    public int IdEstadoCompra { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}
