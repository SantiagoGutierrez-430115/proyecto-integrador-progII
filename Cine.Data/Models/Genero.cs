using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Genero
{
    public int IdGenero { get; set; }

    public string Genero1 { get; set; } = null!;

    public virtual ICollection<PeliculasGenero> PeliculasGeneros { get; set; } = new List<PeliculasGenero>();

    public virtual ICollection<PreferenciasCliente> PreferenciasClientes { get; set; } = new List<PreferenciasCliente>();
}
