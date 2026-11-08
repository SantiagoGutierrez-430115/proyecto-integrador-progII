using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class TiposSala
{
    public int IdTipoSala { get; set; }

    public string TipoSala { get; set; } = null!;

    public virtual ICollection<Sala> Salas { get; set; } = new List<Sala>();
}
