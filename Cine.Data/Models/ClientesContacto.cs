using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class ClientesContacto
{
    public int IdClientesContacto { get; set; }

    public int? IdContacto { get; set; }

    public int? CodCliente { get; set; }

    public virtual Cliente? CodClienteNavigation { get; set; }

    public virtual Contacto? IdContactoNavigation { get; set; }
}
