using System;
using System.Collections.Generic;

namespace Cine.Data.Models;

public partial class Cliente
{
    public int CodCliente { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public int NroDoc { get; set; }

    public int IdTipoDoc { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public DateTime FechaRegistro { get; set; }

    public string Contraseña { get; set; } = null!;

    public int? IdDireccion { get; set; }

    public int? IdUsuario { get; set; }

    public virtual ICollection<ClientesContacto> ClientesContactos { get; set; } = new List<ClientesContacto>();

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual Direccione? IdDireccionNavigation { get; set; }

    public virtual TiposDocumento IdTipoDocNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<PreferenciasCliente> PreferenciasClientes { get; set; } = new List<PreferenciasCliente>();

    public virtual ICollection<Promocione> Promociones { get; set; } = new List<Promocione>();

    public virtual ICollection<ReseniasPelicula> ReseniasPeliculas { get; set; } = new List<ReseniasPelicula>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
