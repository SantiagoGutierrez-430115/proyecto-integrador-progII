using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Pelicula
{
    public int IdPelicula { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateOnly FechaLanzamiento { get; set; }

    public int? IdPais { get; set; }

    public int? IdFormatoPelicula { get; set; }

    public int? IdClasificacion { get; set; }

    public int? IdDistribuidor { get; set; }

    public int IdEstadoPelicula { get; set; }

    public int? IdDirector { get; set; }

    public virtual ICollection<Funcione> Funciones { get; set; } = new List<Funcione>();

    public virtual Clasificacione? IdClasificacionNavigation { get; set; }

    public virtual Directore? IdDirectorNavigation { get; set; }

    public virtual Distribuidore? IdDistribuidorNavigation { get; set; }

    public virtual EstadosPelicula IdEstadoPeliculaNavigation { get; set; } = null!;

    public virtual FormatosPelicula? IdFormatoPeliculaNavigation { get; set; }

    public virtual Paise? IdPaisNavigation { get; set; }

    public virtual ICollection<PeliculasActore> PeliculasActores { get; set; } = new List<PeliculasActore>();

    public virtual ICollection<PeliculasGenero> PeliculasGeneros { get; set; } = new List<PeliculasGenero>();

    public virtual ICollection<PeliculasIdioma> PeliculasIdiomas { get; set; } = new List<PeliculasIdioma>();

    public virtual ICollection<ReseniasPelicula> ReseniasPeliculas { get; set; } = new List<ReseniasPelicula>();
}
