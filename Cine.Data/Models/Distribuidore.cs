using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Distribuidore
{
    public int IdDistribuidor { get; set; }

    public string Distribuidor { get; set; } = null!;

    public virtual ICollection<Pelicula> Peliculas { get; set; } = new List<Pelicula>();
}
