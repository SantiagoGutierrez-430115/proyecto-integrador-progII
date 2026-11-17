using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Cine.Data.Models;

public partial class CineDbContext : DbContext
{
    public CineDbContext()
    {
    }

    public CineDbContext(DbContextOptions<CineDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actore> Actores { get; set; }

    public virtual DbSet<AdicionalesButaca> AdicionalesButacas { get; set; }

    public virtual DbSet<Barrio> Barrios { get; set; }

    public virtual DbSet<Butaca> Butacas { get; set; }

    public virtual DbSet<Cajero> Cajeros { get; set; }

    public virtual DbSet<CanalesVenta> CanalesVentas { get; set; }

    public virtual DbSet<Ciudade> Ciudades { get; set; }

    public virtual DbSet<Clasificacione> Clasificaciones { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<ClientesContacto> ClientesContactos { get; set; }

    public virtual DbSet<Combo> Combos { get; set; }

    public virtual DbSet<CondicionesIva> CondicionesIvas { get; set; }

    public virtual DbSet<Contacto> Contactos { get; set; }

    public virtual DbSet<DetalleFactura> DetalleFacturas { get; set; }

    public virtual DbSet<DiasPromocione> DiasPromociones { get; set; }

    public virtual DbSet<Direccione> Direcciones { get; set; }

    public virtual DbSet<Directore> Directores { get; set; }

    public virtual DbSet<Distribuidore> Distribuidores { get; set; }

    public virtual DbSet<Entrada> Entradas { get; set; }

    public virtual DbSet<EstadosButaca> EstadosButacas { get; set; }

    public virtual DbSet<EstadosCompra> EstadosCompras { get; set; }

    public virtual DbSet<EstadosPelicula> EstadosPeliculas { get; set; }

    public virtual DbSet<EstadosPromocione> EstadosPromociones { get; set; }

    public virtual DbSet<EstadosReserva> EstadosReservas { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<FacturasFormasPago> FacturasFormasPagos { get; set; }

    public virtual DbSet<Financiera> Financieras { get; set; }

    public virtual DbSet<FinancierasFormasPago> FinancierasFormasPagos { get; set; }

    public virtual DbSet<FormasPago> FormasPagos { get; set; }

    public virtual DbSet<FormatosPelicula> FormatosPeliculas { get; set; }

    public virtual DbSet<Funcione> Funciones { get; set; }

    public virtual DbSet<Genero> Generos { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<Idioma> Idiomas { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<Paise> Paises { get; set; }

    public virtual DbSet<Pelicula> Peliculas { get; set; }

    public virtual DbSet<PeliculasActore> PeliculasActores { get; set; }

    public virtual DbSet<PeliculasGenero> PeliculasGeneros { get; set; }

    public virtual DbSet<PeliculasIdioma> PeliculasIdiomas { get; set; }

    public virtual DbSet<PreferenciasCliente> PreferenciasClientes { get; set; }

    public virtual DbSet<ProductosCombo> ProductosCombos { get; set; }

    public virtual DbSet<ProductosShop> ProductosShops { get; set; }

    public virtual DbSet<Promocione> Promociones { get; set; }

    public virtual DbSet<PromocionesArticulo> PromocionesArticulos { get; set; }

    public virtual DbSet<PromocionesFuncione> PromocionesFunciones { get; set; }

    public virtual DbSet<PromocionesHorario> PromocionesHorarios { get; set; }

    public virtual DbSet<Provincia> Provincias { get; set; }

    public virtual DbSet<Puntuacione> Puntuaciones { get; set; }

    public virtual DbSet<ReseniasPelicula> ReseniasPeliculas { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<ReservaDetalle> ReservaDetalles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sala> Salas { get; set; }

    public virtual DbSet<Sucursale> Sucursales { get; set; }

    public virtual DbSet<TarifasEntrada> TarifasEntradas { get; set; }

    public virtual DbSet<TiposButaca> TiposButacas { get; set; }

    public virtual DbSet<TiposCombo> TiposCombos { get; set; }

    public virtual DbSet<TiposContacto> TiposContactos { get; set; }

    public virtual DbSet<TiposDocumento> TiposDocumentos { get; set; }

    public virtual DbSet<TiposProducto> TiposProductos { get; set; }

    public virtual DbSet<TiposPromocione> TiposPromociones { get; set; }

    public virtual DbSet<TiposSala> TiposSalas { get; set; }

    public virtual DbSet<Turno> Turnos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<VwFacturasResuman> VwFacturasResumen { get; set; }

    public virtual DbSet<VwFacturasTotal> VwFacturasTotals { get; set; }

    public virtual DbSet<VwFacturasTotalPagado> VwFacturasTotalPagados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actore>(entity =>
        {
            entity.HasKey(e => e.IdActor);

            entity.ToTable("actores");

            entity.Property(e => e.IdActor).HasColumnName("id_actor");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Biografia)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("biografia");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<AdicionalesButaca>(entity =>
        {
            entity.HasKey(e => e.IdTipoButacas);

            entity.ToTable("adicionales_butacas");

            entity.Property(e => e.IdTipoButacas)
                .ValueGeneratedNever()
                .HasColumnName("id_tipo_butacas");
            entity.Property(e => e.Adicional)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("adicional");

            entity.HasOne(d => d.IdTipoButacasNavigation).WithOne(p => p.AdicionalesButaca)
                .HasForeignKey<AdicionalesButaca>(d => d.IdTipoButacas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_adicionales_butacas_tipos");
        });

        modelBuilder.Entity<Barrio>(entity =>
        {
            entity.HasKey(e => e.IdBarrio);

            entity.ToTable("barrios");

            entity.HasIndex(e => new { e.Barrio1, e.IdCiudad }, "UQ_barrios").IsUnique();

            entity.HasIndex(e => new { e.IdCiudad, e.Barrio1 }, "UQ_barrios_ciudad").IsUnique();

            entity.Property(e => e.IdBarrio).HasColumnName("id_barrio");
            entity.Property(e => e.Barrio1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("barrio");
            entity.Property(e => e.IdCiudad).HasColumnName("id_ciudad");

            entity.HasOne(d => d.IdCiudadNavigation).WithMany(p => p.Barrios)
                .HasForeignKey(d => d.IdCiudad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_barrios_ciudades");
        });

        modelBuilder.Entity<Butaca>(entity =>
        {
            entity.HasKey(e => e.IdButaca);

            entity.ToTable("butacas");

            entity.HasIndex(e => new { e.IdSala, e.NroButaca }, "UQ_butacas_sala_numero").IsUnique();

            entity.Property(e => e.IdButaca).HasColumnName("id_butaca");
            entity.Property(e => e.IdEstadoButaca).HasColumnName("id_estado_butaca");
            entity.Property(e => e.IdSala).HasColumnName("id_sala");
            entity.Property(e => e.IdTipoButacas).HasColumnName("id_tipo_butacas");
            entity.Property(e => e.NroButaca).HasColumnName("nro_butaca");

            entity.HasOne(d => d.IdEstadoButacaNavigation).WithMany(p => p.Butacas)
                .HasForeignKey(d => d.IdEstadoButaca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_butacas_estados_butacas");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Butacas)
                .HasForeignKey(d => d.IdSala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_butacas_salas");

            entity.HasOne(d => d.IdTipoButacasNavigation).WithMany(p => p.Butacas)
                .HasForeignKey(d => d.IdTipoButacas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_butacas_tipos_butacas");
        });

        modelBuilder.Entity<Cajero>(entity =>
        {
            entity.HasKey(e => e.IdCajero);

            entity.ToTable("cajeros");

            entity.HasIndex(e => new { e.IdCajero, e.IdSucursal }, "UQ_cajeros_id_cajero_sucursal").IsUnique();

            entity.Property(e => e.IdCajero).HasColumnName("id_cajero");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.IdTurno).HasColumnName("id_turno");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Cajeros)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cajeros_sucursales");

            entity.HasOne(d => d.IdTurnoNavigation).WithMany(p => p.Cajeros)
                .HasForeignKey(d => d.IdTurno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_cajeros_turnos");
        });

        modelBuilder.Entity<CanalesVenta>(entity =>
        {
            entity.HasKey(e => e.IdCanalVenta).HasName("PK__canales___2222B13AA2C5773F");

            entity.ToTable("canales_ventas");

            entity.HasIndex(e => e.CanalVenta, "UQ__canales___3000E15F68B2890E").IsUnique();

            entity.Property(e => e.IdCanalVenta).HasColumnName("id_canal_venta");
            entity.Property(e => e.CanalVenta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("canal_venta");
        });

        modelBuilder.Entity<Ciudade>(entity =>
        {
            entity.HasKey(e => e.IdCiudad);

            entity.ToTable("ciudades");

            entity.HasIndex(e => new { e.Ciudad, e.IdProvincia }, "UQ_ciudades").IsUnique();

            entity.HasIndex(e => new { e.IdProvincia, e.Ciudad }, "UQ_ciudades_provincia").IsUnique();

            entity.Property(e => e.IdCiudad).HasColumnName("id_ciudad");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ciudad");
            entity.Property(e => e.IdProvincia).HasColumnName("id_provincia");

            entity.HasOne(d => d.IdProvinciaNavigation).WithMany(p => p.Ciudades)
                .HasForeignKey(d => d.IdProvincia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ciudades_provincias");
        });

        modelBuilder.Entity<Clasificacione>(entity =>
        {
            entity.HasKey(e => e.IdClasificacion).HasName("PK__clasific__257A4E1B21CD134C");

            entity.ToTable("clasificaciones");

            entity.HasIndex(e => e.Clasificacion, "UQ__clasific__CC7A2CF5624AA59F").IsUnique();

            entity.Property(e => e.IdClasificacion).HasColumnName("id_clasificacion");
            entity.Property(e => e.Clasificacion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("clasificacion");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.CodCliente);

            entity.ToTable("clientes", tb => tb.HasTrigger("TR_CLIENTES_VALIDAR_ROL"));

            entity.HasIndex(e => new { e.IdTipoDoc, e.NroDoc }, "UQ_clientes_documento").IsUnique();

            entity.HasIndex(e => e.IdUsuario, "UQ_clientes_usuario").IsUnique();

            entity.Property(e => e.CodCliente).HasColumnName("cod_cliente");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.IdDireccion).HasColumnName("id_direccion");
            entity.Property(e => e.IdTipoDoc).HasColumnName("id_tipo_doc");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.NroDoc)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nro_doc");

            entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdDireccion)
                .HasConstraintName("FK_clientes_direcciones");

            entity.HasOne(d => d.IdTipoDocNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdTipoDoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_clientes_tipos_documentos");

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.Cliente)
                .HasForeignKey<Cliente>(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_clientes_usuarios");
        });

        modelBuilder.Entity<ClientesContacto>(entity =>
        {
            entity.HasKey(e => e.IdClientesContacto);

            entity.ToTable("clientes_contactos");

            entity.HasIndex(e => new { e.IdContacto, e.CodCliente }, "UQ_clientes_contactos").IsUnique();

            entity.Property(e => e.IdClientesContacto).HasColumnName("id_clientes_contacto");
            entity.Property(e => e.CodCliente).HasColumnName("cod_cliente");
            entity.Property(e => e.IdContacto).HasColumnName("id_contacto");

            entity.HasOne(d => d.CodClienteNavigation).WithMany(p => p.ClientesContactos)
                .HasForeignKey(d => d.CodCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_clientes_contactos_clientes");

            entity.HasOne(d => d.IdContactoNavigation).WithMany(p => p.ClientesContactos)
                .HasForeignKey(d => d.IdContacto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_clientes_contactos_contactos");
        });

        modelBuilder.Entity<Combo>(entity =>
        {
            entity.HasKey(e => e.IdCombo);

            entity.ToTable("combos", tb => tb.HasTrigger("TR_COMBOS_VALIDAR_FACTURACION"));

            entity.Property(e => e.IdCombo).HasColumnName("id_combo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdTipoCombo).HasColumnName("id_tipo_combo");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");

            entity.HasOne(d => d.IdTipoComboNavigation).WithMany(p => p.Combos)
                .HasForeignKey(d => d.IdTipoCombo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_combos_tipos_combos");
        });

        modelBuilder.Entity<CondicionesIva>(entity =>
        {
            entity.HasKey(e => e.IdCondicionIva).HasName("PK__condicio__F9BF0278510C2816");

            entity.ToTable("condiciones_IVA");

            entity.HasIndex(e => e.Condicion, "UQ__condicio__ADBD20F51EDEA824").IsUnique();

            entity.Property(e => e.IdCondicionIva).HasColumnName("id_condicion_IVA");
            entity.Property(e => e.Condicion)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("condicion");
        });

        modelBuilder.Entity<Contacto>(entity =>
        {
            entity.HasKey(e => e.IdContacto);

            entity.ToTable("contactos");

            entity.HasIndex(e => new { e.IdTipoContacto, e.Contacto1 }, "UQ_contactos_tipo_contacto").IsUnique();

            entity.Property(e => e.IdContacto).HasColumnName("id_contacto");
            entity.Property(e => e.Contacto1)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("contacto");
            entity.Property(e => e.IdTipoContacto).HasColumnName("id_tipo_contacto");

            entity.HasOne(d => d.IdTipoContactoNavigation).WithMany(p => p.Contactos)
                .HasForeignKey(d => d.IdTipoContacto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_contactos_tipos_contactos");
        });

        modelBuilder.Entity<DetalleFactura>(entity =>
        {
            entity.HasKey(e => e.IdDetalleFactura);

            entity.ToTable("detalle_facturas", tb => tb.HasTrigger("TR_DETALLE_FACTURAS_VALIDAR_ESTADO"));

            entity.HasIndex(e => e.IdEntrada, "IX_detalle_facturas_entrada_unica")
                .IsUnique()
                .HasFilter("([id_entrada] IS NOT NULL)");

            entity.Property(e => e.IdDetalleFactura).HasColumnName("id_detalle_factura");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.DescuentoUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("descuento_unitario");
            entity.Property(e => e.IdCombo).HasColumnName("id_combo");
            entity.Property(e => e.IdEntrada).HasColumnName("id_entrada");
            entity.Property(e => e.IdProductoShop).HasColumnName("id_producto_shop");
            entity.Property(e => e.IdPromocion).HasColumnName("id_promocion");
            entity.Property(e => e.NroFactura).HasColumnName("nro_factura");
            entity.Property(e => e.PrecioFinalUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_final_unitario");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_unitario");

            entity.HasOne(d => d.IdComboNavigation).WithMany(p => p.DetalleFacturas)
                .HasForeignKey(d => d.IdCombo)
                .HasConstraintName("FK_detalle_facturas_combos");

            entity.HasOne(d => d.IdEntradaNavigation).WithOne(p => p.DetalleFactura)
                .HasForeignKey<DetalleFactura>(d => d.IdEntrada)
                .HasConstraintName("FK_detalle_facturas_entradas");

            entity.HasOne(d => d.IdProductoShopNavigation).WithMany(p => p.DetalleFacturas)
                .HasForeignKey(d => d.IdProductoShop)
                .HasConstraintName("FK_detalle_facturas_productos_shop");

            entity.HasOne(d => d.IdPromocionNavigation).WithMany(p => p.DetalleFacturas)
                .HasForeignKey(d => d.IdPromocion)
                .HasConstraintName("FK_detalle_facturas_promociones");

            entity.HasOne(d => d.NroFacturaNavigation).WithMany(p => p.DetalleFacturas)
                .HasForeignKey(d => d.NroFactura)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_detalle_facturas_facturas");
        });

        modelBuilder.Entity<DiasPromocione>(entity =>
        {
            entity.HasKey(e => e.IdDiaPromocion).HasName("PK__dias_pro__01263811287E6039");

            entity.ToTable("dias_promociones");

            entity.HasIndex(e => e.DiasPromocion, "UQ__dias_pro__D71CEADDC36CCBDE").IsUnique();

            entity.Property(e => e.IdDiaPromocion).HasColumnName("id_dia_promocion");
            entity.Property(e => e.DiasPromocion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dias_promocion");
        });

        modelBuilder.Entity<Direccione>(entity =>
        {
            entity.HasKey(e => e.IdDireccion);

            entity.ToTable("direcciones");

            entity.Property(e => e.IdDireccion).HasColumnName("id_direccion");
            entity.Property(e => e.Altura).HasColumnName("altura");
            entity.Property(e => e.Calle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("calle");
            entity.Property(e => e.Depto)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("depto");
            entity.Property(e => e.IdBarrio).HasColumnName("id_barrio");
            entity.Property(e => e.Piso)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("piso");

            entity.HasOne(d => d.IdBarrioNavigation).WithMany(p => p.Direcciones)
                .HasForeignKey(d => d.IdBarrio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_direcciones_barrios");
        });

        modelBuilder.Entity<Directore>(entity =>
        {
            entity.HasKey(e => e.IdDirector);

            entity.ToTable("directores");

            entity.Property(e => e.IdDirector).HasColumnName("id_director");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("apellido");
            entity.Property(e => e.Bio)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("bio");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Distribuidore>(entity =>
        {
            entity.HasKey(e => e.IdDistribuidor).HasName("PK__distribu__BE73FD5B7236B006");

            entity.ToTable("distribuidores");

            entity.HasIndex(e => e.Distribuidor, "UQ__distribu__183B36A681009508").IsUnique();

            entity.Property(e => e.IdDistribuidor).HasColumnName("id_distribuidor");
            entity.Property(e => e.Distribuidor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("distribuidor");
        });

        modelBuilder.Entity<Entrada>(entity =>
        {
            entity.HasKey(e => e.IdEntrada);

            entity.ToTable("entradas", tb =>
                {
                    tb.HasTrigger("TR_ENTRADAS_VALIDAR_FACTURACION");
                    tb.HasTrigger("TR_ENTRADAS_VALIDAR_SALA");
                });

            entity.HasIndex(e => new { e.IdButaca, e.IdFuncion }, "IX_entradas_butaca_funcion");

            entity.HasIndex(e => new { e.IdFuncion, e.IdButaca }, "UQ_entradas_funcion_butaca").IsUnique();

            entity.Property(e => e.IdEntrada).HasColumnName("id_entrada");
            entity.Property(e => e.IdButaca).HasColumnName("id_butaca");
            entity.Property(e => e.IdFuncion).HasColumnName("id_funcion");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");

            entity.HasOne(d => d.IdButacaNavigation).WithMany(p => p.Entrada)
                .HasForeignKey(d => d.IdButaca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_entradas_butacas");

            entity.HasOne(d => d.IdFuncionNavigation).WithMany(p => p.Entrada)
                .HasForeignKey(d => d.IdFuncion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_entradas_funciones");
        });

        modelBuilder.Entity<EstadosButaca>(entity =>
        {
            entity.HasKey(e => e.IdEstadoButaca).HasName("PK__estados___3846DD23BD9F7288");

            entity.ToTable("estados_butacas");

            entity.HasIndex(e => e.EstadoButaca, "UQ__estados___C59AA3A3B1F14A6D").IsUnique();

            entity.Property(e => e.IdEstadoButaca).HasColumnName("id_estado_butaca");
            entity.Property(e => e.EstadoButaca)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estado_butaca");
        });

        modelBuilder.Entity<EstadosCompra>(entity =>
        {
            entity.HasKey(e => e.IdEstadoCompra).HasName("PK__estados___0AB2E33F0F675F5A");

            entity.ToTable("estados_compras");

            entity.HasIndex(e => e.Estado, "UQ__estados___40237F16EFA423C4").IsUnique();

            entity.Property(e => e.IdEstadoCompra).HasColumnName("id_estado_compra");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado");
        });

        modelBuilder.Entity<EstadosPelicula>(entity =>
        {
            entity.HasKey(e => e.IdEstadoPelicula);

            entity.ToTable("estados_peliculas");

            entity.HasIndex(e => e.Estado).IsUnique();

            entity.Property(e => e.IdEstadoPelicula).HasColumnName("id_estado_pelicula");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado");
        });

        modelBuilder.Entity<EstadosPromocione>(entity =>
        {
            entity.HasKey(e => e.IdEstadoPromocion).HasName("PK__estados___7D52EFBF4ED6881C");

            entity.ToTable("estados_promociones");

            entity.HasIndex(e => e.Estado, "UQ__estados___40237F16FE8DF6C7").IsUnique();

            entity.Property(e => e.IdEstadoPromocion).HasColumnName("id_estado_promocion");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado");
        });

        modelBuilder.Entity<EstadosReserva>(entity =>
        {
            entity.HasKey(e => e.IdEstadoReserva).HasName("PK__estados___D1D0D7E00AF7BD6C");

            entity.ToTable("estados_reservas");

            entity.HasIndex(e => e.Estado, "UQ__estados___40237F168D90EA93").IsUnique();

            entity.Property(e => e.IdEstadoReserva).HasColumnName("id_estado_reserva");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.NroFactura);

            entity.ToTable("facturas", tb => tb.HasTrigger("TR_FACTURAS_VALIDAR_TRANSICION_ESTADO"));

            entity.Property(e => e.NroFactura).HasColumnName("nro_factura");
            entity.Property(e => e.CodCliente).HasColumnName("cod_cliente");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha");
            entity.Property(e => e.IdCajero).HasColumnName("id_cajero");
            entity.Property(e => e.IdCanalVenta).HasColumnName("id_canal_venta");
            entity.Property(e => e.IdCondicionIva).HasColumnName("id_condicion_IVA");
            entity.Property(e => e.IdEstadoCompra).HasColumnName("id_estado_compra");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");

            entity.HasOne(d => d.CodClienteNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.CodCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_facturas_clientes");

            entity.HasOne(d => d.IdCanalVentaNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdCanalVenta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_facturas_canales_ventas");

            entity.HasOne(d => d.IdCondicionIvaNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdCondicionIva)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_facturas_condiciones_IVA");

            entity.HasOne(d => d.IdEstadoCompraNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdEstadoCompra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_facturas_estados_compras");

            entity.HasOne(d => d.Cajero).WithMany(p => p.Facturas)
                .HasPrincipalKey(p => new { p.IdCajero, p.IdSucursal })
                .HasForeignKey(d => new { d.IdCajero, d.IdSucursal })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_facturas_cajero_sucursal");
        });

        modelBuilder.Entity<FacturasFormasPago>(entity =>
        {
            entity.HasKey(e => e.IdFacturaFormaPago);

            entity.ToTable("facturas_formas_pagos", tb =>
                {
                    tb.HasTrigger("TR_FACTURAS_FORMAS_PAGOS_VALIDAR_ESTADO");
                    tb.HasTrigger("TR_FACTURAS_FORMAS_PAGOS_VALIDAR_FINANCIERA");
                });

            entity.HasIndex(e => new { e.NroFactura, e.IdFormaPago, e.IdFinanciera }, "UQ_facturas_formas_pagos").IsUnique();

            entity.Property(e => e.IdFacturaFormaPago).HasColumnName("id_factura_forma_pago");
            entity.Property(e => e.IdFinanciera).HasColumnName("id_financiera");
            entity.Property(e => e.IdFormaPago).HasColumnName("id_forma_pago");
            entity.Property(e => e.MontoPagado)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("monto_pagado");
            entity.Property(e => e.NroFactura).HasColumnName("nro_factura");

            entity.HasOne(d => d.IdFinancieraNavigation).WithMany(p => p.FacturasFormasPagos)
                .HasForeignKey(d => d.IdFinanciera)
                .HasConstraintName("FK_facturas_formas_pagos_financieras");

            entity.HasOne(d => d.IdFormaPagoNavigation).WithMany(p => p.FacturasFormasPagos)
                .HasForeignKey(d => d.IdFormaPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_facturas_formas_pagos_formas_pagos");

            entity.HasOne(d => d.NroFacturaNavigation).WithMany(p => p.FacturasFormasPagos)
                .HasForeignKey(d => d.NroFactura)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_facturas_formas_pagos_facturas");
        });

        modelBuilder.Entity<Financiera>(entity =>
        {
            entity.HasKey(e => e.IdFinanciera).HasName("PK__financie__B6AE05DA6438B666");

            entity.ToTable("financieras");

            entity.HasIndex(e => e.Financiera1, "UQ__financie__43CEBAAB90E1D877").IsUnique();

            entity.Property(e => e.IdFinanciera).HasColumnName("id_financiera");
            entity.Property(e => e.Financiera1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("financiera");
        });

        modelBuilder.Entity<FinancierasFormasPago>(entity =>
        {
            entity.HasKey(e => e.IdFinancieraFormaPago);

            entity.ToTable("financieras_formas_pagos");

            entity.HasIndex(e => new { e.IdFinanciera, e.IdFormaPago }, "UQ_financieras_formas_pagos").IsUnique();

            entity.Property(e => e.IdFinancieraFormaPago).HasColumnName("id_financiera_forma_pago");
            entity.Property(e => e.IdFinanciera).HasColumnName("id_financiera");
            entity.Property(e => e.IdFormaPago).HasColumnName("id_forma_pago");

            entity.HasOne(d => d.IdFinancieraNavigation).WithMany(p => p.FinancierasFormasPagos)
                .HasForeignKey(d => d.IdFinanciera)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_financieras_formas_pagos_financieras");

            entity.HasOne(d => d.IdFormaPagoNavigation).WithMany(p => p.FinancierasFormasPagos)
                .HasForeignKey(d => d.IdFormaPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_financieras_formas_pagos_formas");
        });

        modelBuilder.Entity<FormasPago>(entity =>
        {
            entity.HasKey(e => e.IdFormaPago).HasName("PK__formas_p__DA9B39EEDAB5FFBE");

            entity.ToTable("formas_pagos");

            entity.HasIndex(e => e.FormaPago, "UQ__formas_p__C2F904FE75947992").IsUnique();

            entity.Property(e => e.IdFormaPago).HasColumnName("id_forma_pago");
            entity.Property(e => e.FormaPago)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("forma_pago");
        });

        modelBuilder.Entity<FormatosPelicula>(entity =>
        {
            entity.HasKey(e => e.IdFormatoPelicula).HasName("PK__formatos__2F896B8F12257883");

            entity.ToTable("formatos_peliculas");

            entity.HasIndex(e => e.Formato, "UQ__formatos__7E6E547580E46234").IsUnique();

            entity.Property(e => e.IdFormatoPelicula).HasColumnName("id_formato_pelicula");
            entity.Property(e => e.Formato)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("formato");
        });

        modelBuilder.Entity<Funcione>(entity =>
        {
            entity.HasKey(e => e.IdFuncion);

            entity.ToTable("funciones");

            entity.HasIndex(e => new { e.IdSala, e.Fecha, e.IdHorario }, "UQ_funciones_sala_fecha_horario").IsUnique();

            entity.Property(e => e.IdFuncion).HasColumnName("id_funcion");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.IdHorario).HasColumnName("id_horario");
            entity.Property(e => e.IdPelicula).HasColumnName("id_pelicula");
            entity.Property(e => e.IdSala).HasColumnName("id_sala");

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.Funciones)
                .HasForeignKey(d => d.IdHorario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_funciones_horarios");

            entity.HasOne(d => d.IdPeliculaNavigation).WithMany(p => p.Funciones)
                .HasForeignKey(d => d.IdPelicula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_funciones_peliculas");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Funciones)
                .HasForeignKey(d => d.IdSala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_funciones_salas");
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.HasKey(e => e.IdGenero).HasName("PK__generos__99A8E4F9E0739B64");

            entity.ToTable("generos");

            entity.HasIndex(e => e.Genero1, "UQ__generos__8C6B39E72593C3D6").IsUnique();

            entity.Property(e => e.IdGenero).HasColumnName("id_genero");
            entity.Property(e => e.Genero1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("genero");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario).HasName("PK__horarios__C5836D69716AEA1B");

            entity.ToTable("horarios");

            entity.HasIndex(e => e.Horario1, "UQ__horarios__AD3C2FAC1A86B3F0").IsUnique();

            entity.Property(e => e.IdHorario).HasColumnName("id_horario");
            entity.Property(e => e.Horario1).HasColumnName("horario");
        });

        modelBuilder.Entity<Idioma>(entity =>
        {
            entity.HasKey(e => e.IdIdioma).HasName("PK__idiomas__0BA1525F3E499ED9");

            entity.ToTable("idiomas");

            entity.HasIndex(e => e.Idioma1, "UQ__idiomas__8620FE803620EAD1").IsUnique();

            entity.Property(e => e.IdIdioma).HasColumnName("id_idioma");
            entity.Property(e => e.Idioma1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idioma");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.IdMarca).HasName("PK__marcas__7E43E99E7375EA52");

            entity.ToTable("marcas");

            entity.HasIndex(e => e.Marca1, "UQ__marcas__0B62C6F2CDFB3066").IsUnique();

            entity.Property(e => e.IdMarca).HasColumnName("id_marca");
            entity.Property(e => e.Marca1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("marca");
        });

        modelBuilder.Entity<Paise>(entity =>
        {
            entity.HasKey(e => e.IdPais).HasName("PK__paises__0941A3A763C952AA");

            entity.ToTable("paises");

            entity.HasIndex(e => e.Pais, "UQ__paises__833F4FBFCB3B39E4").IsUnique();

            entity.HasIndex(e => e.Pais, "UQ_paises_pais").IsUnique();

            entity.Property(e => e.IdPais).HasColumnName("id_pais");
            entity.Property(e => e.Pais)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("pais");
        });

        modelBuilder.Entity<Pelicula>(entity =>
        {
            entity.HasKey(e => e.IdPelicula);

            entity.ToTable("peliculas");

            entity.Property(e => e.IdPelicula).HasColumnName("id_pelicula");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaLanzamiento).HasColumnName("fecha_lanzamiento");
            entity.Property(e => e.IdClasificacion).HasColumnName("id_clasificacion");
            entity.Property(e => e.IdDirector).HasColumnName("id_director");
            entity.Property(e => e.IdDistribuidor).HasColumnName("id_distribuidor");
            entity.Property(e => e.IdEstadoPelicula)
                .HasDefaultValue(1)
                .HasColumnName("id_estado_pelicula");
            entity.Property(e => e.IdFormatoPelicula).HasColumnName("id_formato_pelicula");
            entity.Property(e => e.IdPais).HasColumnName("id_pais");
            entity.Property(e => e.Titulo)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("titulo");

            entity.HasOne(d => d.IdClasificacionNavigation).WithMany(p => p.Peliculas)
                .HasForeignKey(d => d.IdClasificacion)
                .HasConstraintName("FK_peliculas_clasificaciones");

            entity.HasOne(d => d.IdDirectorNavigation).WithMany(p => p.Peliculas)
                .HasForeignKey(d => d.IdDirector)
                .HasConstraintName("FK_peliculas_directores");

            entity.HasOne(d => d.IdDistribuidorNavigation).WithMany(p => p.Peliculas)
                .HasForeignKey(d => d.IdDistribuidor)
                .HasConstraintName("FK_peliculas_distribuidores");

            entity.HasOne(d => d.IdEstadoPeliculaNavigation).WithMany(p => p.Peliculas)
                .HasForeignKey(d => d.IdEstadoPelicula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_peliculas_estados_peliculas");

            entity.HasOne(d => d.IdFormatoPeliculaNavigation).WithMany(p => p.Peliculas)
                .HasForeignKey(d => d.IdFormatoPelicula)
                .HasConstraintName("FK_peliculas_formatos");

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.Peliculas)
                .HasForeignKey(d => d.IdPais)
                .HasConstraintName("FK_peliculas_paises");
        });

        modelBuilder.Entity<PeliculasActore>(entity =>
        {
            entity.HasKey(e => e.IdPeliculaActor);

            entity.ToTable("peliculas_actores");

            entity.HasIndex(e => new { e.IdPelicula, e.IdActor }, "UQ_peliculas_actores").IsUnique();

            entity.Property(e => e.IdPeliculaActor).HasColumnName("id_pelicula_actor");
            entity.Property(e => e.IdActor).HasColumnName("id_actor");
            entity.Property(e => e.IdPelicula).HasColumnName("id_pelicula");

            entity.HasOne(d => d.IdActorNavigation).WithMany(p => p.PeliculasActores)
                .HasForeignKey(d => d.IdActor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_peliculas_actores_actores");

            entity.HasOne(d => d.IdPeliculaNavigation).WithMany(p => p.PeliculasActores)
                .HasForeignKey(d => d.IdPelicula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_peliculas_actores_peliculas");
        });

        modelBuilder.Entity<PeliculasGenero>(entity =>
        {
            entity.HasKey(e => e.IdPeliculaGenero);

            entity.ToTable("peliculas_generos");

            entity.HasIndex(e => new { e.IdPelicula, e.IdGenero }, "UQ_peliculas_generos").IsUnique();

            entity.Property(e => e.IdPeliculaGenero).HasColumnName("id_pelicula_genero");
            entity.Property(e => e.IdGenero).HasColumnName("id_genero");
            entity.Property(e => e.IdPelicula).HasColumnName("id_pelicula");

            entity.HasOne(d => d.IdGeneroNavigation).WithMany(p => p.PeliculasGeneros)
                .HasForeignKey(d => d.IdGenero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_peliculas_generos_generos");

            entity.HasOne(d => d.IdPeliculaNavigation).WithMany(p => p.PeliculasGeneros)
                .HasForeignKey(d => d.IdPelicula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_peliculas_generos_peliculas");
        });

        modelBuilder.Entity<PeliculasIdioma>(entity =>
        {
            entity.HasKey(e => e.IdPeliculaIdioma);

            entity.ToTable("peliculas_idiomas");

            entity.HasIndex(e => new { e.IdPelicula, e.IdIdioma }, "UQ_peliculas_idiomas").IsUnique();

            entity.Property(e => e.IdPeliculaIdioma).HasColumnName("id_pelicula_idioma");
            entity.Property(e => e.IdIdioma).HasColumnName("id_idioma");
            entity.Property(e => e.IdPelicula).HasColumnName("id_pelicula");

            entity.HasOne(d => d.IdIdiomaNavigation).WithMany(p => p.PeliculasIdiomas)
                .HasForeignKey(d => d.IdIdioma)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_peliculas_idiomas_idiomas");

            entity.HasOne(d => d.IdPeliculaNavigation).WithMany(p => p.PeliculasIdiomas)
                .HasForeignKey(d => d.IdPelicula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_peliculas_idiomas_peliculas");
        });

        modelBuilder.Entity<PreferenciasCliente>(entity =>
        {
            entity.HasKey(e => e.IdPreferenciaCliente);

            entity.ToTable("preferencias_clientes");

            entity.HasIndex(e => new { e.CodCliente, e.IdGenero }, "UQ_preferencias_cliente_genero").IsUnique();

            entity.Property(e => e.IdPreferenciaCliente).HasColumnName("id_preferencia_cliente");
            entity.Property(e => e.CodCliente).HasColumnName("cod_cliente");
            entity.Property(e => e.IdGenero).HasColumnName("id_genero");
            entity.Property(e => e.SuscribeNewsletter).HasColumnName("suscribe_newsletter");

            entity.HasOne(d => d.CodClienteNavigation).WithMany(p => p.PreferenciasClientes)
                .HasForeignKey(d => d.CodCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_preferencias_clientes_clientes");

            entity.HasOne(d => d.IdGeneroNavigation).WithMany(p => p.PreferenciasClientes)
                .HasForeignKey(d => d.IdGenero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_preferencias_clientes_generos");
        });

        modelBuilder.Entity<ProductosCombo>(entity =>
        {
            entity.HasKey(e => e.IdProductoCombo);

            entity.ToTable("productos_combos", tb => tb.HasTrigger("TR_PRODUCTOS_COMBOS_VALIDAR_FACTURACION"));

            entity.HasIndex(e => new { e.IdProductoShop, e.IdCombo }, "UQ_productos_combos").IsUnique();

            entity.Property(e => e.IdProductoCombo).HasColumnName("id_producto_combo");
            entity.Property(e => e.Cantidad)
                .HasDefaultValue(1)
                .HasColumnName("cantidad");
            entity.Property(e => e.IdCombo).HasColumnName("id_combo");
            entity.Property(e => e.IdProductoShop).HasColumnName("id_producto_shop");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio_unitario");

            entity.HasOne(d => d.IdComboNavigation).WithMany(p => p.ProductosCombos)
                .HasForeignKey(d => d.IdCombo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_productos_combos_combos");

            entity.HasOne(d => d.IdProductoShopNavigation).WithMany(p => p.ProductosCombos)
                .HasForeignKey(d => d.IdProductoShop)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_productos_combos_productos_shop");
        });

        modelBuilder.Entity<ProductosShop>(entity =>
        {
            entity.HasKey(e => e.IdProductoShop);

            entity.ToTable("productos_shop", tb => tb.HasTrigger("TR_PRODUCTOS_SHOP_VALIDAR_FACTURACION"));

            entity.Property(e => e.IdProductoShop).HasColumnName("id_producto_shop");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdMarca).HasColumnName("id_marca");
            entity.Property(e => e.IdTipoProducto).HasColumnName("id_tipo_producto");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.Stock).HasColumnName("stock");

            entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.ProductosShops)
                .HasForeignKey(d => d.IdMarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_productos_shop_marcas");

            entity.HasOne(d => d.IdTipoProductoNavigation).WithMany(p => p.ProductosShops)
                .HasForeignKey(d => d.IdTipoProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_productos_shop_tipos_productos");
        });

        modelBuilder.Entity<Promocione>(entity =>
        {
            entity.HasKey(e => e.IdPromocion);

            entity.ToTable("promociones");

            entity.Property(e => e.IdPromocion).HasColumnName("id_promocion");
            entity.Property(e => e.CodCliente).HasColumnName("cod_cliente");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaFin)
                .HasDefaultValueSql("(dateadd(day,(30),CONVERT([date],getdate())))")
                .HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio)
                .HasDefaultValueSql("(CONVERT([date],getdate()))")
                .HasColumnName("fecha_inicio");
            entity.Property(e => e.IdCanalVenta).HasColumnName("id_canal_venta");
            entity.Property(e => e.IdDiaPromocion).HasColumnName("id_dia_promocion");
            entity.Property(e => e.IdEstadoPromocion).HasColumnName("id_estado_promocion");
            entity.Property(e => e.IdFinanciera).HasColumnName("id_financiera");
            entity.Property(e => e.IdTipoPromocion).HasColumnName("id_tipo_promocion");
            entity.Property(e => e.MontoMinimo)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("monto_minimo");
            entity.Property(e => e.PorcentajeDescuento)
                .HasDefaultValue(10.00m)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("porcentaje_descuento");

            entity.HasOne(d => d.CodClienteNavigation).WithMany(p => p.Promociones)
                .HasForeignKey(d => d.CodCliente)
                .HasConstraintName("FK_promociones_clientes");

            entity.HasOne(d => d.IdCanalVentaNavigation).WithMany(p => p.Promociones)
                .HasForeignKey(d => d.IdCanalVenta)
                .HasConstraintName("FK_promociones_canales_ventas");

            entity.HasOne(d => d.IdDiaPromocionNavigation).WithMany(p => p.Promociones)
                .HasForeignKey(d => d.IdDiaPromocion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_promociones_dias_promociones");

            entity.HasOne(d => d.IdEstadoPromocionNavigation).WithMany(p => p.Promociones)
                .HasForeignKey(d => d.IdEstadoPromocion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_promociones_estados_promociones");

            entity.HasOne(d => d.IdFinancieraNavigation).WithMany(p => p.Promociones)
                .HasForeignKey(d => d.IdFinanciera)
                .HasConstraintName("FK_promociones_financieras");

            entity.HasOne(d => d.IdTipoPromocionNavigation).WithMany(p => p.Promociones)
                .HasForeignKey(d => d.IdTipoPromocion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_promociones_tipos_promociones");

            entity.HasMany(d => d.IdTipoCombos).WithMany(p => p.IdPromocions)
                .UsingEntity<Dictionary<string, object>>(
                    "PromocionesTiposCombo",
                    r => r.HasOne<TiposCombo>().WithMany()
                        .HasForeignKey("IdTipoCombo")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_promociones_tipos_combos_tipos"),
                    l => l.HasOne<Promocione>().WithMany()
                        .HasForeignKey("IdPromocion")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_promociones_tipos_combos_promociones"),
                    j =>
                    {
                        j.HasKey("IdPromocion", "IdTipoCombo");
                        j.ToTable("promociones_tipos_combos");
                        j.IndexerProperty<int>("IdPromocion").HasColumnName("id_promocion");
                        j.IndexerProperty<int>("IdTipoCombo").HasColumnName("id_tipo_combo");
                    });

            entity.HasMany(d => d.IdTipoProductos).WithMany(p => p.IdPromocions)
                .UsingEntity<Dictionary<string, object>>(
                    "PromocionesTiposProducto",
                    r => r.HasOne<TiposProducto>().WithMany()
                        .HasForeignKey("IdTipoProducto")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_promociones_tipos_productos_tipos"),
                    l => l.HasOne<Promocione>().WithMany()
                        .HasForeignKey("IdPromocion")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_promociones_tipos_productos_promociones"),
                    j =>
                    {
                        j.HasKey("IdPromocion", "IdTipoProducto");
                        j.ToTable("promociones_tipos_productos");
                        j.IndexerProperty<int>("IdPromocion").HasColumnName("id_promocion");
                        j.IndexerProperty<int>("IdTipoProducto").HasColumnName("id_tipo_producto");
                    });
        });

        modelBuilder.Entity<PromocionesArticulo>(entity =>
        {
            entity.HasKey(e => e.IdPromocionArticulo);

            entity.ToTable("promociones_articulos");

            entity.HasIndex(e => new { e.IdPromocion, e.IdCombo }, "UQ_promociones_articulos_combo")
                .IsUnique()
                .HasFilter("([id_combo] IS NOT NULL)");

            entity.HasIndex(e => new { e.IdPromocion, e.IdEntrada }, "UQ_promociones_articulos_entrada")
                .IsUnique()
                .HasFilter("([id_entrada] IS NOT NULL)");

            entity.HasIndex(e => new { e.IdPromocion, e.IdProductoShop }, "UQ_promociones_articulos_producto")
                .IsUnique()
                .HasFilter("([id_producto_shop] IS NOT NULL)");

            entity.Property(e => e.IdPromocionArticulo).HasColumnName("id_promocion_articulo");
            entity.Property(e => e.IdCombo).HasColumnName("id_combo");
            entity.Property(e => e.IdEntrada).HasColumnName("id_entrada");
            entity.Property(e => e.IdProductoShop).HasColumnName("id_producto_shop");
            entity.Property(e => e.IdPromocion).HasColumnName("id_promocion");

            entity.HasOne(d => d.IdComboNavigation).WithMany(p => p.PromocionesArticulos)
                .HasForeignKey(d => d.IdCombo)
                .HasConstraintName("FK_promociones_articulos_combos");

            entity.HasOne(d => d.IdEntradaNavigation).WithMany(p => p.PromocionesArticulos)
                .HasForeignKey(d => d.IdEntrada)
                .HasConstraintName("FK_promociones_articulos_entradas");

            entity.HasOne(d => d.IdProductoShopNavigation).WithMany(p => p.PromocionesArticulos)
                .HasForeignKey(d => d.IdProductoShop)
                .HasConstraintName("FK_promociones_articulos_productos");

            entity.HasOne(d => d.IdPromocionNavigation).WithMany(p => p.PromocionesArticulos)
                .HasForeignKey(d => d.IdPromocion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_promociones_articulos_promociones");
        });

        modelBuilder.Entity<PromocionesFuncione>(entity =>
        {
            entity.HasKey(e => e.IdPromocionFuncion);

            entity.ToTable("promociones_funciones");

            entity.HasIndex(e => new { e.IdPromocion, e.IdFuncion }, "UQ_promociones_funciones").IsUnique();

            entity.Property(e => e.IdPromocionFuncion).HasColumnName("id_promocion_funcion");
            entity.Property(e => e.IdFuncion).HasColumnName("id_funcion");
            entity.Property(e => e.IdPromocion).HasColumnName("id_promocion");

            entity.HasOne(d => d.IdFuncionNavigation).WithMany(p => p.PromocionesFunciones)
                .HasForeignKey(d => d.IdFuncion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_promociones_funciones_funciones");

            entity.HasOne(d => d.IdPromocionNavigation).WithMany(p => p.PromocionesFunciones)
                .HasForeignKey(d => d.IdPromocion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_promociones_funciones_promociones");
        });

        modelBuilder.Entity<PromocionesHorario>(entity =>
        {
            entity.HasKey(e => e.IdPromocionHorario);

            entity.ToTable("promociones_horarios");

            entity.HasIndex(e => new { e.IdPromocion, e.IdHorario }, "UQ_promociones_horarios").IsUnique();

            entity.Property(e => e.IdPromocionHorario).HasColumnName("id_promocion_horario");
            entity.Property(e => e.IdHorario).HasColumnName("id_horario");
            entity.Property(e => e.IdPromocion).HasColumnName("id_promocion");

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.PromocionesHorarios)
                .HasForeignKey(d => d.IdHorario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_promociones_horarios_horarios");

            entity.HasOne(d => d.IdPromocionNavigation).WithMany(p => p.PromocionesHorarios)
                .HasForeignKey(d => d.IdPromocion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_promociones_horarios_promociones");
        });

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.HasKey(e => e.IdProvincia);

            entity.ToTable("provincias");

            entity.HasIndex(e => new { e.Provincia1, e.IdPais }, "UQ_provincias").IsUnique();

            entity.HasIndex(e => new { e.IdPais, e.Provincia1 }, "UQ_provincias_pais").IsUnique();

            entity.Property(e => e.IdProvincia).HasColumnName("id_provincia");
            entity.Property(e => e.IdPais).HasColumnName("id_pais");
            entity.Property(e => e.Provincia1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("provincia");

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.Provincia)
                .HasForeignKey(d => d.IdPais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_provincias_paises");
        });

        modelBuilder.Entity<Puntuacione>(entity =>
        {
            entity.HasKey(e => e.IdPuntuacion).HasName("PK__puntuaci__1B2258DFA1F4C3D4");

            entity.ToTable("puntuaciones");

            entity.HasIndex(e => e.Puntuacion, "UQ__puntuaci__9613D833D577FD13").IsUnique();

            entity.Property(e => e.IdPuntuacion).HasColumnName("id_puntuacion");
            entity.Property(e => e.Puntuacion).HasColumnName("puntuacion");
        });

        modelBuilder.Entity<ReseniasPelicula>(entity =>
        {
            entity.HasKey(e => e.IdResenia);

            entity.ToTable("resenias_peliculas");

            entity.HasIndex(e => new { e.CodCliente, e.IdPelicula }, "UQ_resenias_cliente_pelicula").IsUnique();

            entity.Property(e => e.IdResenia).HasColumnName("id_resenia");
            entity.Property(e => e.CodCliente).HasColumnName("cod_cliente");
            entity.Property(e => e.Comentario)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("comentario");
            entity.Property(e => e.FechaResenia)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha_resenia");
            entity.Property(e => e.IdPelicula).HasColumnName("id_pelicula");
            entity.Property(e => e.IdPuntuacion).HasColumnName("id_puntuacion");

            entity.HasOne(d => d.CodClienteNavigation).WithMany(p => p.ReseniasPeliculas)
                .HasForeignKey(d => d.CodCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_resenias_peliculas_clientes");

            entity.HasOne(d => d.IdPeliculaNavigation).WithMany(p => p.ReseniasPeliculas)
                .HasForeignKey(d => d.IdPelicula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_resenias_peliculas_peliculas");

            entity.HasOne(d => d.IdPuntuacionNavigation).WithMany(p => p.ReseniasPeliculas)
                .HasForeignKey(d => d.IdPuntuacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_resenias_peliculas_puntuaciones");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva);

            entity.ToTable("reservas", tb =>
                {
                    tb.HasTrigger("TR_RESERVAS_VALIDAR_ESTADO");
                    tb.HasTrigger("TR_RESERVAS_VALIDAR_TRANSICION_ESTADO");
                });

            entity.HasIndex(e => new { e.IdFuncion, e.IdEstadoReserva }, "IX_reservas_funcion_estado");

            entity.Property(e => e.IdReserva).HasColumnName("id_reserva");
            entity.Property(e => e.CodCliente).HasColumnName("cod_cliente");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnName("fecha");
            entity.Property(e => e.IdEstadoReserva).HasColumnName("id_estado_reserva");
            entity.Property(e => e.IdFuncion).HasColumnName("id_funcion");
            entity.Property(e => e.MontoReserva)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("monto_reserva");

            entity.HasOne(d => d.CodClienteNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.CodCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_reservas_clientes");

            entity.HasOne(d => d.IdEstadoReservaNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdEstadoReserva)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_reservas_estados_reservas");

            entity.HasOne(d => d.IdFuncionNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdFuncion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_reservas_funciones");
        });

        modelBuilder.Entity<ReservaDetalle>(entity =>
        {
            entity.HasKey(e => e.IdReservaDetalle);

            entity.ToTable("reserva_detalle", tb => tb.HasTrigger("TR_RESERVA_DETALLE_VALIDAR_SALA"));

            entity.HasIndex(e => new { e.IdButaca, e.IdReserva }, "IX_reserva_detalle_butaca_reserva");

            entity.HasIndex(e => new { e.IdReserva, e.IdButaca }, "UQ_reserva_detalle").IsUnique();

            entity.Property(e => e.IdReservaDetalle).HasColumnName("id_reserva_detalle");
            entity.Property(e => e.IdButaca).HasColumnName("id_butaca");
            entity.Property(e => e.IdReserva).HasColumnName("id_reserva");

            entity.HasOne(d => d.IdButacaNavigation).WithMany(p => p.ReservaDetalles)
                .HasForeignKey(d => d.IdButaca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_reserva_detalle_butacas");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.ReservaDetalles)
                .HasForeignKey(d => d.IdReserva)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_reserva_detalle_reservas");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.ToTable("roles");

            entity.HasIndex(e => e.Descripcion, "UQ_roles_descripcion").IsUnique();

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.HasKey(e => e.IdSala);

            entity.ToTable("salas");

            entity.HasIndex(e => new { e.IdSucursal, e.NroSala }, "UQ_salas_sucursal_numero").IsUnique();

            entity.Property(e => e.IdSala).HasColumnName("id_sala");
            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.IdTipoSala).HasColumnName("id_tipo_sala");
            entity.Property(e => e.NroSala).HasColumnName("nro_sala");

            entity.HasOne(d => d.IdSucursalNavigation).WithMany(p => p.Salas)
                .HasForeignKey(d => d.IdSucursal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_salas_sucursales");

            entity.HasOne(d => d.IdTipoSalaNavigation).WithMany(p => p.Salas)
                .HasForeignKey(d => d.IdTipoSala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_salas_tipos_salas");
        });

        modelBuilder.Entity<Sucursale>(entity =>
        {
            entity.HasKey(e => e.IdSucursal);

            entity.ToTable("sucursales");

            entity.HasIndex(e => e.Nombre, "UQ_sucursales_nombre").IsUnique();

            entity.Property(e => e.IdSucursal).HasColumnName("id_sucursal");
            entity.Property(e => e.IdDireccion).HasColumnName("id_direccion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.Sucursales)
                .HasForeignKey(d => d.IdDireccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_sucursales_direcciones");
        });

        modelBuilder.Entity<TarifasEntrada>(entity =>
        {
            entity.HasKey(e => e.IdTipoSala);

            entity.ToTable("tarifas_entradas");

            entity.Property(e => e.IdTipoSala)
                .ValueGeneratedNever()
                .HasColumnName("id_tipo_sala");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");

            entity.HasOne(d => d.IdTipoSalaNavigation).WithOne(p => p.TarifasEntrada)
                .HasForeignKey<TarifasEntrada>(d => d.IdTipoSala)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tarifas_entradas_tipos_salas");
        });

        modelBuilder.Entity<TiposButaca>(entity =>
        {
            entity.HasKey(e => e.IdTipoButacas).HasName("PK__tipos_bu__156FB4EEE0921DA5");

            entity.ToTable("tipos_butacas");

            entity.HasIndex(e => e.TipoButaca, "UQ__tipos_bu__E2D63AA46C820FB1").IsUnique();

            entity.Property(e => e.IdTipoButacas).HasColumnName("id_tipo_butacas");
            entity.Property(e => e.TipoButaca)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo_butaca");
        });

        modelBuilder.Entity<TiposCombo>(entity =>
        {
            entity.HasKey(e => e.IdTipoCombo).HasName("PK__tipos_co__BEC3D24321DFB4E0");

            entity.ToTable("tipos_combos");

            entity.HasIndex(e => e.Descripcion, "UQ__tipos_co__298336B60A299C15").IsUnique();

            entity.Property(e => e.IdTipoCombo).HasColumnName("id_tipo_combo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<TiposContacto>(entity =>
        {
            entity.HasKey(e => e.IdTipoContacto).HasName("PK__tipos_co__CA854C0D6582767C");

            entity.ToTable("tipos_contactos");

            entity.HasIndex(e => e.Tipo, "UQ__tipos_co__E7F956499A7C022E").IsUnique();

            entity.Property(e => e.IdTipoContacto).HasColumnName("id_tipo_contacto");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<TiposDocumento>(entity =>
        {
            entity.HasKey(e => e.IdTipoDoc).HasName("PK__tipos_do__B0A524EA110F2401");

            entity.ToTable("tipos_documentos");

            entity.HasIndex(e => e.Tipo, "UQ__tipos_do__E7F956492BDFA6DD").IsUnique();

            entity.Property(e => e.IdTipoDoc).HasColumnName("id_tipo_doc");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<TiposProducto>(entity =>
        {
            entity.HasKey(e => e.IdTipoProducto).HasName("PK__tipos_pr__F5E0BFB887D63F9C");

            entity.ToTable("tipos_productos");

            entity.HasIndex(e => e.TipoProducto, "UQ__tipos_pr__23D7BA4FFD7691CB").IsUnique();

            entity.Property(e => e.IdTipoProducto).HasColumnName("id_tipo_producto");
            entity.Property(e => e.TipoProducto)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo_producto");
        });

        modelBuilder.Entity<TiposPromocione>(entity =>
        {
            entity.HasKey(e => e.IdTipoPromocion).HasName("PK__tipos_pr__23C381C06EA1B44A");

            entity.ToTable("tipos_promociones");

            entity.HasIndex(e => e.TipoPromocion, "UQ__tipos_pr__E90839B3344A3BD5").IsUnique();

            entity.Property(e => e.IdTipoPromocion).HasColumnName("id_tipo_promocion");
            entity.Property(e => e.TipoPromocion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo_promocion");
        });

        modelBuilder.Entity<TiposSala>(entity =>
        {
            entity.HasKey(e => e.IdTipoSala).HasName("PK__tipos_sa__1C51F013A364C717");

            entity.ToTable("tipos_salas");

            entity.HasIndex(e => e.TipoSala, "UQ__tipos_sa__5DE807E15767FA8A").IsUnique();

            entity.Property(e => e.IdTipoSala).HasColumnName("id_tipo_sala");
            entity.Property(e => e.TipoSala)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo_sala");
        });

        modelBuilder.Entity<Turno>(entity =>
        {
            entity.HasKey(e => e.IdTurno).HasName("PK__turnos__C68E739781E1C68E");

            entity.ToTable("turnos");

            entity.HasIndex(e => e.Turno1, "UQ__turnos__179B81C31A120742").IsUnique();

            entity.Property(e => e.IdTurno).HasColumnName("id_turno");
            entity.Property(e => e.Turno1)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("turno");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario);

            entity.ToTable("usuarios", tb =>
                {
                    tb.HasTrigger("TR_USUARIOS_VALIDAR_CAMBIO_ROL");
                    tb.HasTrigger("TR_USUARIOS_VALIDAR_IDENTIFICADORES");
                });

            entity.HasIndex(e => e.CorreoElectronico, "IX_usuarios_correo_electronico").IsUnique();

            entity.HasIndex(e => e.NombreUsuario, "UQ_usuarios_nombre").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("contraseña");
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(254)
                .IsUnicode(false)
                .HasColumnName("correo_electronico");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_usuario");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_usuarios_roles");
        });

        modelBuilder.Entity<VwFacturasResuman>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Facturas_Resumen");

            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.NroFactura).HasColumnName("nro_factura");
            entity.Property(e => e.SaldoPendiente)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("saldo_pendiente");
            entity.Property(e => e.TotalFactura)
                .HasColumnType("decimal(38, 4)")
                .HasColumnName("total_factura");
            entity.Property(e => e.TotalPagado)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("total_pagado");
        });

        modelBuilder.Entity<VwFacturasTotal>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Facturas_Total");

            entity.Property(e => e.NroFactura).HasColumnName("nro_factura");
            entity.Property(e => e.TotalFactura)
                .HasColumnType("decimal(38, 4)")
                .HasColumnName("total_factura");
        });

        modelBuilder.Entity<VwFacturasTotalPagado>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Facturas_Total_Pagado");

            entity.Property(e => e.NroFactura).HasColumnName("nro_factura");
            entity.Property(e => e.TotalPagado)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("total_pagado");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
