using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class TiposProducto
{
    public int IdTipoProducto { get; set; }

    public string TipoProducto { get; set; } = null!;

    public virtual ICollection<ProductosShop> ProductosShops { get; set; } = new List<ProductosShop>();
}
