using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Promocione
{
    public int IdPromocion { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdTipoPromocion { get; set; }

    public int IdEstadoPromocion { get; set; }

    public int IdDiaPromocion { get; set; }

    public int? IdFinanciera { get; set; }

    public int? IdCanalVenta { get; set; }

    public int? CodCliente { get; set; }

    public decimal PorcentajeDescuento { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public decimal MontoMinimo { get; set; }

    public virtual Cliente? CodClienteNavigation { get; set; }

    public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; } = new List<DetalleFactura>();

    public virtual CanalesVenta? IdCanalVentaNavigation { get; set; }

    public virtual DiasPromocione IdDiaPromocionNavigation { get; set; } = null!;

    public virtual EstadosPromocione IdEstadoPromocionNavigation { get; set; } = null!;

    public virtual Financiera? IdFinancieraNavigation { get; set; }

    public virtual TiposPromocione IdTipoPromocionNavigation { get; set; } = null!;

    public virtual ICollection<PromocionesArticulo> PromocionesArticulos { get; set; } = new List<PromocionesArticulo>();

    public virtual ICollection<PromocionesFuncione> PromocionesFunciones { get; set; } = new List<PromocionesFuncione>();

    public virtual ICollection<PromocionesHorario> PromocionesHorarios { get; set; } = new List<PromocionesHorario>();

    public virtual ICollection<TiposCombo> IdTipoCombos { get; set; } = new List<TiposCombo>();

    public virtual ICollection<TiposProducto> IdTipoProductos { get; set; } = new List<TiposProducto>();
}
