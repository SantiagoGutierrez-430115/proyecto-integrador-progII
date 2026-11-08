using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Sucursale
{
    public int IdSucursal { get; set; }

    public string Nombre { get; set; } = null!;

    public int? IdDireccion { get; set; }

    public virtual ICollection<Cajero> Cajeros { get; set; } = new List<Cajero>();

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual Direccione? IdDireccionNavigation { get; set; }

    public virtual ICollection<Sala> Salas { get; set; } = new List<Sala>();
}
