using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Barrio
{
    public int IdBarrio { get; set; }

    public string Barrio1 { get; set; } = null!;

    public int IdCiudad { get; set; }

    public virtual ICollection<Direccione> Direcciones { get; set; } = new List<Direccione>();

    public virtual Ciudade IdCiudadNavigation { get; set; } = null!;
}
