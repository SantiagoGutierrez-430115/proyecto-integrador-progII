using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Ciudade
{
    public int IdCiudad { get; set; }

    public string Ciudad { get; set; } = null!;

    public int IdProvincia { get; set; }

    public virtual ICollection<Barrio> Barrios { get; set; } = new List<Barrio>();

    public virtual Provincia IdProvinciaNavigation { get; set; } = null!;
}
