using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class ReseniasPelicula
{
    public int IdResenia { get; set; }

    public string? Comentario { get; set; }

    public DateTime FechaResenia { get; set; }

    public int IdPelicula { get; set; }

    public int IdPuntuacion { get; set; }

    public int CodCliente { get; set; }

    public virtual Cliente CodClienteNavigation { get; set; } = null!;

    public virtual Pelicula IdPeliculaNavigation { get; set; } = null!;

    public virtual Puntuacione IdPuntuacionNavigation { get; set; } = null!;
}
