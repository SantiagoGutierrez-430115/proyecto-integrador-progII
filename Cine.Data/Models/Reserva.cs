using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Reserva
{
    public int IdReserva { get; set; }

    public DateTime Fecha { get; set; }

    public int IdFuncion { get; set; }

    public int CodCliente { get; set; }

    public int IdEstadoReserva { get; set; }

    public decimal MontoReserva { get; set; }

    public virtual Cliente CodClienteNavigation { get; set; } = null!;

    public virtual EstadosReserva IdEstadoReservaNavigation { get; set; } = null!;

    public virtual Funcione IdFuncionNavigation { get; set; } = null!;

    public virtual ICollection<ReservaDetalle> ReservaDetalles { get; set; } = new List<ReservaDetalle>();
}
