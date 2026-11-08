using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Directore
{
    public int IdDirector { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public DateOnly? FechaNacimiento { get; set; }

    public string? Bio { get; set; }

    public virtual ICollection<Pelicula> Peliculas { get; set; } = new List<Pelicula>();
}
