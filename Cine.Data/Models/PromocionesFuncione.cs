using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class PromocionesFuncione
{
    public int IdPromocionFuncion { get; set; }

    public int IdPromocion { get; set; }

    public int IdFuncion { get; set; }

    public virtual Funcione IdFuncionNavigation { get; set; } = null!;

    public virtual Promocione IdPromocionNavigation { get; set; } = null!;
}
