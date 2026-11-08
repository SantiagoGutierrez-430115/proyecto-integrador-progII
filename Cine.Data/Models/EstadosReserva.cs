using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class EstadosReserva
{
    public int IdEstadoReserva { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
