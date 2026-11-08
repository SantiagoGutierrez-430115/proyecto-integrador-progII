using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class FormatosPelicula
{
    public int IdFormatoPelicula { get; set; }

    public string Formato { get; set; } = null!;

    public virtual ICollection<Pelicula> Peliculas { get; set; } = new List<Pelicula>();
}
