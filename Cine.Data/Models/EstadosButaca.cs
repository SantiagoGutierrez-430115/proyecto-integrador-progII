using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class EstadosButaca
{
    public int IdEstadoButaca { get; set; }

    public string EstadoButaca { get; set; } = null!;

    public virtual ICollection<Butaca> Butacas { get; set; } = new List<Butaca>();
}
