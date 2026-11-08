using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Marca
{
    public int IdMarca { get; set; }

    public string Marca1 { get; set; } = null!;

    public virtual ICollection<ProductosShop> ProductosShops { get; set; } = new List<ProductosShop>();
}
