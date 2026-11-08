using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class TiposDocumento
{
    public int IdTipoDoc { get; set; }

    public string Tipo { get; set; } = null!;

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
}
