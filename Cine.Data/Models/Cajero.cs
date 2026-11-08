using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Cajero
{
    public int IdCajero { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public int IdTurno { get; set; }

    public int? IdSucursal { get; set; }

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual Sucursale? IdSucursalNavigation { get; set; }

    public virtual Turno IdTurnoNavigation { get; set; } = null!;
}
