using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class PeliculasActore
{
    public int IdPeliculaActor { get; set; }

    public int IdPelicula { get; set; }

    public int IdActor { get; set; }

    public virtual Actore IdActorNavigation { get; set; } = null!;

    public virtual Pelicula IdPeliculaNavigation { get; set; } = null!;
}
