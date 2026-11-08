using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Sala
{
    public int IdSala { get; set; }

    public int NroSala { get; set; }

    public int IdTipoSala { get; set; }

    public int IdSucursal { get; set; }

    public virtual ICollection<Butaca> Butacas { get; set; } = new List<Butaca>();

    public virtual ICollection<Funcione> Funciones { get; set; } = new List<Funcione>();

    public virtual Sucursale IdSucursalNavigation { get; set; } = null!;

    public virtual TiposSala IdTipoSalaNavigation { get; set; } = null!;
}
