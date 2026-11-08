using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class TiposCombo
{
    public int IdTipoCombo { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Combo> Combos { get; set; } = new List<Combo>();

    public virtual ICollection<Promocione> IdPromocions { get; set; } = new List<Promocione>();
}
