using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class PreferenciasCliente
{
    public int IdPreferenciaCliente { get; set; }

    public int CodCliente { get; set; }

    public int IdGenero { get; set; }

    public bool? SuscribeNewsletter { get; set; }

    public virtual Cliente CodClienteNavigation { get; set; } = null!;

    public virtual Genero IdGeneroNavigation { get; set; } = null!;
}
