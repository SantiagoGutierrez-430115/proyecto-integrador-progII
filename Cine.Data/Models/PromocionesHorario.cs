using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class PromocionesHorario
{
    public int IdPromocionHorario { get; set; }

    public int IdPromocion { get; set; }

    public int IdHorario { get; set; }

    public virtual Horario IdHorarioNavigation { get; set; } = null!;

    public virtual Promocione IdPromocionNavigation { get; set; } = null!;
}
