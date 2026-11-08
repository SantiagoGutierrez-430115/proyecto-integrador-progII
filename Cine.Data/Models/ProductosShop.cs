using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class ProductosShop
{
    public int IdProductoShop { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal Precio { get; set; }

    public string? Descripcion { get; set; }

    public int Stock { get; set; }

    public int IdMarca { get; set; }

    public int IdTipoProducto { get; set; }

    public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; } = new List<DetalleFactura>();

    public virtual Marca IdMarcaNavigation { get; set; } = null!;

    public virtual TiposProducto IdTipoProductoNavigation { get; set; } = null!;

    public virtual ICollection<ProductosCombo> ProductosCombos { get; set; } = new List<ProductosCombo>();
}
