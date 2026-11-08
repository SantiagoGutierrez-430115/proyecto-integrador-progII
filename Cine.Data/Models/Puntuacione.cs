using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Puntuacione
{
    public int IdPuntuacion { get; set; }

    public int Puntuacion { get; set; }

    public virtual ICollection<ReseniasPelicula> ReseniasPeliculas { get; set; } = new List<ReseniasPelicula>();
}
