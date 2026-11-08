using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Actore
{
    public int IdActor { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? Biografia { get; set; }

    public virtual ICollection<PeliculasActore> PeliculasActores { get; set; } = new List<PeliculasActore>();
}
