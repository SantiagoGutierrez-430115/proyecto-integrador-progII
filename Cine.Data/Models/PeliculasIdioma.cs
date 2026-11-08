using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class PeliculasIdioma
{
    public int IdPeliculaIdioma { get; set; }

    public int IdPelicula { get; set; }

    public int? IdIdioma { get; set; }

    public virtual Idioma? IdIdiomaNavigation { get; set; }

    public virtual Pelicula IdPeliculaNavigation { get; set; } = null!;
}
