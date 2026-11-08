using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class DiasPromocione
{
    public int IdDiaPromocion { get; set; }

    public string DiasPromocion { get; set; } = null!;

    public virtual ICollection<Promocione> Promociones { get; set; } = new List<Promocione>();
}
