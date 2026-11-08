using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class TiposPromocione
{
    public int IdTipoPromocion { get; set; }

    public string TipoPromocion { get; set; } = null!;

    public virtual ICollection<Promocione> Promociones { get; set; } = new List<Promocione>();
}
