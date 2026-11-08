using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Combo
{
    public int IdCombo { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdTipoCombo { get; set; }

    public virtual TiposCombo IdTipoComboNavigation { get; set; } = null!;

    public virtual ICollection<ProductosCombo> ProductosCombos { get; set; } = new List<ProductosCombo>();
}
