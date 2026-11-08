using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Horario
{
    public int IdHorario { get; set; }

    public TimeOnly Horario1 { get; set; }

    public virtual ICollection<Funcione> Funciones { get; set; } = new List<Funcione>();

    public virtual ICollection<PromocionesHorario> PromocionesHorarios { get; set; } = new List<PromocionesHorario>();
}
