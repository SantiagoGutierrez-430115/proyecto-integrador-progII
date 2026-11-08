using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Promocione
{
    public int IdPromocion { get; set; }

    public string? Descripcion { get; set; }

    public int IdTipoPromocion { get; set; }

    public int IdEstadoPromocion { get; set; }

    public int IdDiaPromocion { get; set; }

    public int IdFinanciera { get; set; }

    public int IdCanalVenta { get; set; }

    public int? CodCliente { get; set; }

    public virtual Cliente? CodClienteNavigation { get; set; }

    public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; } = new List<DetalleFactura>();

    public virtual CanalesVenta IdCanalVentaNavigation { get; set; } = null!;

    public virtual DiasPromocione IdDiaPromocionNavigation { get; set; } = null!;

    public virtual EstadosPromocione IdEstadoPromocionNavigation { get; set; } = null!;

    public virtual Financiera IdFinancieraNavigation { get; set; } = null!;

    public virtual TiposPromocione IdTipoPromocionNavigation { get; set; } = null!;

    public virtual ICollection<PromocionesFuncione> PromocionesFunciones { get; set; } = new List<PromocionesFuncione>();

    public virtual ICollection<PromocionesHorario> PromocionesHorarios { get; set; } = new List<PromocionesHorario>();
}
