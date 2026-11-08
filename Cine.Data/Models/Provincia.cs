using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Provincia
{
    public int IdProvincia { get; set; }

    public string Provincia1 { get; set; } = null!;

    public int IdPais { get; set; }

    public virtual ICollection<Ciudade> Ciudades { get; set; } = new List<Ciudade>();

    public virtual Paise IdPaisNavigation { get; set; } = null!;
}
