using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class ReservaDetalle
{
    public int IdReservaDetalle { get; set; }

    public int IdReserva { get; set; }

    public int IdFuncion { get; set; }

    public int IdButaca { get; set; }

    public virtual Butaca IdButacaNavigation { get; set; } = null!;

    public virtual Funcione IdFuncionNavigation { get; set; } = null!;

    public virtual Reserva IdReservaNavigation { get; set; } = null!;
}
