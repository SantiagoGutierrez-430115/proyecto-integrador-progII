using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Direccione
{
    public int IdDireccion { get; set; }

    public string Calle { get; set; } = null!;

    public int Altura { get; set; }

    public int? Piso { get; set; }

    public string? Depto { get; set; }

    public int IdBarrio { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual Barrio IdBarrioNavigation { get; set; } = null!;

    public virtual ICollection<Sucursale> Sucursales { get; set; } = new List<Sucursale>();
}
