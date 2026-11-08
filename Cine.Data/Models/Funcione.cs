using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Funcione
{
    public int IdFuncion { get; set; }

    public int IdPelicula { get; set; }

    public int IdSala { get; set; }

    public DateOnly Fecha { get; set; }

    public int IdHorario { get; set; }

    public virtual ICollection<Entrada> Entrada { get; set; } = new List<Entrada>();

    public virtual Horario IdHorarioNavigation { get; set; } = null!;

    public virtual Pelicula IdPeliculaNavigation { get; set; } = null!;

    public virtual Sala IdSalaNavigation { get; set; } = null!;

    public virtual ICollection<PromocionesFuncione> PromocionesFunciones { get; set; } = new List<PromocionesFuncione>();

    public virtual ICollection<ReservaDetalle> ReservaDetalles { get; set; } = new List<ReservaDetalle>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
