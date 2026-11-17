using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class EstadosPelicula
{
    public int IdEstadoPelicula { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Pelicula> Peliculas { get; set; } = new List<Pelicula>();
}
