using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Turno
{
    public int IdTurno { get; set; }

    public string Turno1 { get; set; } = null!;

    public virtual ICollection<Cajero> Cajeros { get; set; } = new List<Cajero>();
}
