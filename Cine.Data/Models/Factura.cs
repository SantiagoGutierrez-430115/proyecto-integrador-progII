using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Factura
{
    public int NroFactura { get; set; }

    public int? IdCondicionIva { get; set; }

    public DateTime Fecha { get; set; }

    public int CodCliente { get; set; }

    public int IdCajero { get; set; }

    public int IdSucursal { get; set; }

    public int IdFinanciera { get; set; }

    public int IdEstadoCompra { get; set; }

    public int IdCanalVenta { get; set; }

    public virtual Cliente CodClienteNavigation { get; set; } = null!;

    public virtual ICollection<DetalleFactura> DetalleFacturas { get; set; } = new List<DetalleFactura>();

    public virtual ICollection<FacturasFormasPago> FacturasFormasPagos { get; set; } = new List<FacturasFormasPago>();

    public virtual Cajero IdCajeroNavigation { get; set; } = null!;

    public virtual CanalesVenta IdCanalVentaNavigation { get; set; } = null!;

    public virtual CondicionesIva? IdCondicionIvaNavigation { get; set; }

    public virtual EstadosCompra IdEstadoCompraNavigation { get; set; } = null!;

    public virtual Financiera IdFinancieraNavigation { get; set; } = null!;

    public virtual Sucursale IdSucursalNavigation { get; set; } = null!;
}
