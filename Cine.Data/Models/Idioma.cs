using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Idioma
{
    public int IdIdioma { get; set; }

    public string Idioma1 { get; set; } = null!;

    public virtual ICollection<PeliculasIdioma> PeliculasIdiomas { get; set; } = new List<PeliculasIdioma>();
}
