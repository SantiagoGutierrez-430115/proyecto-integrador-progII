using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class TarifasEntrada
{
    public int IdTipoSala { get; set; }

    public decimal Precio { get; set; }

    public virtual TiposSala IdTipoSalaNavigation { get; set; } = null!;
}
