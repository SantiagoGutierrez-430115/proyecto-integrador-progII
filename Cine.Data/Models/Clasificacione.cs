using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Clasificacione
{
    public int IdClasificacion { get; set; }

    public string Clasificacion { get; set; } = null!;

    public virtual ICollection<Pelicula> Peliculas { get; set; } = new List<Pelicula>();
}
