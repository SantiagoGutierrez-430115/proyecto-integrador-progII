using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Paise
{
    public int IdPais { get; set; }

    public string Pais { get; set; } = null!;

    public virtual ICollection<Pelicula> Peliculas { get; set; } = new List<Pelicula>();

    public virtual ICollection<Provincia> Provincia { get; set; } = new List<Provincia>();
}
