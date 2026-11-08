using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Contacto
{
    public int IdContacto { get; set; }

    public string Contacto1 { get; set; } = null!;

    public int IdTipoContacto { get; set; }

    public virtual ICollection<ClientesContacto> ClientesContactos { get; set; } = new List<ClientesContacto>();

    public virtual TiposContacto IdTipoContactoNavigation { get; set; } = null!;
}
