using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Butaca
{
    public int IdButaca { get; set; }

    public int IdSala { get; set; }

    public int NroButaca { get; set; }

    public int IdTipoButacas { get; set; }

    public int IdEstadoButaca { get; set; }

    public virtual ICollection<Entrada> Entrada { get; set; } = new List<Entrada>();

    public virtual EstadosButaca IdEstadoButacaNavigation { get; set; } = null!;

    public virtual Sala IdSalaNavigation { get; set; } = null!;

    public virtual TiposButaca IdTipoButacasNavigation { get; set; } = null!;

    public virtual ICollection<ReservaDetalle> ReservaDetalles { get; set; } = new List<ReservaDetalle>();
}
