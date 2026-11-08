using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class DetalleFactura
{
    public int IdDetalleFactura { get; set; }

    public int NroFactura { get; set; }

    public int? IdEntrada { get; set; }

    public int? IdProductoShop { get; set; }

    public int? IdPromocion { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public int? IdCombo { get; set; }

    public decimal DescuentoUnitario { get; set; }

    public decimal PrecioFinalUnitario { get; set; }

    public virtual Combo? IdComboNavigation { get; set; }

    public virtual Entrada? IdEntradaNavigation { get; set; }

    public virtual ProductosShop? IdProductoShopNavigation { get; set; }

    public virtual Promocione? IdPromocionNavigation { get; set; }

    public virtual Factura NroFacturaNavigation { get; set; } = null!;
}
