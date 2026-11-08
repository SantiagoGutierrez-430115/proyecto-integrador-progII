using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int IdRol { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual Role IdRolNavigation { get; set; } = null!;
}
