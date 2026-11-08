using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class TiposButaca
{
    public int IdTipoButacas { get; set; }

    public string TipoButaca { get; set; } = null!;

    public virtual ICollection<Butaca> Butacas { get; set; } = new List<Butaca>();
}
