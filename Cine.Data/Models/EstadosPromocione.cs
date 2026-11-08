using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class EstadosPromocione
{
    public int IdEstadoPromocion { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Promocione> Promociones { get; set; } = new List<Promocione>();
}
