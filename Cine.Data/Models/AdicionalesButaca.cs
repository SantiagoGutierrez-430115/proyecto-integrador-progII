using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class AdicionalesButaca
{
    public int IdTipoButacas { get; set; }

    public decimal Adicional { get; set; }

    public virtual TiposButaca IdTipoButacasNavigation { get; set; } = null!;
}
