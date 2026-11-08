using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class PeliculasGenero
{
    public int IdPeliculaGenero { get; set; }

    public int IdPelicula { get; set; }

    public int IdGenero { get; set; }

    public virtual Genero IdGeneroNavigation { get; set; } = null!;

    public virtual Pelicula IdPeliculaNavigation { get; set; } = null!;
}
