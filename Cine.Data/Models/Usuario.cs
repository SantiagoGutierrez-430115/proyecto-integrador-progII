using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public int IdRol { get; set; }

    public string CorreoElectronico { get; set; } = null!;

    public virtual Cliente? Cliente { get; set; }

    public virtual Role Rol { get; set; } = null!;
}
