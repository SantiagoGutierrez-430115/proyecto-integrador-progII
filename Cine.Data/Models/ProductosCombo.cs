using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class ProductosCombo
{
    public int IdProductoCombo { get; set; }

    public int IdProductoShop { get; set; }

    public int IdCombo { get; set; }

    public decimal PrecioUnitario { get; set; }

    public int Cantidad { get; set; }

    public virtual Combo IdComboNavigation { get; set; } = null!;

    public virtual ProductosShop IdProductoShopNavigation { get; set; } = null!;
}
