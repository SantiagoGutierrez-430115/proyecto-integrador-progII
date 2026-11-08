using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class TiposContacto
{
    public int IdTipoContacto { get; set; }

    public string Tipo { get; set; } = null!;

    public virtual ICollection<Contacto> Contactos { get; set; } = new List<Contacto>();
}
