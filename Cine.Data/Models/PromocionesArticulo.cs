using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class PromocionesArticulo
{
    public int IdPromocionArticulo { get; set; }

    public int IdPromocion { get; set; }

    public int? IdEntrada { get; set; }

    public int? IdProductoShop { get; set; }

    public int? IdCombo { get; set; }

    public virtual Combo? IdComboNavigation { get; set; }

    public virtual Entrada? IdEntradaNavigation { get; set; }

    public virtual ProductosShop? IdProductoShopNavigation { get; set; }

    public virtual Promocione IdPromocionNavigation { get; set; } = null!;
}
