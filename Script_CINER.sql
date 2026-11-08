
CREATE DATABASE CINE_R
go
USE CINE_R
go
CREATE TABLE dbo.canales_ventas
(
id_canal_venta int identity(1,1) not null primary key,
canal_venta varchar(50) not null unique
);

create table dbo.clasificaciones
(
    id_clasificacion INT IDENTITY(1,1) PRIMARY KEY,
    clasificacion VARCHAR(20) NOT NULL UNIQUE
);
CREATE TABLE dbo.condiciones_IVA
(
    id_condicion_IVA INT IDENTITY(1,1) PRIMARY KEY,
    condicion VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE dbo.dias_promociones
(
    id_dia_promocion INT IDENTITY(1,1) PRIMARY KEY,
    dias_promocion VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE dbo.distribuidores
(
    id_distribuidor INT IDENTITY(1,1) PRIMARY KEY,
    distribuidor VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.estados_butacas
(
    id_estado_butaca INT IDENTITY(1,1) PRIMARY KEY,
    estado_butaca VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.estados_compras
(
    id_estado_compra INT IDENTITY(1,1) PRIMARY KEY,
    estado VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE dbo.estados_promociones
(
    id_estado_promocion INT IDENTITY(1,1) PRIMARY KEY,
    estado VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE dbo.estados_reservas
(
    id_estado_reserva INT IDENTITY(1,1) PRIMARY KEY,
    estado VARCHAR(20) NOT NULL UNIQUE
);

CREATE TABLE dbo.financieras
(
    id_financiera INT IDENTITY(1,1) PRIMARY KEY,
    financiera VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.formas_pagos
(
    id_forma_pago INT IDENTITY(1,1) PRIMARY KEY,
    forma_pago VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.formatos_peliculas
(
    id_formato_pelicula INT IDENTITY(1,1) PRIMARY KEY,
    formato VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.generos
(
    id_genero INT IDENTITY(1,1) PRIMARY KEY,
    genero VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.horarios
(
    id_horario INT IDENTITY(1,1) PRIMARY KEY,
    horario TIME NOT NULL UNIQUE
);

CREATE TABLE dbo.idiomas
(
    id_idioma INT IDENTITY(1,1) PRIMARY KEY,
    idioma VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.marcas
(
    id_marca INT IDENTITY(1,1) PRIMARY KEY,
    marca VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.paises
(
    id_pais INT IDENTITY(1,1) PRIMARY KEY,
    pais VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.puntuaciones
(
    id_puntuacion INT IDENTITY(1,1) PRIMARY KEY,
    puntuacion INT NOT NULL UNIQUE
);

CREATE TABLE dbo.tipos_butacas
(
    id_tipo_butacas INT IDENTITY(1,1) PRIMARY KEY,
    tipo_butaca VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.tipos_combos
(
    id_tipo_combo INT IDENTITY(1,1) PRIMARY KEY,
    descripcion VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.tipos_contactos
(
    id_tipo_contacto INT IDENTITY(1,1) PRIMARY KEY,
    tipo VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.tipos_documentos
(
    id_tipo_doc INT IDENTITY(1,1) PRIMARY KEY,
    tipo VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.tipos_productos
(
    id_tipo_producto INT IDENTITY(1,1) PRIMARY KEY,
    tipo_producto VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.tipos_promociones
(
    id_tipo_promocion INT IDENTITY(1,1) PRIMARY KEY,
    tipo_promocion VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.tipos_salas
(
    id_tipo_sala INT IDENTITY(1,1) PRIMARY KEY,
    tipo_sala VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE dbo.turnos
(
    id_turno INT IDENTITY(1,1) PRIMARY KEY,
    turno VARCHAR(20) NOT NULL UNIQUE
);


CREATE TABLE dbo.actores
(
    id_actor INT IDENTITY(1,1) NOT NULL,
    nombre VARCHAR(50) NOT NULL,
    apellido VARCHAR(50) NOT NULL,
    biografia VARCHAR(500) NULL,

    CONSTRAINT PK_actores
        PRIMARY KEY (id_actor)
);
GO




CREATE TABLE dbo.directores
(
    id_director INT IDENTITY(1,1) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    fecha_nacimiento DATE NULL,
    bio VARCHAR(500) NULL,

    CONSTRAINT PK_directores
        PRIMARY KEY (id_director)
);
GO




CREATE TABLE dbo.peliculas
(
    id_pelicula INT IDENTITY(1,1) NOT NULL,
    titulo VARCHAR(150) NOT NULL,
    descripcion VARCHAR(500) NULL,
    fecha_lanzamiento DATE NOT NULL,
    id_pais INT NULL,
    id_formato_pelicula INT NULL,
    id_clasificacion INT NULL,
    id_distribuidor INT NULL,
    id_director INT NULL,

    CONSTRAINT PK_peliculas
        PRIMARY KEY (id_pelicula),

    CONSTRAINT FK_peliculas_paises
        FOREIGN KEY (id_pais)
        REFERENCES dbo.paises(id_pais),

    CONSTRAINT FK_peliculas_formatos
        FOREIGN KEY (id_formato_pelicula)
        REFERENCES dbo.formatos_peliculas(id_formato_pelicula),

    CONSTRAINT FK_peliculas_clasificaciones
        FOREIGN KEY (id_clasificacion)
        REFERENCES dbo.clasificaciones(id_clasificacion),

    CONSTRAINT FK_peliculas_distribuidores
        FOREIGN KEY (id_distribuidor)
        REFERENCES dbo.distribuidores(id_distribuidor),

    CONSTRAINT FK_peliculas_directores
        FOREIGN KEY (id_director)
        REFERENCES dbo.directores(id_director)
);
GO




CREATE TABLE dbo.peliculas_actores
(
    id_pelicula_actor INT IDENTITY(1,1) NOT NULL,
    id_pelicula INT NOT NULL,
    id_actor INT NOT NULL,

    CONSTRAINT PK_peliculas_actores
        PRIMARY KEY (id_pelicula_actor),

    CONSTRAINT FK_peliculas_actores_peliculas
        FOREIGN KEY (id_pelicula)
        REFERENCES dbo.peliculas(id_pelicula),

    CONSTRAINT FK_peliculas_actores_actores
        FOREIGN KEY (id_actor)
        REFERENCES dbo.actores(id_actor),

    CONSTRAINT UQ_peliculas_actores
        UNIQUE (id_pelicula, id_actor)
);
GO


CREATE TABLE dbo.peliculas_generos
(
    id_pelicula_genero INT IDENTITY(1,1) NOT NULL,
    id_pelicula INT NOT NULL,
    id_genero INT NOT NULL,

    CONSTRAINT PK_peliculas_generos
        PRIMARY KEY (id_pelicula_genero),

    CONSTRAINT FK_peliculas_generos_peliculas
        FOREIGN KEY (id_pelicula)
        REFERENCES dbo.peliculas(id_pelicula),

    CONSTRAINT FK_peliculas_generos_generos
        FOREIGN KEY (id_genero)
        REFERENCES dbo.generos(id_genero),

    CONSTRAINT UQ_peliculas_generos
        UNIQUE (id_pelicula, id_genero)
);
GO

CREATE TABLE dbo.peliculas_idiomas
(
    id_pelicula_idioma INT IDENTITY(1,1) NOT NULL,
    id_pelicula INT NOT NULL,
    id_idioma INT NOT NULL,

    CONSTRAINT PK_peliculas_idiomas
        PRIMARY KEY (id_pelicula_idioma),

    CONSTRAINT FK_peliculas_idiomas_peliculas
        FOREIGN KEY (id_pelicula)
        REFERENCES dbo.peliculas(id_pelicula),

    CONSTRAINT FK_peliculas_idiomas_idiomas
        FOREIGN KEY (id_idioma)
        REFERENCES dbo.idiomas(id_idioma),

    CONSTRAINT UQ_peliculas_idiomas
        UNIQUE (id_pelicula, id_idioma)
);
------------------------------------------
CREATE TABLE dbo.provincias
(
    id_provincia INT IDENTITY(1,1) NOT NULL,
    provincia VARCHAR(100) NOT NULL,
    id_pais INT NOT NULL,

    CONSTRAINT PK_provincias
        PRIMARY KEY (id_provincia),

    CONSTRAINT FK_provincias_paises
        FOREIGN KEY (id_pais)
        REFERENCES dbo.paises(id_pais),

    CONSTRAINT UQ_provincias
        UNIQUE (provincia, id_pais)
);
GO




CREATE TABLE dbo.ciudades
(
    id_ciudad INT IDENTITY(1,1) NOT NULL,
    ciudad VARCHAR(100) NOT NULL,
    id_provincia INT NOT NULL,

    CONSTRAINT PK_ciudades
        PRIMARY KEY (id_ciudad),

    CONSTRAINT FK_ciudades_provincias
        FOREIGN KEY (id_provincia)
        REFERENCES dbo.provincias(id_provincia),

    CONSTRAINT UQ_ciudades
        UNIQUE (ciudad, id_provincia)
);
GO




CREATE TABLE dbo.barrios
(
    id_barrio INT IDENTITY(1,1) NOT NULL,
    barrio VARCHAR(100) NOT NULL,
    id_ciudad INT NOT NULL,

    CONSTRAINT PK_barrios
        PRIMARY KEY (id_barrio),

    CONSTRAINT FK_barrios_ciudades
        FOREIGN KEY (id_ciudad)
        REFERENCES dbo.ciudades(id_ciudad),

    CONSTRAINT UQ_barrios
        UNIQUE (barrio, id_ciudad)
);
GO




CREATE TABLE dbo.direcciones
(
    id_direccion INT IDENTITY(1,1) NOT NULL,
    calle VARCHAR(100) NOT NULL,
    altura INT NOT NULL,
    piso VARCHAR(10) NULL,
    depto VARCHAR(10) NULL,
    id_barrio INT NOT NULL,

    CONSTRAINT PK_direcciones
        PRIMARY KEY (id_direccion),

    CONSTRAINT FK_direcciones_barrios
        FOREIGN KEY (id_barrio)
        REFERENCES dbo.barrios(id_barrio)
);
GO




CREATE TABLE dbo.sucursales
(
    id_sucursal INT IDENTITY(1,1) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    id_direccion INT NOT NULL,

    CONSTRAINT PK_sucursales
        PRIMARY KEY (id_sucursal),

    CONSTRAINT FK_sucursales_direcciones
        FOREIGN KEY (id_direccion)
        REFERENCES dbo.direcciones(id_direccion)
);

--=====================================================--
CREATE TABLE dbo.salas
(
    id_sala INT IDENTITY(1,1) NOT NULL,
    nro_sala INT NOT NULL,
    id_tipo_sala INT NOT NULL,
    id_sucursal INT NOT NULL,

    CONSTRAINT PK_salas
        PRIMARY KEY (id_sala),

    CONSTRAINT FK_salas_tipos_salas
        FOREIGN KEY (id_tipo_sala)
        REFERENCES dbo.tipos_salas(id_tipo_sala),

    CONSTRAINT FK_salas_sucursales
        FOREIGN KEY (id_sucursal)
        REFERENCES dbo.sucursales(id_sucursal),

    CONSTRAINT UQ_salas_sucursal_numero
        UNIQUE (id_sucursal, nro_sala)
);
GO



CREATE TABLE dbo.butacas
(
    id_butaca INT IDENTITY(1,1) NOT NULL,
    id_sala INT NOT NULL,
    nro_butaca INT NOT NULL,
    id_tipo_butacas INT NOT NULL,
    id_estado_butaca INT NOT NULL,

    CONSTRAINT PK_butacas
        PRIMARY KEY (id_butaca),

    CONSTRAINT FK_butacas_salas
        FOREIGN KEY (id_sala)
        REFERENCES dbo.salas(id_sala),

    CONSTRAINT FK_butacas_tipos_butacas
        FOREIGN KEY (id_tipo_butacas)
        REFERENCES dbo.tipos_butacas(id_tipo_butacas),

    CONSTRAINT FK_butacas_estados_butacas
        FOREIGN KEY (id_estado_butaca)
        REFERENCES dbo.estados_butacas(id_estado_butaca),

    CONSTRAINT UQ_butacas_sala_numero
        UNIQUE (id_sala, nro_butaca)
);
GO


CREATE TABLE dbo.cajeros
(
    id_cajero INT IDENTITY(1,1) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    id_turno INT NOT NULL,
    id_sucursal INT NOT NULL,

    CONSTRAINT PK_cajeros
        PRIMARY KEY (id_cajero),

    CONSTRAINT FK_cajeros_turnos
        FOREIGN KEY (id_turno)
        REFERENCES dbo.turnos(id_turno),

    CONSTRAINT FK_cajeros_sucursales
        FOREIGN KEY (id_sucursal)
        REFERENCES dbo.sucursales(id_sucursal)
);

--=========================================--

CREATE TABLE dbo.clientes
(
    cod_cliente INT IDENTITY(1,1) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    nro_doc VARCHAR(20) NOT NULL,
    id_tipo_doc INT NOT NULL,
    fecha_nacimiento DATE NULL,
    fecha_registro DATE NOT NULL
        CONSTRAINT DF_clientes_fecha_registro DEFAULT (GETDATE()),
    contraseña VARCHAR(255) NOT NULL,
    id_direccion INT NULL,

    CONSTRAINT PK_clientes
        PRIMARY KEY (cod_cliente),

    CONSTRAINT FK_clientes_tipos_documentos
        FOREIGN KEY (id_tipo_doc)
        REFERENCES dbo.tipos_documentos(id_tipo_doc),

    CONSTRAINT FK_clientes_direcciones
        FOREIGN KEY (id_direccion)
        REFERENCES dbo.direcciones(id_direccion),

    CONSTRAINT UQ_clientes_documento
        UNIQUE (id_tipo_doc, nro_doc)
);
GO



CREATE TABLE dbo.contactos
(
    id_contacto INT IDENTITY(1,1) NOT NULL,
    contacto VARCHAR(150) NOT NULL,
    id_tipo_contacto INT NOT NULL,

    CONSTRAINT PK_contactos
        PRIMARY KEY (id_contacto),

    CONSTRAINT FK_contactos_tipos_contactos
        FOREIGN KEY (id_tipo_contacto)
        REFERENCES dbo.tipos_contactos(id_tipo_contacto)
);
GO



CREATE TABLE dbo.clientes_contactos
(
    id_clientes_contacto INT IDENTITY(1,1) NOT NULL,
    id_contacto INT NOT NULL,
    cod_cliente INT NOT NULL,

    CONSTRAINT PK_clientes_contactos
        PRIMARY KEY (id_clientes_contacto),

    CONSTRAINT FK_clientes_contactos_contactos
        FOREIGN KEY (id_contacto)
        REFERENCES dbo.contactos(id_contacto),

    CONSTRAINT FK_clientes_contactos_clientes
        FOREIGN KEY (cod_cliente)
        REFERENCES dbo.clientes(cod_cliente),

    CONSTRAINT UQ_clientes_contactos
        UNIQUE (id_contacto, cod_cliente)
);
GO




CREATE TABLE dbo.preferencias_clientes
(
    id_preferencia_cliente INT IDENTITY(1,1) NOT NULL,
    cod_cliente INT NOT NULL,
    id_genero INT NOT NULL,
    suscribe_newsletter BIT NOT NULL
        CONSTRAINT DF_preferencias_newsletter DEFAULT (0),

    CONSTRAINT PK_preferencias_clientes
        PRIMARY KEY (id_preferencia_cliente),

    CONSTRAINT FK_preferencias_clientes_clientes
        FOREIGN KEY (cod_cliente)
        REFERENCES dbo.clientes(cod_cliente),

    CONSTRAINT FK_preferencias_clientes_generos
        FOREIGN KEY (id_genero)
        REFERENCES dbo.generos(id_genero),

    CONSTRAINT UQ_preferencias_cliente_genero
        UNIQUE (cod_cliente, id_genero)
);

--==========================================--
CREATE TABLE dbo.roles
(
    id_rol INT IDENTITY(1,1) NOT NULL,
    descripcion VARCHAR(50) NOT NULL,

    CONSTRAINT PK_roles
        PRIMARY KEY (id_rol),

    CONSTRAINT UQ_roles_descripcion
        UNIQUE (descripcion)
);
GO




CREATE TABLE dbo.usuarios
(
    id_usuario INT IDENTITY(1,1) NOT NULL,
    nombre_usuario VARCHAR(50) NOT NULL,
    contraseña VARCHAR(255) NOT NULL,
    id_rol INT NOT NULL,

    CONSTRAINT PK_usuarios
        PRIMARY KEY (id_usuario),

    CONSTRAINT UQ_usuarios_nombre
        UNIQUE (nombre_usuario),

    CONSTRAINT FK_usuarios_roles
        FOREIGN KEY (id_rol)
        REFERENCES dbo.roles(id_rol)
);
GO




INSERT INTO dbo.roles (descripcion)
VALUES
    ('Cliente'),
    ('Admin');
GO




ALTER TABLE dbo.clientes
DROP COLUMN contraseña;
GO




ALTER TABLE dbo.clientes
ADD id_usuario INT NOT NULL;
GO



ALTER TABLE dbo.clientes
ADD CONSTRAINT FK_clientes_usuarios
    FOREIGN KEY (id_usuario)
    REFERENCES dbo.usuarios(id_usuario);
GO




ALTER TABLE dbo.clientes
ADD CONSTRAINT UQ_clientes_usuario
    UNIQUE (id_usuario);
GO
-------------------------------------------

CREATE TABLE dbo.funciones
(
    id_funcion INT IDENTITY(1,1) NOT NULL,
    id_pelicula INT NOT NULL,
    id_sala INT NOT NULL,
    fecha DATE NOT NULL,
    id_horario INT NOT NULL,

    CONSTRAINT PK_funciones
        PRIMARY KEY (id_funcion),

    CONSTRAINT FK_funciones_peliculas
        FOREIGN KEY (id_pelicula)
        REFERENCES dbo.peliculas(id_pelicula),

    CONSTRAINT FK_funciones_salas
        FOREIGN KEY (id_sala)
        REFERENCES dbo.salas(id_sala),

    CONSTRAINT FK_funciones_horarios
        FOREIGN KEY (id_horario)
        REFERENCES dbo.horarios(id_horario),

    CONSTRAINT UQ_funciones_sala_fecha_horario
        UNIQUE (id_sala, fecha, id_horario)
);
GO



CREATE TABLE dbo.reservas
(
    id_reserva INT IDENTITY(1,1) NOT NULL,
    fecha DATETIME2 NOT NULL
        CONSTRAINT DF_reservas_fecha DEFAULT (SYSDATETIME()),
    id_funcion INT NOT NULL,
    cod_cliente INT NOT NULL,
    id_estado_reserva INT NOT NULL,
    monto_reserva DECIMAL(10,2) NOT NULL,

    CONSTRAINT PK_reservas
        PRIMARY KEY (id_reserva),

    CONSTRAINT FK_reservas_funciones
        FOREIGN KEY (id_funcion)
        REFERENCES dbo.funciones(id_funcion),

    CONSTRAINT FK_reservas_clientes
        FOREIGN KEY (cod_cliente)
        REFERENCES dbo.clientes(cod_cliente),

    CONSTRAINT FK_reservas_estados_reservas
        FOREIGN KEY (id_estado_reserva)
        REFERENCES dbo.estados_reservas(id_estado_reserva)
);
GO




CREATE TABLE dbo.reserva_detalle
(
    id_reserva_detalle INT IDENTITY(1,1) NOT NULL,
    id_reserva INT NOT NULL,
    id_butaca INT NOT NULL,

    CONSTRAINT PK_reserva_detalle
        PRIMARY KEY (id_reserva_detalle),

    CONSTRAINT FK_reserva_detalle_reservas
        FOREIGN KEY (id_reserva)
        REFERENCES dbo.reservas(id_reserva),

    CONSTRAINT FK_reserva_detalle_butacas
        FOREIGN KEY (id_butaca)
        REFERENCES dbo.butacas(id_butaca),

    CONSTRAINT UQ_reserva_detalle
        UNIQUE (id_reserva, id_butaca)
);
GO



CREATE TABLE dbo.entradas
(
    id_entrada INT IDENTITY(1,1) NOT NULL,
    id_funcion INT NOT NULL,
    id_butaca INT NOT NULL,
    precio DECIMAL(10,2) NOT NULL,

    CONSTRAINT PK_entradas
        PRIMARY KEY (id_entrada),

    CONSTRAINT FK_entradas_funciones
        FOREIGN KEY (id_funcion)
        REFERENCES dbo.funciones(id_funcion),

    CONSTRAINT FK_entradas_butacas
        FOREIGN KEY (id_butaca)
        REFERENCES dbo.butacas(id_butaca),

    CONSTRAINT UQ_entradas_funcion_butaca
        UNIQUE (id_funcion, id_butaca)
);
GO

--=======================================


CREATE TABLE dbo.facturas
(
    nro_factura INT IDENTITY(1,1) NOT NULL,
    id_condicion_IVA INT NOT NULL,
    fecha DATETIME2 NOT NULL
        CONSTRAINT DF_facturas_fecha DEFAULT (SYSDATETIME()),
    cod_cliente INT NOT NULL,
    id_cajero INT NOT NULL,
    id_sucursal INT NOT NULL,
    id_financiera INT NULL,
    id_estado_compra INT NOT NULL,
    id_canal_venta INT NOT NULL,

    CONSTRAINT PK_facturas
        PRIMARY KEY (nro_factura),

    CONSTRAINT FK_facturas_condiciones_IVA
        FOREIGN KEY (id_condicion_IVA)
        REFERENCES dbo.condiciones_IVA(id_condicion_IVA),

    CONSTRAINT FK_facturas_clientes
        FOREIGN KEY (cod_cliente)
        REFERENCES dbo.clientes(cod_cliente),

    CONSTRAINT FK_facturas_cajeros
        FOREIGN KEY (id_cajero)
        REFERENCES dbo.cajeros(id_cajero),

    CONSTRAINT FK_facturas_sucursales
        FOREIGN KEY (id_sucursal)
        REFERENCES dbo.sucursales(id_sucursal),

    CONSTRAINT FK_facturas_financieras
        FOREIGN KEY (id_financiera)
        REFERENCES dbo.financieras(id_financiera),

    CONSTRAINT FK_facturas_estados_compras
        FOREIGN KEY (id_estado_compra)
        REFERENCES dbo.estados_compras(id_estado_compra),

    CONSTRAINT FK_facturas_canales_ventas
        FOREIGN KEY (id_canal_venta)
        REFERENCES dbo.canales_ventas(id_canal_venta)
);
GO



CREATE TABLE dbo.facturas_formas_pagos
(
    id_factura_forma_pago INT IDENTITY(1,1) NOT NULL,
    nro_factura INT NOT NULL,
    id_forma_pago INT NOT NULL,

    CONSTRAINT PK_facturas_formas_pagos
        PRIMARY KEY (id_factura_forma_pago),

    CONSTRAINT FK_facturas_formas_pagos_facturas
        FOREIGN KEY (nro_factura)
        REFERENCES dbo.facturas(nro_factura),

    CONSTRAINT FK_facturas_formas_pagos_formas_pagos
        FOREIGN KEY (id_forma_pago)
        REFERENCES dbo.formas_pagos(id_forma_pago),

    CONSTRAINT UQ_facturas_formas_pagos
        UNIQUE (nro_factura, id_forma_pago)
);
GO

CREATE TABLE dbo.productos_shop
(
    id_producto_shop INT IDENTITY(1,1) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    precio DECIMAL(10,2) NOT NULL,
    descripcion VARCHAR(500) NULL,
    stock INT NOT NULL,
    id_marca INT NOT NULL,
    id_tipo_producto INT NOT NULL,

    CONSTRAINT PK_productos_shop
        PRIMARY KEY (id_producto_shop),

    CONSTRAINT FK_productos_shop_marcas
        FOREIGN KEY (id_marca)
        REFERENCES dbo.marcas(id_marca),

    CONSTRAINT FK_productos_shop_tipos_productos
        FOREIGN KEY (id_tipo_producto)
        REFERENCES dbo.tipos_productos(id_tipo_producto)
);
GO


CREATE TABLE dbo.promociones
(
    id_promocion INT IDENTITY(1,1) NOT NULL,
    descripcion VARCHAR(200) NOT NULL,
    id_tipo_promocion INT NOT NULL,
    id_estado_promocion INT NOT NULL,
    id_dia_promocion INT NOT NULL,
    id_financiera INT NULL,
    id_canal_venta INT NULL,
    cod_cliente INT NULL,

    CONSTRAINT PK_promociones
        PRIMARY KEY (id_promocion),

    CONSTRAINT FK_promociones_tipos_promociones
        FOREIGN KEY (id_tipo_promocion)
        REFERENCES dbo.tipos_promociones(id_tipo_promocion),

    CONSTRAINT FK_promociones_estados_promociones
        FOREIGN KEY (id_estado_promocion)
        REFERENCES dbo.estados_promociones(id_estado_promocion),

    CONSTRAINT FK_promociones_dias_promociones
        FOREIGN KEY (id_dia_promocion)
        REFERENCES dbo.dias_promociones(id_dia_promocion),

    CONSTRAINT FK_promociones_financieras
        FOREIGN KEY (id_financiera)
        REFERENCES dbo.financieras(id_financiera),

    CONSTRAINT FK_promociones_canales_ventas
        FOREIGN KEY (id_canal_venta)
        REFERENCES dbo.canales_ventas(id_canal_venta),

    CONSTRAINT FK_promociones_clientes
        FOREIGN KEY (cod_cliente)
        REFERENCES dbo.clientes(cod_cliente)
);
GO
--

CREATE TABLE dbo.detalle_facturas
(
    id_detalle_factura INT IDENTITY(1,1) NOT NULL,
    nro_factura INT NOT NULL,
    id_entrada INT NULL,
    id_producto_shop INT NULL,
    id_promocion INT NULL,
    cantidad INT NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,

    CONSTRAINT PK_detalle_facturas
        PRIMARY KEY (id_detalle_factura),

    CONSTRAINT FK_detalle_facturas_facturas
        FOREIGN KEY (nro_factura)
        REFERENCES dbo.facturas(nro_factura),

    CONSTRAINT FK_detalle_facturas_entradas
        FOREIGN KEY (id_entrada)
        REFERENCES dbo.entradas(id_entrada),

    CONSTRAINT FK_detalle_facturas_productos_shop
        FOREIGN KEY (id_producto_shop)
        REFERENCES dbo.productos_shop(id_producto_shop),

    CONSTRAINT FK_detalle_facturas_promociones
        FOREIGN KEY (id_promocion)
        REFERENCES dbo.promociones(id_promocion)
);
GO


--============================--




CREATE TABLE dbo.combos
(
    id_combo INT IDENTITY(1,1) NOT NULL,
    descripcion VARCHAR(200) NOT NULL,
    id_tipo_combo INT NOT NULL,

    CONSTRAINT PK_combos
        PRIMARY KEY (id_combo),

    CONSTRAINT FK_combos_tipos_combos
        FOREIGN KEY (id_tipo_combo)
        REFERENCES dbo.tipos_combos(id_tipo_combo)
);
GO




CREATE TABLE dbo.productos_combos
(
    id_producto_combo INT IDENTITY(1,1) NOT NULL,
    id_producto_shop INT NOT NULL,
    id_combo INT NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,

    CONSTRAINT PK_productos_combos
        PRIMARY KEY (id_producto_combo),

    CONSTRAINT FK_productos_combos_productos_shop
        FOREIGN KEY (id_producto_shop)
        REFERENCES dbo.productos_shop(id_producto_shop),

    CONSTRAINT FK_productos_combos_combos
        FOREIGN KEY (id_combo)
        REFERENCES dbo.combos(id_combo),

    CONSTRAINT UQ_productos_combos
        UNIQUE (id_producto_shop, id_combo)
);
GO




CREATE TABLE dbo.promociones_funciones
(
    id_promocion_funcion INT IDENTITY(1,1) NOT NULL,
    id_promocion INT NOT NULL,
    id_funcion INT NOT NULL,

    CONSTRAINT PK_promociones_funciones
        PRIMARY KEY (id_promocion_funcion),

    CONSTRAINT FK_promociones_funciones_promociones
        FOREIGN KEY (id_promocion)
        REFERENCES dbo.promociones(id_promocion),

    CONSTRAINT FK_promociones_funciones_funciones
        FOREIGN KEY (id_funcion)
        REFERENCES dbo.funciones(id_funcion),

    CONSTRAINT UQ_promociones_funciones
        UNIQUE (id_promocion, id_funcion)
);
GO




CREATE TABLE dbo.promociones_horarios
(
    id_promocion_horario INT IDENTITY(1,1) NOT NULL,
    id_promocion INT NOT NULL,
    id_horario INT NOT NULL,

    CONSTRAINT PK_promociones_horarios
        PRIMARY KEY (id_promocion_horario),

    CONSTRAINT FK_promociones_horarios_promociones
        FOREIGN KEY (id_promocion)
        REFERENCES dbo.promociones(id_promocion),

    CONSTRAINT FK_promociones_horarios_horarios
        FOREIGN KEY (id_horario)
        REFERENCES dbo.horarios(id_horario),

    CONSTRAINT UQ_promociones_horarios
        UNIQUE (id_promocion, id_horario)
);
GO


--=============================================--

CREATE TABLE dbo.resenias_peliculas
(
    id_resenia INT IDENTITY(1,1) NOT NULL,
    comentario VARCHAR(1000) NULL,
    fecha_resenia DATETIME2 NOT NULL
        CONSTRAINT DF_resenias_fecha DEFAULT (SYSDATETIME()),
    id_pelicula INT NOT NULL,
    id_puntuacion INT NOT NULL,
    cod_cliente INT NOT NULL,

    CONSTRAINT PK_resenias_peliculas
        PRIMARY KEY (id_resenia),

    CONSTRAINT FK_resenias_peliculas_peliculas
        FOREIGN KEY (id_pelicula)
        REFERENCES dbo.peliculas(id_pelicula),

    CONSTRAINT FK_resenias_peliculas_puntuaciones
        FOREIGN KEY (id_puntuacion)
        REFERENCES dbo.puntuaciones(id_puntuacion),

    CONSTRAINT FK_resenias_peliculas_clientes
        FOREIGN KEY (cod_cliente)
        REFERENCES dbo.clientes(cod_cliente),

    CONSTRAINT UQ_resenias_cliente_pelicula
        UNIQUE (cod_cliente, id_pelicula)
);
GO


--======================
ALTER TABLE dbo.detalle_facturas
ADD CONSTRAINT CK_detalle_facturas_un_solo_concepto
CHECK
(
    (
        CASE WHEN id_entrada IS NOT NULL THEN 1 ELSE 0 END
        +
        CASE WHEN id_producto_shop IS NOT NULL THEN 1 ELSE 0 END
        +
        CASE WHEN id_promocion IS NOT NULL THEN 1 ELSE 0 END
    ) = 1
);
GO

---------------------------

CREATE OR ALTER TRIGGER TR_RESERVA_DETALLE_VALIDAR_SALA
ON dbo.reserva_detalle
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted RD
        INNER JOIN dbo.reservas R
            ON RD.id_reserva = R.id_reserva
        INNER JOIN dbo.funciones F
            ON R.id_funcion = F.id_funcion
        INNER JOIN dbo.butacas B
            ON RD.id_butaca = B.id_butaca
        WHERE F.id_sala <> B.id_sala
    )
    BEGIN
        ROLLBACK TRANSACTION;

        THROW 50001,
            'La butaca no pertenece a la sala de la funcion reservada.',
            1;
    END;
END;
GO

---=======================================---
CREATE OR ALTER TRIGGER TR_ENTRADAS_VALIDAR_SALA
ON dbo.entradas
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted E
        INNER JOIN dbo.funciones F
            ON E.id_funcion = F.id_funcion
        INNER JOIN dbo.butacas B
            ON E.id_butaca = B.id_butaca
        WHERE F.id_sala <> B.id_sala
    )
    BEGIN
        ROLLBACK TRANSACTION;

        THROW 50002,
            'La butaca no pertenece a la sala de la funcion.',
            1;
    END;
END;
GO
--===========================================--
INSERT INTO dbo.estados_reservas (estado)
SELECT V.estado
FROM (VALUES
    ('Pendiente'),
    ('Confirmada'),
    ('Cancelada'),
    ('Convertida')
) AS V(estado)
WHERE NOT EXISTS
(
    SELECT 1
    FROM dbo.estados_reservas ER
    WHERE ER.estado = V.estado
);
GO

SELECT *
FROM dbo.estados_reservas
ORDER BY id_estado_reserva;
GO

--======================================--
CREATE NONCLUSTERED INDEX IX_reserva_detalle_butaca_reserva
ON dbo.reserva_detalle (id_butaca, id_reserva);
GO

CREATE NONCLUSTERED INDEX IX_reservas_funcion_estado
ON dbo.reservas (id_funcion, id_estado_reserva);
GO

CREATE NONCLUSTERED INDEX IX_entradas_butaca_funcion
ON dbo.entradas (id_butaca, id_funcion);
GO
--====================================--
CREATE OR ALTER TRIGGER dbo.TR_RESERVA_DETALLE_VALIDAR_SALA
ON dbo.reserva_detalle
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ID_FUNCION INT;
    DECLARE @ID_BUTACA INT;
    DECLARE @RESULTADO INT;
    DECLARE @RECURSO NVARCHAR(255);

    DECLARE CURSOR_BUTACAS CURSOR LOCAL FAST_FORWARD
    FOR
        SELECT DISTINCT
            R.id_funcion,
            I.id_butaca
        FROM inserted I
        INNER JOIN dbo.reservas R
            ON I.id_reserva = R.id_reserva
        INNER JOIN dbo.estados_reservas ER
            ON R.id_estado_reserva = ER.id_estado_reserva
        WHERE ER.estado IN ('Pendiente', 'Confirmada')
        ORDER BY R.id_funcion, I.id_butaca;

    OPEN CURSOR_BUTACAS;

    FETCH NEXT FROM CURSOR_BUTACAS
    INTO @ID_FUNCION, @ID_BUTACA;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @RECURSO = CONCAT(
            'CINE_BUTACA_',
            @ID_FUNCION,
            '_',
            @ID_BUTACA
        );

        EXEC @RESULTADO = sys.sp_getapplock
            @Resource = @RECURSO,
            @LockMode = 'Exclusive',
            @LockOwner = 'Transaction',
            @LockTimeout = 10000;

        IF @RESULTADO < 0
        BEGIN
            CLOSE CURSOR_BUTACAS;
            DEALLOCATE CURSOR_BUTACAS;

            THROW 50003,
                'No se pudo bloquear la butaca para validar su disponibilidad.',
                1;
        END;

        FETCH NEXT FROM CURSOR_BUTACAS
        INTO @ID_FUNCION, @ID_BUTACA;
    END;

    CLOSE CURSOR_BUTACAS;
    DEALLOCATE CURSOR_BUTACAS;

    -- Validar que las butacas pertenezcan a la sala.
    IF EXISTS
    (
        SELECT 1
        FROM inserted I
        INNER JOIN dbo.reservas R
            ON I.id_reserva = R.id_reserva
        INNER JOIN dbo.funciones F
            ON R.id_funcion = F.id_funcion
        INNER JOIN dbo.butacas B
            ON I.id_butaca = B.id_butaca
        WHERE F.id_sala <> B.id_sala
    )
    BEGIN
        THROW 50004,
            'La butaca no pertenece a la sala de la funcion.',
            1;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM inserted I
        INNER JOIN dbo.reservas R
            ON I.id_reserva = R.id_reserva
        INNER JOIN dbo.estados_reservas ER
            ON R.id_estado_reserva = ER.id_estado_reserva
        INNER JOIN dbo.entradas E
            ON E.id_funcion = R.id_funcion
            AND E.id_butaca = I.id_butaca
        WHERE ER.estado IN ('Pendiente', 'Confirmada')
    )
    BEGIN
        THROW 50005,
            'La butaca ya tiene una entrada vendida para esta funcion.',
            1;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM inserted I
        INNER JOIN dbo.reservas R
            ON I.id_reserva = R.id_reserva
        INNER JOIN dbo.estados_reservas ER
            ON R.id_estado_reserva = ER.id_estado_reserva
        INNER JOIN dbo.reserva_detalle RD
            ON RD.id_butaca = I.id_butaca
        INNER JOIN dbo.reservas R2
            ON RD.id_reserva = R2.id_reserva
        INNER JOIN dbo.estados_reservas ER2
            ON R2.id_estado_reserva = ER2.id_estado_reserva
        WHERE ER.estado IN ('Pendiente', 'Confirmada')
          AND ER2.estado IN ('Pendiente', 'Confirmada')
          AND R.id_funcion = R2.id_funcion
          AND R.id_reserva <> R2.id_reserva
    )
    BEGIN
        THROW 50006,
            'La butaca ya esta reservada para esta funcion.',
            1;
    END;
END;
GO

--==============================================--
CREATE OR ALTER TRIGGER dbo.TR_ENTRADAS_VALIDAR_SALA
ON dbo.entradas
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ID_FUNCION INT;
    DECLARE @ID_BUTACA INT;
    DECLARE @RESULTADO INT;
    DECLARE @RECURSO NVARCHAR(255);

    DECLARE CURSOR_BUTACAS CURSOR LOCAL FAST_FORWARD
    FOR
        SELECT DISTINCT
            id_funcion,
            id_butaca
        FROM inserted
        ORDER BY id_funcion, id_butaca;

    OPEN CURSOR_BUTACAS;

    FETCH NEXT FROM CURSOR_BUTACAS
    INTO @ID_FUNCION, @ID_BUTACA;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @RECURSO = CONCAT(
            'CINE_BUTACA_',
            @ID_FUNCION,
            '_',
            @ID_BUTACA
        );

        EXEC @RESULTADO = sys.sp_getapplock
            @Resource = @RECURSO,
            @LockMode = 'Exclusive',
            @LockOwner = 'Transaction',
            @LockTimeout = 10000;

        IF @RESULTADO < 0
        BEGIN
            CLOSE CURSOR_BUTACAS;
            DEALLOCATE CURSOR_BUTACAS;

            THROW 50007,
                'No se pudo bloquear la butaca para validar la venta.',
                1;
        END;

        FETCH NEXT FROM CURSOR_BUTACAS
        INTO @ID_FUNCION, @ID_BUTACA;
    END;

    CLOSE CURSOR_BUTACAS;
    DEALLOCATE CURSOR_BUTACAS;

    IF EXISTS
    (
        SELECT 1
        FROM inserted I
        INNER JOIN dbo.funciones F
            ON I.id_funcion = F.id_funcion
        INNER JOIN dbo.butacas B
            ON I.id_butaca = B.id_butaca
        WHERE F.id_sala <> B.id_sala
    )
    BEGIN
        THROW 50008,
            'La butaca no pertenece a la sala de la funcion.',
            1;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM inserted I
        INNER JOIN dbo.reservas R
            ON R.id_funcion = I.id_funcion
        INNER JOIN dbo.estados_reservas ER
            ON R.id_estado_reserva = ER.id_estado_reserva
        INNER JOIN dbo.reserva_detalle RD
            ON RD.id_reserva = R.id_reserva
            AND RD.id_butaca = I.id_butaca
        WHERE ER.estado IN ('Pendiente', 'Confirmada')
    )
    BEGIN
        THROW 50009,
            'La butaca tiene una reserva pendiente o confirmada.',
            1;
    END;
END;
GO
----===========================================--

CREATE OR ALTER TRIGGER dbo.TR_RESERVAS_VALIDAR_ESTADO
ON dbo.reservas
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT (UPDATE(id_estado_reserva) OR UPDATE(id_funcion))
        RETURN;

    DECLARE @ID_FUNCION INT;
    DECLARE @ID_BUTACA INT;
    DECLARE @RESULTADO INT;
    DECLARE @RECURSO NVARCHAR(255);

    DECLARE CURSOR_BUTACAS CURSOR LOCAL FAST_FORWARD
    FOR
        SELECT DISTINCT X.id_funcion, X.id_butaca
        FROM
        (
            SELECT I.id_funcion, RD.id_butaca
            FROM inserted I
            INNER JOIN dbo.reserva_detalle RD
                ON I.id_reserva = RD.id_reserva

            UNION

            SELECT D.id_funcion, RD.id_butaca
            FROM deleted D
            INNER JOIN dbo.reserva_detalle RD
                ON D.id_reserva = RD.id_reserva
        ) X
        ORDER BY X.id_funcion, X.id_butaca;

    OPEN CURSOR_BUTACAS;

    FETCH NEXT FROM CURSOR_BUTACAS
    INTO @ID_FUNCION, @ID_BUTACA;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @RECURSO = CONCAT(
            'CINE_BUTACA_',
            @ID_FUNCION,
            '_',
            @ID_BUTACA
        );

        EXEC @RESULTADO = sys.sp_getapplock
            @Resource = @RECURSO,
            @LockMode = 'Exclusive',
            @LockOwner = 'Transaction',
            @LockTimeout = 10000;

        IF @RESULTADO < 0
        BEGIN
            CLOSE CURSOR_BUTACAS;
            DEALLOCATE CURSOR_BUTACAS;

            THROW 50010,
                'No se pudo bloquear la butaca para modificar la reserva.',
                1;
        END;

        FETCH NEXT FROM CURSOR_BUTACAS
        INTO @ID_FUNCION, @ID_BUTACA;
    END;

    CLOSE CURSOR_BUTACAS;
    DEALLOCATE CURSOR_BUTACAS;

    IF EXISTS
    (
        SELECT 1
        FROM inserted I
        INNER JOIN dbo.estados_reservas ER
            ON I.id_estado_reserva = ER.id_estado_reserva
        INNER JOIN dbo.reserva_detalle RD
            ON I.id_reserva = RD.id_reserva
        INNER JOIN dbo.entradas E
            ON E.id_funcion = I.id_funcion
            AND E.id_butaca = RD.id_butaca
        WHERE ER.estado IN ('Pendiente', 'Confirmada')
    )
    BEGIN
        THROW 50011,
            'No se puede activar la reserva: una butaca ya tiene una entrada vendida.',
            1;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM inserted I
        INNER JOIN dbo.estados_reservas ER
            ON I.id_estado_reserva = ER.id_estado_reserva
        INNER JOIN dbo.reserva_detalle RD
            ON I.id_reserva = RD.id_reserva
        INNER JOIN dbo.reserva_detalle RD2
            ON RD.id_butaca = RD2.id_butaca
        INNER JOIN dbo.reservas R2
            ON RD2.id_reserva = R2.id_reserva
        INNER JOIN dbo.estados_reservas ER2
            ON R2.id_estado_reserva = ER2.id_estado_reserva
        WHERE ER.estado IN ('Pendiente', 'Confirmada')
          AND ER2.estado IN ('Pendiente', 'Confirmada')
          AND I.id_funcion = R2.id_funcion
          AND I.id_reserva <> R2.id_reserva
    )
    BEGIN
        THROW 50012,
            'No se puede activar la reserva: una butaca ya esta reservada.',
            1;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM inserted I
        INNER JOIN dbo.estados_reservas ER
            ON I.id_estado_reserva = ER.id_estado_reserva
        INNER JOIN dbo.reserva_detalle RD
            ON I.id_reserva = RD.id_reserva
        INNER JOIN dbo.funciones F
            ON I.id_funcion = F.id_funcion
        INNER JOIN dbo.butacas B
            ON RD.id_butaca = B.id_butaca
        WHERE ER.estado IN ('Pendiente', 'Confirmada')
          AND F.id_sala <> B.id_sala
    )
    BEGIN
        THROW 50013,
            'La butaca no pertenece a la sala de la funcion.',
            1;
    END;
END;
GO

-----==========================================--
CREATE OR ALTER TRIGGER dbo.TR_RESERVAS_VALIDAR_TRANSICION_ESTADO
ON dbo.reservas
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT UPDATE(id_estado_reserva)
        RETURN;

    IF EXISTS
    (
        SELECT 1
        FROM inserted I
        INNER JOIN deleted D
            ON I.id_reserva = D.id_reserva
        INNER JOIN dbo.estados_reservas E_ANT
            ON D.id_estado_reserva = E_ANT.id_estado_reserva
        INNER JOIN dbo.estados_reservas E_NUE
            ON I.id_estado_reserva = E_NUE.id_estado_reserva
        WHERE
            E_ANT.estado <> E_NUE.estado
            AND NOT
            (
                (E_ANT.estado = 'Pendiente'
                 AND E_NUE.estado IN
                 ('Confirmada', 'Cancelada', 'Convertida'))

                OR

                (E_ANT.estado = 'Confirmada'
                 AND E_NUE.estado IN
                 ('Cancelada', 'Convertida'))
            )
    )
    BEGIN
        THROW 50014,
            'La transicion de estado de la reserva no esta permitida.',
            1;
    END;
END;
GO
---=======================================--
IF NOT EXISTS
(
    SELECT 1
    FROM sys.key_constraints
    WHERE name = 'UQ_cajeros_id_cajero_sucursal'
      AND parent_object_id = OBJECT_ID('dbo.cajeros')
)
BEGIN
    ALTER TABLE dbo.cajeros
    ADD CONSTRAINT UQ_cajeros_id_cajero_sucursal
    UNIQUE (id_cajero, id_sucursal);
END;
GO



IF EXISTS
(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_facturas_cajeros'
)
BEGIN
    ALTER TABLE dbo.facturas
    DROP CONSTRAINT FK_facturas_cajeros;
END;
GO

IF EXISTS
(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_facturas_sucursales'
)
BEGIN
    ALTER TABLE dbo.facturas
    DROP CONSTRAINT FK_facturas_sucursales;
END;
GO



IF NOT EXISTS
(
    SELECT 1
    FROM sys.foreign_keys
    WHERE name = 'FK_facturas_cajero_sucursal'
)
BEGIN
    ALTER TABLE dbo.facturas
    ADD CONSTRAINT FK_facturas_cajero_sucursal
    FOREIGN KEY (id_cajero, id_sucursal)
    REFERENCES dbo.cajeros (id_cajero, id_sucursal);
END;
GO
--============================================--
CREATE OR ALTER TRIGGER dbo.TR_CLIENTES_VALIDAR_ROL
ON dbo.clientes
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted I
        INNER JOIN dbo.usuarios U
            ON I.id_usuario = U.id_usuario
        INNER JOIN dbo.roles R
            ON U.id_rol = R.id_rol
        WHERE R.descripcion <> 'Cliente'
    )
    BEGIN
        THROW 50015,
            'No se puede asociar un usuario que no tenga el rol Cliente.',
            1;
    END;
END;
GO




CREATE OR ALTER TRIGGER dbo.TR_USUARIOS_VALIDAR_CAMBIO_ROL
ON dbo.usuarios
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT UPDATE(id_rol)
        RETURN;

    IF EXISTS
    (
        SELECT 1
        FROM inserted I
        INNER JOIN dbo.clientes C
            ON I.id_usuario = C.id_usuario
        INNER JOIN dbo.roles R
            ON I.id_rol = R.id_rol
        WHERE R.descripcion <> 'Cliente'
    )
    BEGIN
        THROW 50016,
            'No se puede cambiar el rol de un usuario asociado a un cliente.',
            1;
    END;
END;
GO
--===============================================--
CREATE UNIQUE NONCLUSTERED INDEX IX_detalle_facturas_entrada_unica
ON dbo.detalle_facturas (id_entrada)
WHERE id_entrada IS NOT NULL;
GO

ALTER TABLE dbo.detalle_facturas
ADD CONSTRAINT CK_detalle_facturas_cantidad_entrada
CHECK
(
    id_entrada IS NULL
    OR cantidad = 1
);
GO

---=============================================--

ALTER TABLE dbo.detalle_facturas
ADD CONSTRAINT CK_detalle_facturas_cantidad_positiva
CHECK (cantidad > 0);
GO

ALTER TABLE dbo.detalle_facturas
ADD CONSTRAINT CK_detalle_facturas_precio_no_negativo
CHECK (precio_unitario >= 0);
GO

-- ENTRADAS
ALTER TABLE dbo.entradas
ADD CONSTRAINT CK_entradas_precio_no_negativo
CHECK (precio >= 0);
GO


--  PRODUCTOS DEL SHOP
ALTER TABLE dbo.productos_shop
ADD CONSTRAINT CK_productos_shop_precio_no_negativo
CHECK (precio >= 0);
GO

ALTER TABLE dbo.productos_shop
ADD CONSTRAINT CK_productos_shop_stock_no_negativo
CHECK (stock >= 0);
GO


--  RESERVAS
ALTER TABLE dbo.reservas
ADD CONSTRAINT CK_reservas_monto_no_negativo
CHECK (monto_reserva >= 0);
GO

--============================--
ALTER TABLE dbo.productos_combos
ADD CONSTRAINT CK_productos_combos_precio_no_negativo
CHECK (precio_unitario >= 0);
GO

---------------
ALTER TABLE dbo.salas
ADD CONSTRAINT CK_salas_numero_positivo
CHECK (nro_sala > 0);
GO


ALTER TABLE dbo.butacas
ADD CONSTRAINT CK_butacas_numero_positivo
CHECK (nro_butaca > 0);
GO

-----------------------
ALTER TABLE dbo.direcciones
ADD CONSTRAINT CK_direcciones_altura_positiva
CHECK (altura > 0);
GO

------------------------------------------------
ALTER TABLE dbo.puntuaciones
ADD CONSTRAINT CK_puntuaciones_rango
CHECK (puntuacion BETWEEN 1 AND 5);
GO
-----------------------------
ALTER TABLE dbo.clientes
ADD CONSTRAINT CK_clientes_fecha_nacimiento
CHECK
(
    fecha_nacimiento IS NULL
    OR fecha_nacimiento <= CONVERT(DATE, GETDATE())
);
GO

ALTER TABLE dbo.clientes
ADD CONSTRAINT CK_clientes_fecha_registro
CHECK
(
    fecha_registro <= CONVERT(DATE, GETDATE())
);
GO
-------------------------
ALTER TABLE dbo.directores
ADD CONSTRAINT CK_directores_fecha_nacimiento
CHECK
(
    fecha_nacimiento IS NULL
    OR fecha_nacimiento <= CONVERT(DATE, GETDATE())
);
GO

------------
ALTER TABLE dbo.funciones
ADD CONSTRAINT CK_funciones_fecha_no_pasada
CHECK
(
    fecha >= CONVERT(DATE, GETDATE())
);
GO
--=================================================--
ALTER TABLE dbo.usuarios
ADD correo_electronico VARCHAR(254) NOT NULL;
GO

ALTER TABLE dbo.usuarios
ADD CONSTRAINT CK_usuarios_correo_electronico_NoVacio
CHECK (LEN(LTRIM(RTRIM(correo_electronico))) > 0);
GO

CREATE UNIQUE NONCLUSTERED INDEX
IX_usuarios_correo_electronico
ON dbo.usuarios (correo_electronico);
GO

--===============================================--
ALTER TABLE dbo.usuarios
ADD CONSTRAINT CK_usuarios_nombre_usuario_sin_espacios
CHECK
(
    DATALENGTH(nombre_usuario) =
    DATALENGTH(LTRIM(RTRIM(nombre_usuario)))
);
GO

ALTER TABLE dbo.usuarios
ADD CONSTRAINT CK_usuarios_correo_sin_espacios
CHECK
(
    DATALENGTH(correo_electronico) =
    DATALENGTH(LTRIM(RTRIM(correo_electronico)))
);
GO

---=======================================--
CREATE OR ALTER TRIGGER dbo.TR_USUARIOS_VALIDAR_IDENTIFICADORES
ON dbo.usuarios
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT (UPDATE(nombre_usuario) OR UPDATE(correo_electronico))
        RETURN;

    DECLARE @Resultado INT;

    EXEC @Resultado = sys.sp_getapplock
        @Resource = 'CINE_USUARIOS_IDENTIFICADORES',
        @LockMode = 'Exclusive',
        @LockOwner = 'Transaction',
        @LockTimeout = 10000;

    IF @Resultado < 0
    BEGIN
        THROW 50017,
            'No se pudo validar la unicidad de los identificadores.',
            1;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM inserted I
        INNER JOIN dbo.usuarios U
            ON U.id_usuario <> I.id_usuario
            AND
            (
                I.nombre_usuario = U.correo_electronico
                OR
                I.correo_electronico = U.nombre_usuario
            )
    )
    BEGIN
        THROW 50018,
            'El nombre de usuario o correo electronico ya esta utilizado como identificador de otra cuenta.',
            1;
    END;
END;
GO

--===============================================--
INSERT INTO dbo.tipos_documentos (tipo)
VALUES
    ('DNI'),
    ('Pasaporte'),
    ('Documento extranjero');
GO
--==========================================--
ALTER TABLE dbo.tipos_documentos
ADD CONSTRAINT CK_tipos_documentos_sin_espacios
CHECK
(
    DATALENGTH(tipo) =
    DATALENGTH(LTRIM(RTRIM(tipo)))
);
GO

--===========================================--
ALTER TABLE dbo.paises
ADD CONSTRAINT UQ_paises_pais
UNIQUE (pais);
GO

ALTER TABLE dbo.provincias
ADD CONSTRAINT UQ_provincias_pais
UNIQUE (id_pais, provincia);
GO

ALTER TABLE dbo.ciudades
ADD CONSTRAINT UQ_ciudades_provincia
UNIQUE (id_provincia, ciudad);
GO

ALTER TABLE dbo.barrios
ADD CONSTRAINT UQ_barrios_ciudad
UNIQUE (id_ciudad, barrio);
GO

--====================================--
ALTER TABLE dbo.contactos
ADD CONSTRAINT UQ_contactos_tipo_contacto
UNIQUE (id_tipo_contacto, contacto);
GO

--=======================================--
ALTER TABLE dbo.sucursales
ADD CONSTRAINT UQ_sucursales_nombre
UNIQUE (nombre);
GO

--===============================--
ALTER TABLE dbo.condiciones_IVA
ALTER COLUMN condicion VARCHAR(30) NOT NULL;
GO





BEGIN TRY
    BEGIN TRANSACTION;

    -- CANALES DE VENTA
    INSERT INTO dbo.canales_ventas (canal_venta)
    SELECT V.valor
    FROM (VALUES ('Presencial'), ('Web')) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.canales_ventas T
        WHERE T.canal_venta = V.valor
    );

    -- CLASIFICACIONES
    INSERT INTO dbo.clasificaciones (clasificacion)
    SELECT V.valor
    FROM (VALUES ('ATP'), ('+13'), ('+16'), ('+18')) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.clasificaciones T
        WHERE T.clasificacion = V.valor
    );

    -- CONDICIONES IVA
    INSERT INTO dbo.condiciones_IVA (condicion)
    SELECT V.valor
    FROM (VALUES
        ('Consumidor Final'),
        ('Responsable Inscripto'),
        ('Monotributista')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.condiciones_IVA T
        WHERE T.condicion = V.valor
    );

    -- DÍAS DE PROMOCIONES
    INSERT INTO dbo.dias_promociones (dias_promocion)
    SELECT V.valor
    FROM (VALUES
        ('Todos'),
        ('Lunes'),
        ('Martes'),
        ('Miercoles'),
        ('Jueves'),
        ('Viernes'),
        ('Sabado'),
        ('Domingo')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.dias_promociones T
        WHERE T.dias_promocion = V.valor
    );

    -- DISTRIBUIDORES
    INSERT INTO dbo.distribuidores (distribuidor)
    SELECT V.valor
    FROM (VALUES
        ('Andes Films'),
        ('Sur Distribucion')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.distribuidores T
        WHERE T.distribuidor = V.valor
    );

    -- ESTADOS DE BUTACAS
    INSERT INTO dbo.estados_butacas (estado_butaca)
    SELECT V.valor
    FROM (VALUES
        ('Disponible'),
        ('Fuera de servicio')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.estados_butacas T
        WHERE T.estado_butaca = V.valor
    );

    -- ESTADOS DE COMPRAS
    INSERT INTO dbo.estados_compras (estado)
    SELECT V.valor
    FROM (VALUES
        ('Pendiente'),
        ('Pagada'),
        ('Anulada')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.estados_compras T
        WHERE T.estado = V.valor
    );

    -- ESTADOS DE PROMOCIONES
    INSERT INTO dbo.estados_promociones (estado)
    SELECT V.valor
    FROM (VALUES
        ('Activa'),
        ('Inactiva')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.estados_promociones T
        WHERE T.estado = V.valor
    );

    -- FINANCIERAS
    INSERT INTO dbo.financieras (financiera)
    SELECT V.valor
    FROM (VALUES
        ('Banco Nacion'),
        ('Banco Galicia')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.financieras T
        WHERE T.financiera = V.valor
    );

    -- FORMAS DE PAGO
    INSERT INTO dbo.formas_pagos (forma_pago)
    SELECT V.valor
    FROM (VALUES
        ('Efectivo'),
        ('Tarjeta de debito'),
        ('Tarjeta de credito'),
        ('Transferencia')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.formas_pagos T
        WHERE T.forma_pago = V.valor
    );

    -- FORMATOS DE PELICULAS
    INSERT INTO dbo.formatos_peliculas (formato)
    SELECT V.valor
    FROM (VALUES ('2D'), ('3D')) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.formatos_peliculas T
        WHERE T.formato = V.valor
    );

    -- GENEROS
    INSERT INTO dbo.generos (genero)
    SELECT V.valor
    FROM (VALUES
        ('Accion'),
        ('Comedia'),
        ('Drama'),
        ('Ciencia ficcion')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.generos T
        WHERE T.genero = V.valor
    );

    -- HORARIOS
    INSERT INTO dbo.horarios (horario)
    SELECT V.valor
    FROM (VALUES
        (CAST('17:00' AS TIME)),
        (CAST('19:30' AS TIME)),
        (CAST('22:00' AS TIME))
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.horarios T
        WHERE T.horario = V.valor
    );

    -- IDIOMAS
    INSERT INTO dbo.idiomas (idioma)
    SELECT V.valor
    FROM (VALUES ('Español'), ('Ingles')) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.idiomas T
        WHERE T.idioma = V.valor
    );

    -- MARCAS
    INSERT INTO dbo.marcas (marca)
    SELECT V.valor
    FROM (VALUES
        ('Cine Snacks'),
        ('Bebidas del Centro')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.marcas T
        WHERE T.marca = V.valor
    );

    -- PAISES
    INSERT INTO dbo.paises (pais)
    SELECT V.valor
    FROM (VALUES
        ('Argentina'),
        ('Estados Unidos')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.paises T
        WHERE T.pais = V.valor
    );

    -- PUNTUACIONES
    INSERT INTO dbo.puntuaciones (puntuacion)
    SELECT V.valor
    FROM (VALUES (1), (2), (3), (4), (5)) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.puntuaciones T
        WHERE T.puntuacion = V.valor
    );

    -- TIPOS DE BUTACAS
    INSERT INTO dbo.tipos_butacas (tipo_butaca)
    SELECT V.valor
    FROM (VALUES
        ('Comun'),
        ('Preferencial')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.tipos_butacas T
        WHERE T.tipo_butaca = V.valor
    );

    -- TIPOS DE COMBOS
    INSERT INTO dbo.tipos_combos (descripcion)
    SELECT V.valor
    FROM (VALUES
        ('Individual'),
        ('Duo')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.tipos_combos T
        WHERE T.descripcion = V.valor
    );

    -- TIPOS DE CONTACTOS
    INSERT INTO dbo.tipos_contactos (tipo)
    SELECT V.valor
    FROM (VALUES
        ('Email'),
        ('Telefono')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.tipos_contactos T
        WHERE T.tipo = V.valor
    );

    -- TIPOS DE PRODUCTOS
    INSERT INTO dbo.tipos_productos (tipo_producto)
    SELECT V.valor
    FROM (VALUES
        ('Golosinas'),
        ('Bebidas')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.tipos_productos T
        WHERE T.tipo_producto = V.valor
    );

    -- TIPOS DE PROMOCIONES
    INSERT INTO dbo.tipos_promociones (tipo_promocion)
    SELECT V.valor
    FROM (VALUES
        ('Descuento general'),
        ('Beneficio bancario')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.tipos_promociones T
        WHERE T.tipo_promocion = V.valor
    );

    -- TIPOS DE SALAS
    INSERT INTO dbo.tipos_salas (tipo_sala)
    SELECT V.valor
    FROM (VALUES
        ('Tradicional'),
        ('Premium'),
        ('3D')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.tipos_salas T
        WHERE T.tipo_sala = V.valor
    );

    -- TURNOS
    INSERT INTO dbo.turnos (turno)
    SELECT V.valor
    FROM (VALUES
        ('Mañana'),
        ('Tarde'),
        ('Noche')
    ) V(valor)
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.turnos T
        WHERE T.turno = V.valor
    );

    COMMIT TRANSACTION;

    PRINT 'Catalogos cargados correctamente.';

END TRY
BEGIN CATCH

    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;

END CATCH;
GO

---==========================================--

BEGIN TRY
    BEGIN TRANSACTION;

    -- PROVINCIAS
    INSERT INTO dbo.provincias (provincia, id_pais)
    SELECT V.provincia, P.id_pais
    FROM (VALUES
        ('Córdoba'),
        ('Buenos Aires')
    ) V(provincia)
    CROSS JOIN dbo.paises P
    WHERE P.pais = 'Argentina'
      AND NOT EXISTS (
          SELECT 1
          FROM dbo.provincias PR
          WHERE PR.id_pais = P.id_pais
            AND PR.provincia = V.provincia
      );

    -- CIUDADES
    INSERT INTO dbo.ciudades (ciudad, id_provincia)
    SELECT V.ciudad, PR.id_provincia
    FROM (VALUES
        ('Córdoba', 'Córdoba'),
        ('Villa Carlos Paz', 'Córdoba'),
        ('Buenos Aires', 'Buenos Aires')
    ) V(ciudad, provincia)
    INNER JOIN dbo.provincias PR
        ON PR.provincia = V.provincia
    INNER JOIN dbo.paises P
        ON P.id_pais = PR.id_pais
       AND P.pais = 'Argentina'
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.ciudades C
        WHERE C.id_provincia = PR.id_provincia
          AND C.ciudad = V.ciudad
    );

    -- BARRIOS
    INSERT INTO dbo.barrios (barrio, id_ciudad)
    SELECT V.barrio, C.id_ciudad
    FROM (VALUES
        ('Centro', 'Córdoba'),
        ('Nueva Córdoba', 'Córdoba'),
        ('Centro', 'Villa Carlos Paz'),
        ('Palermo', 'Buenos Aires')
    ) V(barrio, ciudad)
    INNER JOIN dbo.ciudades C
        ON C.ciudad = V.ciudad
    INNER JOIN dbo.provincias PR
        ON PR.id_provincia = C.id_provincia
    INNER JOIN dbo.paises P
        ON P.id_pais = PR.id_pais
       AND P.pais = 'Argentina'
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.barrios B
        WHERE B.id_ciudad = C.id_ciudad
          AND B.barrio = V.barrio
    );

    COMMIT TRANSACTION;

    PRINT 'Provincias, ciudades y barrios cargados correctamente.';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO

----========================================--


BEGIN TRY
    BEGIN TRANSACTION;

    -- DIRECCIONES
    INSERT INTO dbo.direcciones
        (calle, altura, piso, depto, id_barrio)
    SELECT
        V.calle,
        V.altura,
        V.piso,
        V.depto,
        B.id_barrio
    FROM (VALUES
        ('Av. General Paz', 100, NULL, NULL, 'Centro', 'Córdoba'),
        ('Obispo Trejo', 500, NULL, NULL, 'Nueva Córdoba', 'Córdoba'),
        ('Av. San Martín', 200, NULL, NULL, 'Centro', 'Villa Carlos Paz')
    ) V(calle, altura, piso, depto, barrio, ciudad)
    INNER JOIN dbo.barrios B
        ON B.barrio = V.barrio
    INNER JOIN dbo.ciudades C
        ON C.id_ciudad = B.id_ciudad
       AND C.ciudad = V.ciudad
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.direcciones D
        WHERE D.calle = V.calle
          AND D.altura = V.altura
          AND D.id_barrio = B.id_barrio
    );

    -- SUCURSALES
    INSERT INTO dbo.sucursales (nombre, id_direccion)
    SELECT V.nombre, D.id_direccion
    FROM (VALUES
        ('Cine Central Córdoba', 'Av. General Paz', 100, 'Centro', 'Córdoba'),
        ('Cine Carlos Paz', 'Av. San Martín', 200, 'Centro', 'Villa Carlos Paz')
    ) V(nombre, calle, altura, barrio, ciudad)
    INNER JOIN dbo.barrios B
        ON B.barrio = V.barrio
    INNER JOIN dbo.ciudades C
        ON C.id_ciudad = B.id_ciudad
       AND C.ciudad = V.ciudad
    INNER JOIN dbo.direcciones D
        ON D.id_barrio = B.id_barrio
       AND D.calle = V.calle
       AND D.altura = V.altura
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.sucursales S
        WHERE S.nombre = V.nombre
    );

    COMMIT TRANSACTION;

    PRINT 'Direcciones y sucursales cargadas correctamente.';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO

--======================================================--
BEGIN TRY
    BEGIN TRANSACTION;

    -- CREAR SALAS
    INSERT INTO dbo.salas
        (nro_sala, id_tipo_sala, id_sucursal)
    SELECT
        V.nro_sala,
        TS.id_tipo_sala,
        S.id_sucursal
    FROM (VALUES
        ('Cine Central Córdoba', 1, 'Tradicional'),
        ('Cine Central Córdoba', 2, '3D'),
        ('Cine Carlos Paz', 1, 'Tradicional'),
        ('Cine Carlos Paz', 2, 'Premium')
    ) V(nombre_sucursal, nro_sala, tipo_sala)
    INNER JOIN dbo.sucursales S
        ON S.nombre = V.nombre_sucursal
    INNER JOIN dbo.tipos_salas TS
        ON TS.tipo_sala = V.tipo_sala
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.salas SA
        WHERE SA.id_sucursal = S.id_sucursal
          AND SA.nro_sala = V.nro_sala
    );

    -- CREAR BUTACAS
    INSERT INTO dbo.butacas
        (id_sala, nro_butaca, id_tipo_butacas, id_estado_butaca)
    SELECT
        S.id_sala,
        V.nro_butaca,
        TB.id_tipo_butacas,
        EB.id_estado_butaca
    FROM dbo.salas S
    INNER JOIN dbo.sucursales SU
        ON SU.id_sucursal = S.id_sucursal
    CROSS JOIN (VALUES
        (1, 'Comun'),
        (2, 'Comun'),
        (3, 'Comun'),
        (4, 'Comun'),
        (5, 'Comun'),
        (6, 'Comun'),
        (7, 'Preferencial'),
        (8, 'Preferencial')
    ) V(nro_butaca, tipo_butaca)
    INNER JOIN dbo.tipos_butacas TB
        ON TB.tipo_butaca = V.tipo_butaca
    INNER JOIN dbo.estados_butacas EB
        ON EB.estado_butaca = 'Disponible'
    WHERE SU.nombre IN (
        'Cine Central Córdoba',
        'Cine Carlos Paz'
    )
      AND S.nro_sala IN (1, 2)
      AND NOT EXISTS (
          SELECT 1
          FROM dbo.butacas B
          WHERE B.id_sala = S.id_sala
            AND B.nro_butaca = V.nro_butaca
      );

    COMMIT TRANSACTION;

    PRINT 'Salas y butacas creadas correctamente.';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO
--=====================================--


BEGIN TRY
    BEGIN TRANSACTION;

    -- DIRECTORES
    INSERT INTO dbo.directores
        (nombre, apellido, fecha_nacimiento, bio)
    SELECT V.nombre, V.apellido, V.fecha_nacimiento, V.bio
    FROM (VALUES
        ('Martín', 'Suárez', '1980-04-15',
         'Director argentino especializado en drama y suspenso.'),
        ('Lucía', 'Fernández', '1985-09-22',
         'Directora argentina de cine de ciencia ficción.'),
        ('Thomas', 'Miller', '1975-02-10',
         'Director estadounidense de cine de aventuras.')
    ) V(nombre, apellido, fecha_nacimiento, bio)
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.directores D
        WHERE D.nombre = V.nombre
          AND D.apellido = V.apellido
    );

    -- PELÍCULAS
    INSERT INTO dbo.peliculas
        (titulo, descripcion, fecha_lanzamiento,
         id_pais, id_formato_pelicula,
         id_clasificacion, id_distribuidor, id_director)
    SELECT
        V.titulo,
        V.descripcion,
        V.fecha_lanzamiento,
        PA.id_pais,
        FO.id_formato_pelicula,
        CL.id_clasificacion,
        DI.id_distribuidor,
        DR.id_director
    FROM (VALUES
        ('El último viaje',
         'Un hombre emprende un viaje que cambiará su vida.',
         '2024-05-10', 'Argentina', '2D', 'ATP',
         'Andes Films', 'Martín', 'Suárez'),

        ('Código perdido',
         'Una programadora descubre un secreto oculto en una red informática.',
         '2025-08-21', 'Argentina', '2D', '+13',
         'Andes Films', 'Lucía', 'Fernández'),

        ('La ciudad sumergida',
         'Una expedición explora una antigua ciudad bajo el océano.',
         '2026-03-12', 'Estados Unidos', '3D', '+13',
         'Sur Distribucion', 'Thomas', 'Miller'),

        ('Rumbo a las estrellas',
         'Una tripulación intenta encontrar un nuevo hogar para la humanidad.',
         '2026-12-18', 'Estados Unidos', '3D', '+16',
         'Sur Distribucion', 'Lucía', 'Fernández')
    ) V(titulo, descripcion, fecha_lanzamiento,
        pais, formato, clasificacion,
        distribuidor, nombre_director, apellido_director)
    INNER JOIN dbo.paises PA
        ON PA.pais = V.pais
    INNER JOIN dbo.formatos_peliculas FO
        ON FO.formato = V.formato
    INNER JOIN dbo.clasificaciones CL
        ON CL.clasificacion = V.clasificacion
    INNER JOIN dbo.distribuidores DI
        ON DI.distribuidor = V.distribuidor
    INNER JOIN dbo.directores DR
        ON DR.nombre = V.nombre_director
       AND DR.apellido = V.apellido_director
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.peliculas P
        WHERE P.titulo = V.titulo
    );

    COMMIT TRANSACTION;

    PRINT 'Directores y películas cargados correctamente.';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO
---==============================================--
BEGIN TRY
    BEGIN TRANSACTION;

    INSERT INTO dbo.funciones
        (id_pelicula, id_sala, fecha, id_horario)
    SELECT
        P.id_pelicula,
        S.id_sala,
        V.fecha,
        H.id_horario
    FROM (VALUES
        ('El último viaje', 'Cine Central Córdoba', 1, '2026-11-10', '17:00'),
        ('Código perdido', 'Cine Central Córdoba', 2, '2026-11-10', '19:30'),
        ('La ciudad sumergida', 'Cine Carlos Paz', 1, '2026-11-10', '17:00'),
        ('Rumbo a las estrellas', 'Cine Carlos Paz', 2, '2026-11-10', '22:00'),
        ('El último viaje', 'Cine Central Córdoba', 1, '2026-11-11', '19:30'),
        ('La ciudad sumergida', 'Cine Carlos Paz', 1, '2026-11-11', '22:00')
    ) V(titulo, sucursal, nro_sala, fecha, horario)
    INNER JOIN dbo.peliculas P
        ON P.titulo = V.titulo
    INNER JOIN dbo.sucursales SU
        ON SU.nombre = V.sucursal
    INNER JOIN dbo.salas S
        ON S.id_sucursal = SU.id_sucursal
       AND S.nro_sala = V.nro_sala
    INNER JOIN dbo.horarios H
        ON H.horario = CAST(V.horario AS TIME)
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.funciones F
        WHERE F.id_sala = S.id_sala
          AND F.fecha = V.fecha
          AND F.id_horario = H.id_horario
    );

    COMMIT TRANSACTION;

    PRINT 'Funciones creadas correctamente.';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO
--==============================================--
BEGIN TRY
    BEGIN TRANSACTION;

    -- PRODUCTOS
    INSERT INTO dbo.productos_shop
        (nombre, precio, descripcion, stock, id_marca, id_tipo_producto)
    SELECT
        V.nombre,
        V.precio,
        V.descripcion,
        V.stock,
        M.id_marca,
        TP.id_tipo_producto
    FROM (VALUES
        ('Pochoclos medianos', 3500, 'Pochoclos salados tamaño mediano.', 50, 'Cine Snacks', 'Golosinas'),
        ('Gaseosa 500 ml', 2200, 'Gaseosa en botella de 500 ml.', 60, 'Bebidas del Centro', 'Bebidas'),
        ('Chocolate', 1800, 'Barra de chocolate.', 40, 'Cine Snacks', 'Golosinas'),
        ('Caramelos', 900, 'Bolsa pequeña de caramelos.', 35, 'Cine Snacks', 'Golosinas')
    ) V(nombre, precio, descripcion, stock, marca, tipo_producto)
    INNER JOIN dbo.marcas M
        ON M.marca = V.marca
    INNER JOIN dbo.tipos_productos TP
        ON TP.tipo_producto = V.tipo_producto
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.productos_shop PS
        WHERE PS.nombre = V.nombre
    );

    -- COMBOS
    INSERT INTO dbo.combos (descripcion, id_tipo_combo)
    SELECT V.descripcion, TC.id_tipo_combo
    FROM (VALUES
        ('Combo Individual: pochoclos medianos y gaseosa.', 'Individual'),
        ('Combo Duo: dos pochoclos medianos y dos gaseosas.', 'Duo')
    ) V(descripcion, tipo_combo)
    INNER JOIN dbo.tipos_combos TC
        ON TC.descripcion = V.tipo_combo
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.combos C
        WHERE C.descripcion = V.descripcion
    );

    COMMIT TRANSACTION;

    PRINT 'Productos y combos creados correctamente.';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO
--=========================================--
ALTER TABLE dbo.productos_combos
ADD cantidad INT NOT NULL
    CONSTRAINT DF_productos_combos_cantidad DEFAULT (1);
GO

ALTER TABLE dbo.productos_combos
ADD CONSTRAINT CK_productos_combos_cantidad_positiva
CHECK (cantidad > 0);
GO
--============================================--
BEGIN TRY
    BEGIN TRANSACTION;

    INSERT INTO dbo.productos_combos
        (id_producto_shop, id_combo, precio_unitario, cantidad)
    SELECT
        PS.id_producto_shop,
        C.id_combo,
        PS.precio,
        V.cantidad
    FROM (VALUES
        ('Combo Individual: pochoclos medianos y gaseosa.',
         'Pochoclos medianos', 1),
        ('Combo Individual: pochoclos medianos y gaseosa.',
         'Gaseosa 500 ml', 1),
        ('Combo Duo: dos pochoclos medianos y dos gaseosas.',
         'Pochoclos medianos', 2),
        ('Combo Duo: dos pochoclos medianos y dos gaseosas.',
         'Gaseosa 500 ml', 2)
    ) V(descripcion_combo, nombre_producto, cantidad)
    INNER JOIN dbo.combos C
        ON C.descripcion = V.descripcion_combo
    INNER JOIN dbo.productos_shop PS
        ON PS.nombre = V.nombre_producto
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.productos_combos PC
        WHERE PC.id_combo = C.id_combo
          AND PC.id_producto_shop = PS.id_producto_shop
    );

    COMMIT TRANSACTION;

    PRINT 'Composición de combos cargada correctamente.';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO
--=========================================--

ALTER TABLE dbo.promociones
ADD
    porcentaje_descuento DECIMAL(5,2) NOT NULL
        CONSTRAINT DF_promociones_porcentaje
        DEFAULT (10.00),

    fecha_inicio DATE NOT NULL
        CONSTRAINT DF_promociones_fecha_inicio
        DEFAULT (CONVERT(DATE, GETDATE())),

    fecha_fin DATE NOT NULL
        CONSTRAINT DF_promociones_fecha_fin
        DEFAULT (DATEADD(DAY, 30, CONVERT(DATE, GETDATE()))),

    monto_minimo DECIMAL(10,2) NOT NULL
        CONSTRAINT DF_promociones_monto_minimo
        DEFAULT (0);
GO

ALTER TABLE dbo.promociones
ADD
    CONSTRAINT CK_promociones_porcentaje
        CHECK (porcentaje_descuento > 0
               AND porcentaje_descuento <= 100),

    CONSTRAINT CK_promociones_fechas
        CHECK (fecha_fin >= fecha_inicio),

    CONSTRAINT CK_promociones_monto_minimo
        CHECK (monto_minimo >= 0);
GO

-------------====================================--
CREATE TABLE dbo.promociones_articulos
(
    id_promocion_articulo INT IDENTITY(1,1) NOT NULL,
    id_promocion INT NOT NULL,
    id_entrada INT NULL,
    id_producto_shop INT NULL,
    id_combo INT NULL,

    CONSTRAINT PK_promociones_articulos
        PRIMARY KEY (id_promocion_articulo),

    CONSTRAINT FK_promociones_articulos_promociones
        FOREIGN KEY (id_promocion)
        REFERENCES dbo.promociones(id_promocion),

    CONSTRAINT FK_promociones_articulos_entradas
        FOREIGN KEY (id_entrada)
        REFERENCES dbo.entradas(id_entrada),

    CONSTRAINT FK_promociones_articulos_productos
        FOREIGN KEY (id_producto_shop)
        REFERENCES dbo.productos_shop(id_producto_shop),

    CONSTRAINT FK_promociones_articulos_combos
        FOREIGN KEY (id_combo)
        REFERENCES dbo.combos(id_combo),

    CONSTRAINT CK_promociones_articulos_un_solo_articulo
        CHECK (
            (CASE WHEN id_entrada IS NOT NULL THEN 1 ELSE 0 END) +
            (CASE WHEN id_producto_shop IS NOT NULL THEN 1 ELSE 0 END) +
            (CASE WHEN id_combo IS NOT NULL THEN 1 ELSE 0 END) = 1
        )
);
GO

CREATE UNIQUE INDEX UQ_promociones_articulos_entrada
ON dbo.promociones_articulos(id_promocion, id_entrada)
WHERE id_entrada IS NOT NULL;
GO

CREATE UNIQUE INDEX UQ_promociones_articulos_producto
ON dbo.promociones_articulos(id_promocion, id_producto_shop)
WHERE id_producto_shop IS NOT NULL;
GO

CREATE UNIQUE INDEX UQ_promociones_articulos_combo
ON dbo.promociones_articulos(id_promocion, id_combo)
WHERE id_combo IS NOT NULL;
GO

--=========================================--
ALTER TABLE dbo.detalle_facturas
ADD
    id_combo INT NULL,
    descuento_unitario DECIMAL(10,2) NOT NULL
        CONSTRAINT DF_detalle_facturas_descuento
        DEFAULT (0),
    precio_final_unitario DECIMAL(10,2) NULL;
GO

ALTER TABLE dbo.detalle_facturas
ADD CONSTRAINT FK_detalle_facturas_combos
FOREIGN KEY (id_combo)
REFERENCES dbo.combos(id_combo);
GO

ALTER TABLE dbo.detalle_facturas
DROP CONSTRAINT CK_detalle_facturas_un_solo_concepto;
GO

ALTER TABLE dbo.detalle_facturas
ADD CONSTRAINT CK_detalle_facturas_un_solo_concepto
CHECK (
    (CASE WHEN id_entrada IS NOT NULL THEN 1 ELSE 0 END) +
    (CASE WHEN id_producto_shop IS NOT NULL THEN 1 ELSE 0 END) +
    (CASE WHEN id_promocion IS NOT NULL THEN 1 ELSE 0 END) +
    (CASE WHEN id_combo IS NOT NULL THEN 1 ELSE 0 END) = 1
);
GO
---===========================================--
ALTER TABLE dbo.detalle_facturas
ADD
    CONSTRAINT CK_detalle_facturas_descuento_no_negativo
        CHECK (descuento_unitario >= 0),

    CONSTRAINT CK_detalle_facturas_precio_final_no_negativo
        CHECK (
            precio_final_unitario IS NULL
            OR precio_final_unitario >= 0
        ),

    CONSTRAINT CK_detalle_facturas_descuento_no_supera_precio
        CHECK (descuento_unitario <= precio_unitario),

    CONSTRAINT CK_detalle_facturas_precio_final_correcto
        CHECK (
            precio_final_unitario IS NULL
            OR precio_final_unitario = precio_unitario - descuento_unitario
        );
GO

-------------------------------------------------

ALTER TABLE dbo.facturas_formas_pagos
ADD monto_pagado DECIMAL(10,2) NOT NULL
    CONSTRAINT DF_facturas_formas_pagos_monto
    DEFAULT (0);
GO

ALTER TABLE dbo.facturas_formas_pagos
ADD CONSTRAINT CK_facturas_formas_pagos_monto_positivo
CHECK (monto_pagado > 0);
GO
----------------------------------------
CREATE TABLE dbo.financieras_formas_pagos
(
    id_financiera_forma_pago INT IDENTITY(1,1) NOT NULL,
    id_financiera INT NOT NULL,
    id_forma_pago INT NOT NULL,

    CONSTRAINT PK_financieras_formas_pagos
        PRIMARY KEY (id_financiera_forma_pago),

    CONSTRAINT FK_financieras_formas_pagos_financieras
        FOREIGN KEY (id_financiera)
        REFERENCES dbo.financieras(id_financiera),

    CONSTRAINT FK_financieras_formas_pagos_formas
        FOREIGN KEY (id_forma_pago)
        REFERENCES dbo.formas_pagos(id_forma_pago),

    CONSTRAINT UQ_financieras_formas_pagos
        UNIQUE (id_financiera, id_forma_pago)
);
GO
----------------------------

INSERT INTO dbo.financieras_formas_pagos
    (id_financiera, id_forma_pago)
VALUES
    (1, 2),
    (1, 3), 
    (2, 2), 
    (2, 3); 
GO

-----------------------------------
ALTER TABLE dbo.facturas_formas_pagos
ADD id_financiera INT NULL;
GO

ALTER TABLE dbo.facturas_formas_pagos
ADD CONSTRAINT FK_facturas_formas_pagos_financieras
FOREIGN KEY (id_financiera)
REFERENCES dbo.financieras(id_financiera);
GO

ALTER TABLE dbo.facturas_formas_pagos
ADD CONSTRAINT CK_facturas_formas_pagos_financiera
CHECK (
    (id_forma_pago IN (1, 4) AND id_financiera IS NULL)
    OR
    (id_forma_pago IN (2, 3) AND id_financiera IS NOT NULL)
);
GO


CREATE OR ALTER TRIGGER dbo.TR_FACTURAS_FORMAS_PAGOS_VALIDAR_FINANCIERA
ON dbo.facturas_formas_pagos
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE i.id_financiera IS NOT NULL
          AND NOT EXISTS (
              SELECT 1
              FROM dbo.financieras_formas_pagos ffp
              WHERE ffp.id_financiera = i.id_financiera
                AND ffp.id_forma_pago = i.id_forma_pago
          )
    )
    BEGIN
        THROW 50030,
            'La financiera seleccionada no admite el medio de pago indicado.',
            1;
    END;
END;
GO

ALTER TABLE dbo.detalle_facturas
DROP CONSTRAINT CK_detalle_facturas_un_solo_concepto;
GO

ALTER TABLE dbo.detalle_facturas
ADD CONSTRAINT CK_detalle_facturas_un_solo_concepto
CHECK (
    (CASE WHEN id_entrada IS NOT NULL THEN 1 ELSE 0 END) +
    (CASE WHEN id_producto_shop IS NOT NULL THEN 1 ELSE 0 END) +
    (CASE WHEN id_combo IS NOT NULL THEN 1 ELSE 0 END) = 1
);
GO

ALTER TABLE dbo.detalle_facturas
ALTER COLUMN precio_final_unitario DECIMAL(10,2) NOT NULL;
GO



CREATE OR ALTER VIEW dbo.vw_Facturas_Total
AS
SELECT
    f.nro_factura,
    COALESCE(
        SUM(
            CAST(df.cantidad AS DECIMAL(10,2))
            * df.precio_final_unitario
        ),
        0
    ) AS total_factura
FROM dbo.facturas f
LEFT JOIN dbo.detalle_facturas df
    ON df.nro_factura = f.nro_factura
GROUP BY
    f.nro_factura;
GO


CREATE OR ALTER VIEW dbo.vw_Facturas_Total_Pagado
AS
SELECT
    f.nro_factura,
    COALESCE(SUM(ffp.monto_pagado), 0) AS total_pagado
FROM dbo.facturas f
LEFT JOIN dbo.facturas_formas_pagos ffp
    ON ffp.nro_factura = f.nro_factura
GROUP BY
    f.nro_factura;
GO


CREATE OR ALTER VIEW dbo.vw_Facturas_Resumen
AS
SELECT
    f.nro_factura,
    ec.estado,
    ft.total_factura,
    fp.total_pagado,
    CASE
        WHEN ft.total_factura > fp.total_pagado
            THEN ft.total_factura - fp.total_pagado
        ELSE 0
    END AS saldo_pendiente
FROM dbo.facturas f
INNER JOIN dbo.estados_compras ec
    ON ec.id_estado_compra = f.id_estado_compra
INNER JOIN dbo.vw_Facturas_Total ft
    ON ft.nro_factura = f.nro_factura
INNER JOIN dbo.vw_Facturas_Total_Pagado fp
    ON fp.nro_factura = f.nro_factura;
GO



ALTER TABLE dbo.facturas_formas_pagos
DROP CONSTRAINT UQ_facturas_formas_pagos;
GO

ALTER TABLE dbo.facturas_formas_pagos
ADD CONSTRAINT UQ_facturas_formas_pagos
UNIQUE
(
    nro_factura,
    id_forma_pago,
    id_financiera
);
GO

ALTER TABLE dbo.facturas_formas_pagos
DROP CONSTRAINT DF_facturas_formas_pagos_monto;
GO


ALTER TABLE dbo.facturas
DROP CONSTRAINT FK_facturas_financieras;
GO

ALTER TABLE dbo.facturas
DROP COLUMN id_financiera;
GO



CREATE OR ALTER TRIGGER dbo.TR_FACTURAS_VALIDAR_TRANSICION_ESTADO
ON dbo.facturas
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF UPDATE(id_estado_compra)
    BEGIN
        IF EXISTS (
            SELECT 1
            FROM inserted i
            INNER JOIN deleted d
                ON i.nro_factura = d.nro_factura
            WHERE
                i.id_estado_compra <> d.id_estado_compra
                AND NOT (
                    d.id_estado_compra = 1
                    AND i.id_estado_compra IN (2, 3)
                )
        )
        BEGIN
            THROW 50040,
                'Transicion de estado no permitida para la factura.',
                1;
        END;
    END;
END;
GO

CREATE OR ALTER TRIGGER dbo.TR_FACTURAS_FORMAS_PAGOS_VALIDAR_ESTADO
ON dbo.facturas_formas_pagos
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN dbo.facturas f
            ON f.nro_factura = i.nro_factura
        WHERE f.id_estado_compra = 3
    )
    BEGIN
        THROW 50041,
            'No se pueden registrar pagos en facturas anuladas.',
            1;
    END;
END;
GO

CREATE OR ALTER TRIGGER dbo.TR_FACTURAS_FORMAS_PAGOS_VALIDAR_ESTADO
ON dbo.facturas_formas_pagos
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN dbo.facturas f
            ON f.nro_factura = i.nro_factura
        WHERE f.id_estado_compra IN (2, 3)
    )
    BEGIN
        THROW 50041,
            'No se pueden registrar ni modificar pagos en facturas pagadas o anuladas.',
            1;
    END;
END;
GO

CREATE OR ALTER TRIGGER dbo.TR_FACTURAS_FORMAS_PAGOS_VALIDAR_ESTADO
ON dbo.facturas_formas_pagos
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM (
            SELECT nro_factura FROM inserted
            UNION
            SELECT nro_factura FROM deleted
        ) p
        INNER JOIN dbo.facturas f
            ON f.nro_factura = p.nro_factura
        WHERE f.id_estado_compra IN (2, 3)
    )
    BEGIN
        THROW 50041,
            'No se pueden modificar, agregar ni eliminar pagos de facturas pagadas o anuladas.',
            1;
    END;
END;
GO

-------================================================
CREATE OR ALTER TRIGGER dbo.TR_DETALLE_FACTURAS_VALIDAR_ESTADO
ON dbo.detalle_facturas
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM (
            SELECT nro_factura FROM inserted
            UNION
            SELECT nro_factura FROM deleted
        ) d
        INNER JOIN dbo.facturas f
            ON f.nro_factura = d.nro_factura
        WHERE f.id_estado_compra IN (2, 3)
    )
    BEGIN
        THROW 50042,
            'No se pueden agregar, modificar ni eliminar detalles de facturas pagadas o anuladas.',
            1;
    END;
END;
GO



CREATE OR ALTER TRIGGER dbo.TR_ENTRADAS_VALIDAR_FACTURACION
ON dbo.entradas
AFTER UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM deleted d
        INNER JOIN dbo.detalle_facturas df
            ON df.id_entrada = d.id_entrada
        INNER JOIN dbo.facturas f
            ON f.nro_factura = df.nro_factura
        WHERE f.id_estado_compra IN (2, 3)
    )
    BEGIN
        THROW 50043,
            'No se pueden modificar ni eliminar entradas asociadas a facturas pagadas o anuladas.',
            1;
    END;
END;
GO



CREATE OR ALTER TRIGGER dbo.TR_PRODUCTOS_SHOP_VALIDAR_FACTURACION
ON dbo.productos_shop
AFTER UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM deleted d
        INNER JOIN dbo.detalle_facturas df
            ON df.id_producto_shop = d.id_producto_shop
        INNER JOIN dbo.facturas f
            ON f.nro_factura = df.nro_factura
        WHERE f.id_estado_compra IN (2, 3)
    )
    BEGIN
        THROW 50044,
            'No se pueden modificar ni eliminar productos asociados a facturas pagadas o anuladas.',
            1;
    END;
END;
GO

CREATE OR ALTER TRIGGER dbo.TR_COMBOS_VALIDAR_FACTURACION
ON dbo.combos
AFTER UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM deleted d
        INNER JOIN dbo.detalle_facturas df
            ON df.id_combo = d.id_combo
        INNER JOIN dbo.facturas f
            ON f.nro_factura = df.nro_factura
        WHERE f.id_estado_compra IN (2, 3)
    )
    BEGIN
        THROW 50045,
            'No se pueden modificar ni eliminar combos asociados a facturas pagadas o anuladas.',
            1;
    END;
END;
GO



CREATE OR ALTER TRIGGER dbo.TR_PRODUCTOS_COMBOS_VALIDAR_FACTURACION
ON dbo.productos_combos
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM (
            SELECT id_combo FROM inserted
            UNION
            SELECT id_combo FROM deleted
        ) c
        INNER JOIN dbo.detalle_facturas df
            ON df.id_combo = c.id_combo
        INNER JOIN dbo.facturas f
            ON f.nro_factura = df.nro_factura
        WHERE f.id_estado_compra IN (2, 3)
    )
    BEGIN
        THROW 50046,
            'No se pueden modificar los componentes de combos asociados a facturas pagadas o anuladas.',
            1;
    END;
END;
GO


ALTER TABLE dbo.combos
ADD precio DECIMAL(10,2) NULL;
GO


UPDATE dbo.combos
SET precio = CASE
    WHEN id_combo = 1 THEN 5000.00
    WHEN id_combo = 2 THEN 10000.00
END
WHERE id_combo IN (1, 2);
GO

ALTER TABLE dbo.combos
ALTER COLUMN precio DECIMAL(10,2) NOT NULL;
GO

ALTER TABLE dbo.combos
ADD CONSTRAINT CK_combos_precio_no_negativo
CHECK (precio >= 0);
GO


CREATE OR ALTER TRIGGER dbo.TR_PRODUCTOS_SHOP_VALIDAR_FACTURACION
ON dbo.productos_shop
AFTER UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

  
    IF EXISTS (
        SELECT 1
        FROM deleted d
        INNER JOIN dbo.detalle_facturas df
            ON df.id_producto_shop = d.id_producto_shop
        INNER JOIN dbo.facturas f
            ON f.nro_factura = df.nro_factura
        WHERE f.id_estado_compra IN (2, 3)
          AND NOT EXISTS (
              SELECT 1
              FROM inserted i
              WHERE i.id_producto_shop = d.id_producto_shop
          )
    )
    BEGIN
        THROW 50044,
            'No se pueden eliminar productos asociados a facturas pagadas o anuladas.',
            1;
    END;

    IF UPDATE(nombre)
       OR UPDATE(id_marca)
       OR UPDATE(id_tipo_producto)
    BEGIN
        IF EXISTS (
            SELECT 1
            FROM inserted i
            INNER JOIN deleted d
                ON d.id_producto_shop = i.id_producto_shop
            INNER JOIN dbo.detalle_facturas df
                ON df.id_producto_shop = d.id_producto_shop
            INNER JOIN dbo.facturas f
                ON f.nro_factura = df.nro_factura
            WHERE f.id_estado_compra IN (2, 3)
              AND (
                    i.nombre <> d.nombre
                    OR i.id_marca <> d.id_marca
                    OR i.id_tipo_producto <> d.id_tipo_producto
              )
        )
        BEGIN
            THROW 50047,
                'No se pueden cambiar los datos identificatorios de productos asociados a facturas pagadas o anuladas.',
                1;
        END;
    END;
END;
GO


---------------------------------------===========------

CREATE TABLE dbo.tarifas_entradas (
    id_tipo_sala INT NOT NULL,
    precio DECIMAL(10,2) NOT NULL,

    CONSTRAINT PK_tarifas_entradas
        PRIMARY KEY (id_tipo_sala),

    CONSTRAINT FK_tarifas_entradas_tipos_salas
        FOREIGN KEY (id_tipo_sala)
        REFERENCES dbo.tipos_salas(id_tipo_sala),

    CONSTRAINT CK_tarifas_entradas_precio
        CHECK (precio > 0)
);
GO

INSERT INTO dbo.tarifas_entradas (id_tipo_sala, precio)
VALUES
    (1, 5000.00),
    (2, 7000.00),
    (3, 8000.00);
GO


CREATE TABLE dbo.adicionales_butacas (
    id_tipo_butacas INT NOT NULL,
    adicional DECIMAL(10,2) NOT NULL,

    CONSTRAINT PK_adicionales_butacas
        PRIMARY KEY (id_tipo_butacas),

    CONSTRAINT FK_adicionales_butacas_tipos
        FOREIGN KEY (id_tipo_butacas)
        REFERENCES dbo.tipos_butacas(id_tipo_butacas),

    CONSTRAINT CK_adicionales_butacas_no_negativo
        CHECK (adicional >= 0)
);
GO

INSERT INTO dbo.adicionales_butacas
    (id_tipo_butacas, adicional)
VALUES
    (1, 0.00),
    (2, 1500.00);
GO


CREATE TABLE dbo.promociones_tipos_productos (
    id_promocion INT NOT NULL,
    id_tipo_producto INT NOT NULL,

    CONSTRAINT PK_promociones_tipos_productos
        PRIMARY KEY (id_promocion, id_tipo_producto),

    CONSTRAINT FK_promociones_tipos_productos_promociones
        FOREIGN KEY (id_promocion)
        REFERENCES dbo.promociones(id_promocion),

    CONSTRAINT FK_promociones_tipos_productos_tipos
        FOREIGN KEY (id_tipo_producto)
        REFERENCES dbo.tipos_productos(id_tipo_producto)
);
GO

CREATE TABLE dbo.promociones_tipos_combos (
    id_promocion INT NOT NULL,
    id_tipo_combo INT NOT NULL,

    CONSTRAINT PK_promociones_tipos_combos
        PRIMARY KEY (id_promocion, id_tipo_combo),

    CONSTRAINT FK_promociones_tipos_combos_promociones
        FOREIGN KEY (id_promocion)
        REFERENCES dbo.promociones(id_promocion),

    CONSTRAINT FK_promociones_tipos_combos_tipos
        FOREIGN KEY (id_tipo_combo)
        REFERENCES dbo.tipos_combos(id_tipo_combo)
);
GO


CREATE TYPE dbo.TVP_EntradasVenta AS TABLE
(
    id_funcion INT NOT NULL,
    id_butaca INT NOT NULL,
    id_promocion INT NULL
);
GO


CREATE TYPE dbo.TVP_ProductosVenta AS TABLE
(
    id_producto_shop INT NOT NULL,
    cantidad INT NOT NULL,
    id_promocion INT NULL
);
GO

CREATE TYPE dbo.TVP_CombosVenta AS TABLE
(
    id_combo INT NOT NULL,
    cantidad INT NOT NULL,
    id_promocion INT NULL
);
GO

CREATE TYPE dbo.TVP_PagosVenta AS TABLE
(
    id_forma_pago INT NOT NULL,
    id_financiera INT NULL,
    monto_pagado DECIMAL(10,2) NOT NULL
);
GO



--=====================================--
CREATE OR ALTER PROCEDURE dbo.SP_REGISTRAR_VENTA
    @cod_cliente INT = NULL,
    @id_cajero INT,
    @id_sucursal INT,
    @id_condicion_IVA INT,
    @id_canal_venta INT,
    @id_reserva INT = NULL,

    @Entradas dbo.TVP_EntradasVenta READONLY,
    @Productos dbo.TVP_ProductosVenta READONLY,
    @Combos dbo.TVP_CombosVenta READONLY,
    @Pagos dbo.TVP_PagosVenta READONLY,

    @nro_factura INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @nro_factura = NULL;

    BEGIN TRY
        BEGIN TRANSACTION;

        ----------------------------------------------------
        -- 1. VALIDACIONES INICIALES
        ----------------------------------------------------

        IF @id_cajero IS NULL OR @id_sucursal IS NULL
            THROW 50001, 'Debe indicar cajero y sucursal.', 1;

        IF @id_condicion_IVA IS NULL
            THROW 50002, 'Debe indicar la condicion de IVA.', 1;

        IF NOT EXISTS (
            SELECT 1
            FROM dbo.condiciones_IVA
            WHERE id_condicion_IVA = @id_condicion_IVA
        )
            THROW 50003, 'La condicion de IVA indicada no existe.', 1;

        IF NOT EXISTS (
            SELECT 1
            FROM dbo.cajeros
            WHERE id_cajero = @id_cajero
              AND id_sucursal = @id_sucursal
        )
            THROW 50004, 'El cajero no pertenece a la sucursal indicada.', 1;

        IF @id_reserva IS NULL AND @cod_cliente IS NULL
            THROW 50005, 'Debe indicar un cliente para la venta.', 1;

        IF @id_reserva IS NOT NULL AND @cod_cliente IS NOT NULL
            THROW 50006, 'Al convertir una reserva, el cliente se obtiene de ella.', 1;

        IF NOT EXISTS (
            SELECT 1
            FROM dbo.canales_ventas
            WHERE id_canal_venta = @id_canal_venta
        )
            THROW 50007, 'El canal de venta no existe.', 1;

        IF NOT EXISTS (SELECT 1 FROM @Pagos)
            THROW 50008, 'Debe indicar al menos un medio de pago.', 1;

        IF EXISTS (
            SELECT 1
            FROM @Pagos
            WHERE monto_pagado <= 0
        )
            THROW 50009, 'Los importes de pago deben ser mayores a cero.', 1;

        IF EXISTS (
            SELECT 1
            FROM @Productos
            WHERE cantidad <= 0
        )
        OR EXISTS (
            SELECT 1
            FROM @Combos
            WHERE cantidad <= 0
        )
            THROW 50010, 'Las cantidades deben ser mayores a cero.', 1;

        ----------------------------------------------------
        -- 2. OBTENER CLIENTE Y ENTRADAS
        ----------------------------------------------------

        DECLARE @Cliente INT;
        DECLARE @EstadoReserva INT;
        DECLARE @FuncionReserva INT;

        CREATE TABLE #Tickets
        (
            id_funcion INT NOT NULL,
            id_butaca INT NOT NULL,
            id_promocion INT NULL,
            precio DECIMAL(10,2) NOT NULL,
            PRIMARY KEY (id_funcion, id_butaca)
        );

        IF @id_reserva IS NOT NULL
        BEGIN
            IF EXISTS (SELECT 1 FROM @Entradas)
                THROW 50011, 'No se pueden agregar entradas adicionales al convertir una reserva.', 1;

            SELECT
                @Cliente = r.cod_cliente,
                @EstadoReserva = r.id_estado_reserva,
                @FuncionReserva = r.id_funcion
            FROM dbo.reservas r WITH (UPDLOCK, HOLDLOCK)
            WHERE r.id_reserva = @id_reserva;

            IF @Cliente IS NULL
                THROW 50012, 'La reserva indicada no existe.', 1;

            IF @EstadoReserva NOT IN (1,2)
                THROW 50013, 'La reserva no se encuentra pendiente o confirmada.', 1;

            INSERT INTO #Tickets
            (
                id_funcion,
                id_butaca,
                id_promocion,
                precio
            )
            SELECT
                @FuncionReserva,
                rd.id_butaca,
                NULL,
                te.precio + ab.adicional
            FROM dbo.reserva_detalle rd
            INNER JOIN dbo.butacas b
                ON b.id_butaca = rd.id_butaca
            INNER JOIN dbo.salas s
                ON s.id_sala = b.id_sala
            INNER JOIN dbo.tarifas_entradas te
                ON te.id_tipo_sala = s.id_tipo_sala
            INNER JOIN dbo.adicionales_butacas ab
                ON ab.id_tipo_butacas = b.id_tipo_butacas
            WHERE rd.id_reserva = @id_reserva;

            IF NOT EXISTS (SELECT 1 FROM #Tickets)
                THROW 50014, 'La reserva no tiene entradas asociadas.', 1;
        END
        ELSE
        BEGIN
            SET @Cliente = @cod_cliente;

            INSERT INTO #Tickets
            (
                id_funcion,
                id_butaca,
                id_promocion,
                precio
            )
            SELECT
                ev.id_funcion,
                ev.id_butaca,
                ev.id_promocion,
                te.precio + ab.adicional
            FROM @Entradas ev
            INNER JOIN dbo.funciones f
                ON f.id_funcion = ev.id_funcion
            INNER JOIN dbo.salas s
                ON s.id_sala = f.id_sala
            INNER JOIN dbo.butacas b
                ON b.id_butaca = ev.id_butaca
            INNER JOIN dbo.tarifas_entradas te
                ON te.id_tipo_sala = s.id_tipo_sala
            INNER JOIN dbo.adicionales_butacas ab
                ON ab.id_tipo_butacas = b.id_tipo_butacas;
            
            IF (SELECT COUNT(*) FROM #Tickets) <>
               (SELECT COUNT(*) FROM @Entradas)
                THROW 50015, 'Una o mas entradas tienen funcion o butaca invalida.', 1;
        END;

        IF NOT EXISTS (
            SELECT 1 FROM #Tickets
        )
        AND NOT EXISTS (SELECT 1 FROM @Productos)
        AND NOT EXISTS (SELECT 1 FROM @Combos)
            THROW 50016, 'La venta debe contener al menos un articulo.', 1;

        IF NOT EXISTS (
            SELECT 1 FROM dbo.clientes WHERE cod_cliente = @Cliente
        )
            THROW 50017, 'El cliente indicado no existe.', 1;

        ----------------------------------------------------
        -- 3. VALIDAR PRODUCTOS Y COMBOS
        ----------------------------------------------------

        IF EXISTS (
            SELECT 1
            FROM @Productos p
            LEFT JOIN dbo.productos_shop ps
                ON ps.id_producto_shop = p.id_producto_shop
            WHERE ps.id_producto_shop IS NULL
        )
            THROW 50018, 'Existe un producto que no esta registrado.', 1;

        IF EXISTS (
            SELECT 1
            FROM @Combos c
            LEFT JOIN dbo.combos cb
                ON cb.id_combo = c.id_combo
            WHERE cb.id_combo IS NULL
        )
            THROW 50019, 'Existe un combo que no esta registrado.', 1;

        IF EXISTS (
            SELECT 1
            FROM @Combos c
            WHERE NOT EXISTS (
                SELECT 1
                FROM dbo.productos_combos pc
                WHERE pc.id_combo = c.id_combo
            )
        )
            THROW 50020, 'Existe un combo sin productos asociados.', 1;

        ----------------------------------------------------
        -- 4. CREAR FACTURA PENDIENTE
        ----------------------------------------------------

        INSERT INTO dbo.facturas
        (
            id_condicion_IVA,
            cod_cliente,
            id_cajero,
            id_sucursal,
            id_estado_compra,
            id_canal_venta
        )
        VALUES
        (
            @id_condicion_IVA,
            @Cliente,
            @id_cajero,
            @id_sucursal,
            1,
            @id_canal_venta
        );

        SET @nro_factura = CONVERT(INT, SCOPE_IDENTITY());
        IF @id_reserva IS NOT NULL
BEGIN
    UPDATE dbo.reservas
    SET id_estado_reserva = 4
    WHERE id_reserva = @id_reserva;
END;
        ----------------------------------------------------
        -- 5. GENERAR ENTRADAS
        ----------------------------------------------------

        INSERT INTO dbo.entradas
        (
            id_funcion,
            id_butaca,
            precio
        )
        SELECT
            id_funcion,
            id_butaca,
            precio
        FROM #Tickets;

        ----------------------------------------------------
        -- 6. ARMAR DETALLES DE FACTURA
        ----------------------------------------------------

        CREATE TABLE #Lineas
        (
            id_linea INT IDENTITY(1,1) PRIMARY KEY,
            tipo CHAR(1) NOT NULL,
            id_entrada INT NULL,
            id_producto_shop INT NULL,
            id_combo INT NULL,
            id_promocion INT NULL,
            id_funcion INT NULL,
            id_horario INT NULL,
            cantidad INT NOT NULL,
            precio_unitario DECIMAL(10,2) NOT NULL,
            descuento_unitario DECIMAL(10,2) NOT NULL DEFAULT 0
        );

        -- Entradas
        INSERT INTO #Lineas
        (
            tipo,
            id_entrada,
            id_promocion,
            id_funcion,
            id_horario,
            cantidad,
            precio_unitario
        )
        SELECT
            'E',
            e.id_entrada,
            t.id_promocion,
            t.id_funcion,
            f.id_horario,
            1,
            t.precio
        FROM #Tickets t
        INNER JOIN dbo.entradas e
            ON e.id_funcion = t.id_funcion
           AND e.id_butaca = t.id_butaca
        INNER JOIN dbo.funciones f
            ON f.id_funcion = t.id_funcion;

        -- Productos
        INSERT INTO #Lineas
        (
            tipo,
            id_producto_shop,
            id_promocion,
            cantidad,
            precio_unitario
        )
        SELECT
            'P',
            ps.id_producto_shop,
            p.id_promocion,
            p.cantidad,
            ps.precio
        FROM @Productos p
        INNER JOIN dbo.productos_shop ps
            ON ps.id_producto_shop = p.id_producto_shop;

        -- Combos
        INSERT INTO #Lineas
        (
            tipo,
            id_combo,
            id_promocion,
            cantidad,
            precio_unitario
        )
        SELECT
            'C',
            cb.id_combo,
            c.id_promocion,
            c.cantidad,
            cb.precio
        FROM @Combos c
        INNER JOIN dbo.combos cb
            ON cb.id_combo = c.id_combo;

        ----------------------------------------------------
        -- 7. CALCULAR SUBTOTAL ANTES DE DESCUENTOS
        ----------------------------------------------------

        DECLARE @Subtotal DECIMAL(12,2);

        SELECT
            @Subtotal = SUM(cantidad * precio_unitario)
        FROM #Lineas;

        ----------------------------------------------------
        -- 8. VALIDAR Y APLICAR PROMOCIONES
        ----------------------------------------------------

        DECLARE
            @Linea INT,
            @Tipo CHAR(1),
            @Promocion INT,
            @Porcentaje DECIMAL(5,2),
            @MontoMinimo DECIMAL(10,2),
            @TipoPromocion INT,
            @EstadoPromocion INT,
            @DiaPromocion INT,
            @Financiera INT,
            @CanalPromocion INT,
            @ClientePromocion INT,
            @FechaInicio DATE,
            @FechaFin DATE,
            @FechaVenta DATE,
            @Precio DECIMAL(10,2),
            @Descuento DECIMAL(10,2),
            @Entrada INT,
            @Producto INT,
            @Combo INT,
            @Funcion INT,
            @Horario INT,
            @Coincide BIT,
            @TieneRestricciones BIT;

        SET @FechaVenta = CONVERT(DATE, SYSDATETIME());

        DECLARE cur_promociones CURSOR LOCAL FAST_FORWARD FOR
            SELECT
                id_linea,
                tipo,
                id_promocion,
                id_entrada,
                id_producto_shop,
                id_combo,
                id_funcion,
                id_horario,
                precio_unitario
            FROM #Lineas
            WHERE id_promocion IS NOT NULL;

        OPEN cur_promociones;

        FETCH NEXT FROM cur_promociones INTO
            @Linea, @Tipo, @Promocion, @Entrada,
            @Producto, @Combo, @Funcion, @Horario, @Precio;

        WHILE @@FETCH_STATUS = 0
        BEGIN
        SET @Porcentaje = NULL;
SET @MontoMinimo = NULL;
SET @TipoPromocion = NULL;
SET @EstadoPromocion = NULL;
SET @DiaPromocion = NULL;
SET @Financiera = NULL;
SET @CanalPromocion = NULL;
SET @ClientePromocion = NULL;
SET @FechaInicio = NULL;
SET @FechaFin = NULL;
            SELECT
                @Porcentaje = porcentaje_descuento,
                @MontoMinimo = monto_minimo,
                @TipoPromocion = id_tipo_promocion,
                @EstadoPromocion = id_estado_promocion,
                @DiaPromocion = id_dia_promocion,
                @Financiera = id_financiera,
                @CanalPromocion = id_canal_venta,
                @ClientePromocion = cod_cliente,
                @FechaInicio = fecha_inicio,
                @FechaFin = fecha_fin
            FROM dbo.promociones
            WHERE id_promocion = @Promocion;

            IF @EstadoPromocion IS NULL
                THROW 50021, 'Una promocion indicada no existe.', 1;

            IF @EstadoPromocion <> 1
                THROW 50022, 'La promocion no esta activa.', 1;

            IF @FechaVenta NOT BETWEEN @FechaInicio AND @FechaFin
                THROW 50023, 'La promocion esta fuera de su periodo de vigencia.', 1;

            IF @MontoMinimo > @Subtotal
                THROW 50024, 'La compra no alcanza el monto minimo de la promocion.', 1;

            IF @CanalPromocion IS NOT NULL
               AND @CanalPromocion <> @id_canal_venta
                THROW 50025, 'La promocion no corresponde al canal de venta.', 1;

            IF @ClientePromocion IS NOT NULL
               AND @ClientePromocion <> @Cliente
                THROW 50026, 'La promocion no corresponde al cliente.', 1;

            IF @DiaPromocion <> 1
               AND @DiaPromocion <> (
                    ((DATEPART(WEEKDAY, @FechaVenta) + @@DATEFIRST - 2) % 7) + 2
               )
                THROW 50027, 'La promocion no corresponde al dia actual.', 1;

            -- Restricciones de articulos y categorias
            SET @TieneRestricciones = 0;
            SET @Coincide = 0;

            IF EXISTS (
                SELECT 1
                FROM dbo.promociones_articulos
                WHERE id_promocion = @Promocion
            )
            OR EXISTS (
                SELECT 1
                FROM dbo.promociones_tipos_productos
                WHERE id_promocion = @Promocion
            )
            OR EXISTS (
                SELECT 1
                FROM dbo.promociones_tipos_combos
                WHERE id_promocion = @Promocion
            )
                SET @TieneRestricciones = 1;

            IF @TieneRestricciones = 0
                SET @Coincide = 1;
            ELSE
            BEGIN
               IF @Tipo = 'E'
BEGIN
    IF EXISTS (
        SELECT 1
        FROM dbo.promociones_funciones pf
        WHERE pf.id_promocion = @Promocion
          AND pf.id_funcion = @Funcion
    )
        SET @Coincide = 1;
END;

                IF @Tipo = 'P'
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM dbo.promociones_articulos pa
                        WHERE pa.id_promocion = @Promocion
                          AND pa.id_producto_shop = @Producto
                    )
                    OR EXISTS (
                        SELECT 1
                        FROM dbo.promociones_tipos_productos pt
                        INNER JOIN dbo.productos_shop ps
                            ON ps.id_tipo_producto = pt.id_tipo_producto
                        WHERE pt.id_promocion = @Promocion
                          AND ps.id_producto_shop = @Producto
                    )
                        SET @Coincide = 1;
                END;

                IF @Tipo = 'C'
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM dbo.promociones_articulos pa
                        WHERE pa.id_promocion = @Promocion
                          AND pa.id_combo = @Combo
                    )
                    OR EXISTS (
                        SELECT 1
                        FROM dbo.promociones_tipos_combos pt
                        INNER JOIN dbo.combos cb
                            ON cb.id_tipo_combo = pt.id_tipo_combo
                        WHERE pt.id_promocion = @Promocion
                          AND cb.id_combo = @Combo
                    )
                        SET @Coincide = 1;
                END;
            END;

            IF @Coincide = 0
                THROW 50028, 'La promocion no corresponde al articulo vendido.', 1;

            -- Restricciones por funcion
            IF EXISTS (
                SELECT 1
                FROM dbo.promociones_funciones
                WHERE id_promocion = @Promocion
            )
            AND NOT EXISTS (
                SELECT 1
                FROM dbo.promociones_funciones
                WHERE id_promocion = @Promocion
                  AND id_funcion = @Funcion
            )
                THROW 50029, 'La promocion no corresponde a esta funcion.', 1;

            -- Restricciones por horario
            IF EXISTS (
                SELECT 1
                FROM dbo.promociones_horarios
                WHERE id_promocion = @Promocion
            )
            AND NOT EXISTS (
                SELECT 1
                FROM dbo.promociones_horarios
                WHERE id_promocion = @Promocion
                  AND id_horario = @Horario
            )
                THROW 50030, 'La promocion no corresponde a este horario.', 1;

            -- Beneficio bancario: todo el pago debe ser con la
            -- tarjeta de la financiera asociada.
            IF @TipoPromocion = 2
            BEGIN
                IF @Financiera IS NULL
                    THROW 50031, 'El beneficio bancario no tiene financiera asociada.', 1;

                IF EXISTS (
                    SELECT 1
                    FROM @Pagos
                    WHERE id_forma_pago NOT IN (2,3)
                       OR id_financiera IS NULL
                       OR id_financiera <> @Financiera
                )
                    THROW 50032, 'El beneficio bancario exige pagar todo con tarjeta de la financiera correspondiente.', 1;
            END
            ELSE IF @TipoPromocion <> 1
                THROW 50033, 'El tipo de promocion no es valido.', 1;

            SET @Descuento = ROUND(@Precio * @Porcentaje / 100.0, 2);

            UPDATE #Lineas
            SET descuento_unitario = @Descuento
            WHERE id_linea = @Linea;

            FETCH NEXT FROM cur_promociones INTO
                @Linea, @Tipo, @Promocion, @Entrada,
                @Producto, @Combo, @Funcion, @Horario, @Precio;
        END;

        CLOSE cur_promociones;
        DEALLOCATE cur_promociones;

        ----------------------------------------------------
        -- 9. ACTUALIZAR STOCK DE PRODUCTOS Y COMPONENTES
        ----------------------------------------------------

        CREATE TABLE #Consumo
        (
            id_producto_shop INT PRIMARY KEY,
            cantidad INT NOT NULL
        );

        INSERT INTO #Consumo
        (
            id_producto_shop,
            cantidad
        )
        SELECT
            id_producto_shop,
            SUM(cantidad)
        FROM
        (
            SELECT
                id_producto_shop,
                cantidad
            FROM @Productos

            UNION ALL

            SELECT
                pc.id_producto_shop,
                pc.cantidad * c.cantidad
            FROM @Combos c
            INNER JOIN dbo.productos_combos pc
                ON pc.id_combo = c.id_combo
        ) x
        GROUP BY id_producto_shop;

        IF EXISTS (
            SELECT 1
            FROM dbo.productos_shop ps WITH (UPDLOCK, HOLDLOCK)
            INNER JOIN #Consumo c
                ON c.id_producto_shop = ps.id_producto_shop
            WHERE ps.stock < c.cantidad
        )
            THROW 50034, 'No hay stock suficiente para completar la venta.', 1;

        UPDATE ps
        SET ps.stock = ps.stock - c.cantidad
        FROM dbo.productos_shop ps
        INNER JOIN #Consumo c
            ON c.id_producto_shop = ps.id_producto_shop;

        ----------------------------------------------------
        -- 10. INSERTAR DETALLES DE FACTURA
        ----------------------------------------------------

        INSERT INTO dbo.detalle_facturas
        (
            nro_factura,
            id_entrada,
            id_producto_shop,
            id_promocion,
            cantidad,
            precio_unitario,
            id_combo,
            descuento_unitario,
            precio_final_unitario
        )
        SELECT
            @nro_factura,
            id_entrada,
            id_producto_shop,
            id_promocion,
            cantidad,
            precio_unitario,
            id_combo,
            descuento_unitario,
            precio_unitario - descuento_unitario
        FROM #Lineas;

        ----------------------------------------------------
        -- 11. CALCULAR TOTAL FINAL
        ----------------------------------------------------

        DECLARE @TotalFactura DECIMAL(12,2);
        DECLARE @TotalPagos DECIMAL(12,2);

        SELECT
            @TotalFactura = SUM(
                cantidad * (precio_unitario - descuento_unitario)
            )
        FROM #Lineas;

        SELECT
            @TotalPagos = SUM(monto_pagado)
        FROM @Pagos;

        IF @TotalFactura IS NULL OR @TotalFactura <= 0
            THROW 50035, 'El total de la factura debe ser mayor a cero.', 1;

        IF @TotalPagos <> @TotalFactura
            THROW 50036, 'El importe abonado debe coincidir exactamente con el total de la factura.', 1;

        ----------------------------------------------------
        -- 12. REGISTRAR PAGOS
        ----------------------------------------------------

        INSERT INTO dbo.facturas_formas_pagos
        (
            nro_factura,
            id_forma_pago,
            monto_pagado,
            id_financiera
        )
        SELECT
            @nro_factura,
            id_forma_pago,
            monto_pagado,
            id_financiera
        FROM @Pagos;

      

        ----------------------------------------------------
        -- 14. MARCAR FACTURA COMO PAGADA
        ----------------------------------------------------

        UPDATE dbo.facturas
        SET id_estado_compra = 2
        WHERE nro_factura = @nro_factura;

        COMMIT TRANSACTION;

        SELECT
            @nro_factura AS nro_factura,
            @TotalFactura AS total_factura,
            @TotalPagos AS total_pagado,
            'Venta registrada correctamente' AS resultado;

    END TRY
    BEGIN CATCH

        IF CURSOR_STATUS('local', 'cur_promociones') >= 0
            CLOSE cur_promociones;

        IF CURSOR_STATUS('local', 'cur_promociones') >= -1
            DEALLOCATE cur_promociones;

        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        SET @nro_factura = NULL;

        THROW;

    END CATCH;
END;
GO





BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @id_usuario INT;
    DECLARE @cod_cliente INT;
    DECLARE @id_cajero INT;

    -- Usuario de prueba
    INSERT INTO dbo.usuarios
    (
        nombre_usuario,
        contraseña,
        id_rol,
        correo_electronico
    )
    VALUES
    (
        'cliente_prueba_cine',
        'TEMPORAL_CAMBIAR_EN_API',
        1,
        'cliente.prueba@cine.local'
    );

    SET @id_usuario = SCOPE_IDENTITY();

    -- Cliente de prueba
    INSERT INTO dbo.clientes
    (
        nombre,
        apellido,
        nro_doc,
        id_tipo_doc,
        id_direccion,
        id_usuario
    )
    VALUES
    (
        'Cliente',
        'Prueba',
        '99999999',
        1,
        1,
        @id_usuario
    );

    SET @cod_cliente = SCOPE_IDENTITY();

    -- Cajero de prueba
    INSERT INTO dbo.cajeros
    (
        nombre,
        apellido,
        id_turno,
        id_sucursal
    )
    VALUES
    (
        'Cajero',
        'Prueba',
        1,
        1
    );

    SET @id_cajero = SCOPE_IDENTITY();

    COMMIT TRANSACTION;

    SELECT
        @id_usuario AS id_usuario,
        @cod_cliente AS cod_cliente,
        @id_cajero AS id_cajero;

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;

GO



CREATE OR ALTER PROCEDURE dbo.SP_ANULAR_FACTURA
    @nro_factura INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @Estado INT;

        SELECT
            @Estado = id_estado_compra
        FROM dbo.facturas WITH (UPDLOCK, HOLDLOCK)
        WHERE nro_factura = @nro_factura;

        IF @Estado IS NULL
            THROW 50100, 'La factura indicada no existe.', 1;

        IF @Estado <> 1
            THROW 50101, 'Solo se pueden anular facturas pendientes.', 1;

        UPDATE dbo.facturas
        SET id_estado_compra = 3
        WHERE nro_factura = @nro_factura;

        COMMIT TRANSACTION;

        SELECT
            @nro_factura AS nro_factura,
            'Factura anulada correctamente' AS resultado;

    END TRY
    BEGIN CATCH

        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH;
END;
GO




CREATE OR ALTER PROCEDURE dbo.SP_CONSULTAR_FACTURA
    @nro_factura INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.facturas
        WHERE nro_factura = @nro_factura
    )
        THROW 50110, 'La factura indicada no existe.', 1;

    -- 1. Cabecera de factura
    SELECT
        f.nro_factura,
        f.fecha,
        f.cod_cliente,
        c.nombre AS nombre_cliente,
        c.apellido AS apellido_cliente,
        c.nro_doc,
        ci.condicion AS condicion_IVA,
        f.id_cajero,
        ca.nombre AS nombre_cajero,
        ca.apellido AS apellido_cajero,
        f.id_sucursal,
        s.nombre AS sucursal,
        f.id_canal_venta,
        cv.canal_venta,
        f.id_estado_compra,
        ec.estado AS estado_compra,
        vt.total_factura
    FROM dbo.facturas f
    INNER JOIN dbo.clientes c
        ON c.cod_cliente = f.cod_cliente
    INNER JOIN dbo.condiciones_IVA ci
        ON ci.id_condicion_IVA = f.id_condicion_IVA
    INNER JOIN dbo.cajeros ca
        ON ca.id_cajero = f.id_cajero
    INNER JOIN dbo.sucursales s
        ON s.id_sucursal = f.id_sucursal
    INNER JOIN dbo.canales_ventas cv
        ON cv.id_canal_venta = f.id_canal_venta
    INNER JOIN dbo.estados_compras ec
        ON ec.id_estado_compra = f.id_estado_compra
    LEFT JOIN dbo.vw_Facturas_Total vt
        ON vt.nro_factura = f.nro_factura
    WHERE f.nro_factura = @nro_factura;

    -- 2. Detalles de factura
   SELECT
    df.id_detalle_factura,

    -- Entrada
    df.id_entrada,
    en.id_funcion,
    pe.titulo AS pelicula,
    fu.fecha AS fecha_funcion,
    ho.horario,
    sa.nro_sala,
    bu.id_butaca,

    -- Producto
    df.id_producto_shop,
    ps.nombre AS producto,

    -- Combo
    df.id_combo,
    cb.descripcion AS combo,

    -- Promocion
    df.id_promocion,
    pr.descripcion AS promocion,

    -- Importes
    df.cantidad,
    df.precio_unitario,
    df.descuento_unitario,
    df.precio_final_unitario,
    df.cantidad * df.precio_final_unitario AS subtotal

FROM dbo.detalle_facturas df

LEFT JOIN dbo.entradas en
    ON en.id_entrada = df.id_entrada

LEFT JOIN dbo.funciones fu
    ON fu.id_funcion = en.id_funcion

LEFT JOIN dbo.peliculas pe
    ON pe.id_pelicula = fu.id_pelicula

LEFT JOIN dbo.horarios ho
    ON ho.id_horario = fu.id_horario

LEFT JOIN dbo.salas sa
    ON sa.id_sala = fu.id_sala

LEFT JOIN dbo.butacas bu
    ON bu.id_butaca = en.id_butaca

LEFT JOIN dbo.productos_shop ps
    ON ps.id_producto_shop = df.id_producto_shop

LEFT JOIN dbo.combos cb
    ON cb.id_combo = df.id_combo

LEFT JOIN dbo.promociones pr
    ON pr.id_promocion = df.id_promocion

WHERE df.nro_factura = @nro_factura

ORDER BY df.id_detalle_factura;
    -- 3. Pagos de factura
    SELECT
        fp.id_factura_forma_pago,
        fp.id_forma_pago,
        mp.forma_pago,
        fp.id_financiera,
        fi.financiera,
        fp.monto_pagado
    FROM dbo.facturas_formas_pagos fp
    INNER JOIN dbo.formas_pagos mp
        ON mp.id_forma_pago = fp.id_forma_pago
    LEFT JOIN dbo.financieras fi
        ON fi.id_financiera = fp.id_financiera
    WHERE fp.nro_factura = @nro_factura
    ORDER BY fp.id_factura_forma_pago;

END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_LISTAR_FUNCIONES_DISPONIBLES
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        f.id_funcion,
        f.fecha,
        h.horario,
        p.id_pelicula,
        p.titulo AS pelicula,
        s.id_sala,
        s.nro_sala,
        su.id_sucursal,
        su.nombre AS sucursal,
        ts.tipo_sala,
        COUNT(b.id_butaca) AS total_butacas,
        SUM(
            CASE
                WHEN e.id_entrada IS NULL
                 AND r.id_reserva IS NULL
                THEN 1
                ELSE 0
            END
        ) AS butacas_disponibles
    FROM dbo.funciones f
    INNER JOIN dbo.horarios h
        ON h.id_horario = f.id_horario
    INNER JOIN dbo.peliculas p
        ON p.id_pelicula = f.id_pelicula
    INNER JOIN dbo.salas s
        ON s.id_sala = f.id_sala
    INNER JOIN dbo.sucursales su
        ON su.id_sucursal = s.id_sucursal
    INNER JOIN dbo.tipos_salas ts
        ON ts.id_tipo_sala = s.id_tipo_sala
    INNER JOIN dbo.butacas b
        ON b.id_sala = s.id_sala
    LEFT JOIN dbo.entradas e
        ON e.id_funcion = f.id_funcion
       AND e.id_butaca = b.id_butaca
    LEFT JOIN dbo.reserva_detalle rd
        ON rd.id_butaca = b.id_butaca
    LEFT JOIN dbo.reservas r
        ON r.id_reserva = rd.id_reserva
       AND r.id_funcion = f.id_funcion
       AND r.id_estado_reserva IN (1, 2)
    WHERE f.fecha >= CONVERT(DATE, GETDATE())
    GROUP BY
        f.id_funcion,
        f.fecha,
        h.horario,
        p.id_pelicula,
        p.titulo,
        s.id_sala,
        s.nro_sala,
        su.id_sucursal,
        su.nombre,
        ts.tipo_sala
    ORDER BY
        f.fecha,
        h.horario,
        p.titulo;
END;
GO


CREATE OR ALTER PROCEDURE dbo.SP_CREAR_RESERVA
    @id_funcion INT,
    @cod_cliente INT,
    @Butacas dbo.TVP_EntradasVenta READONLY,
    @id_reserva INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

       IF NOT EXISTS (
    SELECT 1
    FROM dbo.funciones f
    INNER JOIN dbo.horarios h
        ON h.id_horario = f.id_horario
    WHERE f.id_funcion = @id_funcion
      AND DATEADD(
            SECOND,
            DATEDIFF(SECOND, CAST('00:00:00' AS TIME), h.horario),
            CAST(f.fecha AS DATETIME2)
          ) > SYSDATETIME()
)
    THROW 50120, 'La funcion no existe o ya comenzo.', 1;

        IF NOT EXISTS (
            SELECT 1
            FROM dbo.clientes
            WHERE cod_cliente = @cod_cliente
        )
            THROW 50121, 'El cliente indicado no existe.', 1;

        IF NOT EXISTS (SELECT 1 FROM @Butacas)
            THROW 50122, 'Debe indicar al menos una butaca.', 1;

        IF EXISTS (
            SELECT 1
            FROM @Butacas
            WHERE id_funcion <> @id_funcion
               OR id_promocion IS NOT NULL
        )
            THROW 50123, 'Las butacas deben pertenecer a la funcion indicada y no deben incluir promociones.', 1;

        IF EXISTS (
            SELECT id_butaca
            FROM @Butacas
            GROUP BY id_butaca
            HAVING COUNT(*) > 1
        )
            THROW 50124, 'No puede incluir la misma butaca mas de una vez.', 1;

        DECLARE @Monto DECIMAL(10,2);

        SELECT @Monto = SUM(t.precio + ab.adicional)
        FROM @Butacas b
        INNER JOIN dbo.funciones f
            ON f.id_funcion = b.id_funcion
        INNER JOIN dbo.salas s
            ON s.id_sala = f.id_sala
        INNER JOIN dbo.tarifas_entradas t
            ON t.id_tipo_sala = s.id_tipo_sala
        INNER JOIN dbo.butacas bu
            ON bu.id_butaca = b.id_butaca
           AND bu.id_sala = s.id_sala
        INNER JOIN dbo.adicionales_butacas ab
            ON ab.id_tipo_butacas = bu.id_tipo_butacas;

        IF @Monto IS NULL
            THROW 50125, 'No se pudieron calcular las tarifas de las butacas.', 1;

        INSERT INTO dbo.reservas (
            id_funcion,
            cod_cliente,
            id_estado_reserva,
            monto_reserva
        )
        VALUES (
            @id_funcion,
            @cod_cliente,
            1,
            @Monto
        );

        SET @id_reserva = CONVERT(INT, SCOPE_IDENTITY());

        INSERT INTO dbo.reserva_detalle (
            id_reserva,
            id_butaca
        )
        SELECT
            @id_reserva,
            id_butaca
        FROM @Butacas;

        COMMIT TRANSACTION;

        SELECT
            @id_reserva AS id_reserva,
            @Monto AS monto_reserva,
            'Reserva creada correctamente.' AS resultado;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;
GO



CREATE OR ALTER PROCEDURE dbo.SP_ACTUALIZAR_ESTADO_RESERVA
    @id_reserva INT,
    @id_estado_reserva INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @EstadoActual INT;

        SELECT @EstadoActual = id_estado_reserva
        FROM dbo.reservas WITH (UPDLOCK, HOLDLOCK)
        WHERE id_reserva = @id_reserva;

        IF @EstadoActual IS NULL
            THROW 50130, 'La reserva indicada no existe.', 1;

        IF @id_estado_reserva NOT IN (1, 2, 3, 4)
            THROW 50131, 'El estado indicado no es valido.', 1;

        IF @EstadoActual = @id_estado_reserva
        BEGIN
            COMMIT TRANSACTION;
            SELECT 'La reserva ya se encuentra en ese estado.' AS resultado;
            RETURN;
        END;

        IF NOT (
            (@EstadoActual = 1 AND @id_estado_reserva IN (2, 3, 4))
            OR
            (@EstadoActual = 2 AND @id_estado_reserva IN (3, 4))
        )
            THROW 50132, 'La transicion de estado solicitada no esta permitida.', 1;

        UPDATE dbo.reservas
        SET id_estado_reserva = @id_estado_reserva
        WHERE id_reserva = @id_reserva;

        COMMIT TRANSACTION;

        SELECT
            @id_reserva AS id_reserva,
            @id_estado_reserva AS id_estado_reserva,
            'Estado de reserva actualizado correctamente.' AS resultado;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;
GO



CREATE OR ALTER PROCEDURE dbo.SP_LISTAR_PRODUCTOS_SHOP
    @busqueda VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_producto_shop,
        nombre,
        descripcion,
        precio,
        stock,
        id_marca,
        id_tipo_producto
    FROM dbo.productos_shop
    WHERE stock > 0
      AND (
          @busqueda IS NULL
          OR nombre LIKE '%' + @busqueda + '%'
      )
    ORDER BY nombre;
END;
GO


CREATE OR ALTER PROCEDURE dbo.SP_LISTAR_COMBOS
    @busqueda VARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.id_combo,
        c.descripcion,
        c.precio,
        pc.id_producto_shop,
        ps.nombre AS producto,
        pc.cantidad,
        pc.precio_unitario AS precio_referencia
    FROM dbo.combos c
    LEFT JOIN dbo.productos_combos pc
        ON pc.id_combo = c.id_combo
    LEFT JOIN dbo.productos_shop ps
        ON ps.id_producto_shop = pc.id_producto_shop
    WHERE
        @busqueda IS NULL
        OR c.descripcion LIKE '%' + @busqueda + '%'
    ORDER BY c.descripcion, ps.nombre;
END;
GO



CREATE OR ALTER PROCEDURE dbo.SP_CREAR_PELICULA
    @titulo VARCHAR(150),
    @descripcion VARCHAR(500) = NULL,
    @fecha_lanzamiento DATE,
    @id_pais INT = NULL,
    @id_formato_pelicula INT = NULL,
    @id_clasificacion INT = NULL,
    @id_distribuidor INT = NULL,
    @id_director INT = NULL,
    @id_pelicula INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @titulo IS NULL OR LEN(LTRIM(RTRIM(@titulo))) = 0
        THROW 50140, 'El titulo de la pelicula es obligatorio.', 1;

    IF @id_pais IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM dbo.paises WHERE id_pais = @id_pais)
        THROW 50141, 'El pais indicado no existe.', 1;

    IF @id_formato_pelicula IS NOT NULL
       AND NOT EXISTS (
           SELECT 1 FROM dbo.formatos_peliculas
           WHERE id_formato_pelicula = @id_formato_pelicula
       )
        THROW 50142, 'El formato de pelicula indicado no existe.', 1;

    IF @id_clasificacion IS NOT NULL
       AND NOT EXISTS (
           SELECT 1 FROM dbo.clasificaciones
           WHERE id_clasificacion = @id_clasificacion
       )
        THROW 50143, 'La clasificacion indicada no existe.', 1;

    IF @id_distribuidor IS NOT NULL
       AND NOT EXISTS (
           SELECT 1 FROM dbo.distribuidores
           WHERE id_distribuidor = @id_distribuidor
       )
        THROW 50144, 'El distribuidor indicado no existe.', 1;

    IF @id_director IS NOT NULL
       AND NOT EXISTS (
           SELECT 1 FROM dbo.directores
           WHERE id_director = @id_director
       )
        THROW 50145, 'El director indicado no existe.', 1;

    INSERT INTO dbo.peliculas (
        titulo,
        descripcion,
        fecha_lanzamiento,
        id_pais,
        id_formato_pelicula,
        id_clasificacion,
        id_distribuidor,
        id_director
    )
    VALUES (
        LTRIM(RTRIM(@titulo)),
        @descripcion,
        @fecha_lanzamiento,
        @id_pais,
        @id_formato_pelicula,
        @id_clasificacion,
        @id_distribuidor,
        @id_director
    );

    SET @id_pelicula = CONVERT(INT, SCOPE_IDENTITY());

    SELECT
        @id_pelicula AS id_pelicula,
        'Pelicula creada correctamente.' AS resultado;
END;
GO



CREATE OR ALTER PROCEDURE dbo.SP_MODIFICAR_PELICULA
    @id_pelicula INT,
    @titulo VARCHAR(150),
    @descripcion VARCHAR(500) = NULL,
    @fecha_lanzamiento DATE,
    @id_pais INT = NULL,
    @id_formato_pelicula INT = NULL,
    @id_clasificacion INT = NULL,
    @id_distribuidor INT = NULL,
    @id_director INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.peliculas
        WHERE id_pelicula = @id_pelicula
    )
        THROW 50150, 'La pelicula indicada no existe.', 1;

    IF @titulo IS NULL OR LEN(LTRIM(RTRIM(@titulo))) = 0
        THROW 50151, 'El titulo de la pelicula es obligatorio.', 1;

    IF @id_pais IS NOT NULL
       AND NOT EXISTS (SELECT 1 FROM dbo.paises WHERE id_pais = @id_pais)
        THROW 50152, 'El pais indicado no existe.', 1;

    IF @id_formato_pelicula IS NOT NULL
       AND NOT EXISTS (
           SELECT 1 FROM dbo.formatos_peliculas
           WHERE id_formato_pelicula = @id_formato_pelicula
       )
        THROW 50153, 'El formato de pelicula indicado no existe.', 1;

    IF @id_clasificacion IS NOT NULL
       AND NOT EXISTS (
           SELECT 1 FROM dbo.clasificaciones
           WHERE id_clasificacion = @id_clasificacion
       )
        THROW 50154, 'La clasificacion indicada no existe.', 1;

    IF @id_distribuidor IS NOT NULL
       AND NOT EXISTS (
           SELECT 1 FROM dbo.distribuidores
           WHERE id_distribuidor = @id_distribuidor
       )
        THROW 50155, 'El distribuidor indicado no existe.', 1;

    IF @id_director IS NOT NULL
       AND NOT EXISTS (
           SELECT 1 FROM dbo.directores
           WHERE id_director = @id_director
       )
        THROW 50156, 'El director indicado no existe.', 1;

    UPDATE dbo.peliculas
    SET
        titulo = LTRIM(RTRIM(@titulo)),
        descripcion = @descripcion,
        fecha_lanzamiento = @fecha_lanzamiento,
        id_pais = @id_pais,
        id_formato_pelicula = @id_formato_pelicula,
        id_clasificacion = @id_clasificacion,
        id_distribuidor = @id_distribuidor,
        id_director = @id_director
    WHERE id_pelicula = @id_pelicula;

    SELECT
        @id_pelicula AS id_pelicula,
        'Pelicula modificada correctamente.' AS resultado;
END;
GO




CREATE OR ALTER PROCEDURE dbo.SP_ELIMINAR_PELICULA
    @id_pelicula INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (
            SELECT 1
            FROM dbo.peliculas WITH (UPDLOCK, HOLDLOCK)
            WHERE id_pelicula = @id_pelicula
        )
            THROW 50160, 'La pelicula indicada no existe.', 1;

        IF EXISTS (
            SELECT 1
            FROM dbo.funciones
            WHERE id_pelicula = @id_pelicula
        )
            THROW 50161, 'No se puede eliminar la pelicula porque tiene funciones asociadas.', 1;

        DELETE FROM dbo.peliculas
        WHERE id_pelicula = @id_pelicula;

        COMMIT TRANSACTION;

        SELECT
            @id_pelicula AS id_pelicula,
            'Pelicula eliminada correctamente.' AS resultado;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;
GO






CREATE OR ALTER PROCEDURE dbo.SP_LISTAR_PELICULAS
    @busqueda VARCHAR(150) = NULL,
    @id_clasificacion INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.id_pelicula,
        p.titulo,
        p.descripcion,
        p.fecha_lanzamiento,
        p.id_pais,
        p.id_formato_pelicula,
        p.id_clasificacion,
        p.id_distribuidor,
        p.id_director
    FROM dbo.peliculas p
    WHERE
        (@busqueda IS NULL OR p.titulo LIKE '%' + @busqueda + '%')
        AND
        (@id_clasificacion IS NULL OR p.id_clasificacion = @id_clasificacion)
    ORDER BY p.titulo;
END;
GO



CREATE OR ALTER PROCEDURE dbo.SP_CREAR_PRODUCTO_SHOP
    @nombre VARCHAR(100),
    @precio DECIMAL(10,2),
    @descripcion VARCHAR(500) = NULL,
    @stock INT,
    @id_marca INT,
    @id_tipo_producto INT,
    @id_producto_shop INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @nombre IS NULL OR LEN(LTRIM(RTRIM(@nombre))) = 0
        THROW 50170, 'El nombre del producto es obligatorio.', 1;

    IF @precio < 0
        THROW 50171, 'El precio no puede ser negativo.', 1;

    IF @stock < 0
        THROW 50172, 'El stock no puede ser negativo.', 1;

    IF NOT EXISTS (
        SELECT 1 FROM dbo.marcas WHERE id_marca = @id_marca
    )
        THROW 50173, 'La marca indicada no existe.', 1;

    IF NOT EXISTS (
        SELECT 1 FROM dbo.tipos_productos WHERE id_tipo_producto = @id_tipo_producto
    )
        THROW 50174, 'El tipo de producto indicado no existe.', 1;

    INSERT INTO dbo.productos_shop (
        nombre,
        precio,
        descripcion,
        stock,
        id_marca,
        id_tipo_producto
    )
    VALUES (
        LTRIM(RTRIM(@nombre)),
        @precio,
        @descripcion,
        @stock,
        @id_marca,
        @id_tipo_producto
    );

    SET @id_producto_shop = CONVERT(INT, SCOPE_IDENTITY());

    SELECT
        @id_producto_shop AS id_producto_shop,
        'Producto creado correctamente.' AS resultado;
END;
GO


CREATE OR ALTER PROCEDURE dbo.SP_MODIFICAR_PRODUCTO_SHOP
    @id_producto_shop INT,
    @nombre VARCHAR(100),
    @precio DECIMAL(10,2),
    @descripcion VARCHAR(500) = NULL,
    @stock INT,
    @id_marca INT,
    @id_tipo_producto INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.productos_shop
        WHERE id_producto_shop = @id_producto_shop
    )
        THROW 50180, 'El producto indicado no existe.', 1;

    IF @nombre IS NULL OR LEN(LTRIM(RTRIM(@nombre))) = 0
        THROW 50181, 'El nombre del producto es obligatorio.', 1;

    IF @precio < 0
        THROW 50182, 'El precio no puede ser negativo.', 1;

    IF @stock < 0
        THROW 50183, 'El stock no puede ser negativo.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.marcas WHERE id_marca = @id_marca)
        THROW 50184, 'La marca indicada no existe.', 1;

    IF NOT EXISTS (
        SELECT 1 FROM dbo.tipos_productos
        WHERE id_tipo_producto = @id_tipo_producto
    )
        THROW 50185, 'El tipo de producto indicado no existe.', 1;

    UPDATE dbo.productos_shop
    SET
        nombre = LTRIM(RTRIM(@nombre)),
        precio = @precio,
        descripcion = @descripcion,
        stock = @stock,
        id_marca = @id_marca,
        id_tipo_producto = @id_tipo_producto
    WHERE id_producto_shop = @id_producto_shop;

    SELECT
        @id_producto_shop AS id_producto_shop,
        'Producto modificado correctamente.' AS resultado;
END;
GO



CREATE OR ALTER PROCEDURE dbo.SP_ELIMINAR_PRODUCTO_SHOP
    @id_producto_shop INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (
            SELECT 1
            FROM dbo.productos_shop WITH (UPDLOCK, HOLDLOCK)
            WHERE id_producto_shop = @id_producto_shop
        )
            THROW 50190, 'El producto indicado no existe.', 1;

        IF EXISTS (
            SELECT 1
            FROM dbo.detalle_facturas
            WHERE id_producto_shop = @id_producto_shop
        )
            THROW 50191, 'No se puede eliminar: el producto tiene registros de venta.', 1;

        IF EXISTS (
            SELECT 1
            FROM dbo.productos_combos
            WHERE id_producto_shop = @id_producto_shop
        )
            THROW 50192, 'No se puede eliminar: el producto forma parte de uno o mas combos.', 1;

        DELETE FROM dbo.productos_shop
        WHERE id_producto_shop = @id_producto_shop;

        COMMIT TRANSACTION;

        SELECT
            @id_producto_shop AS id_producto_shop,
            'Producto eliminado correctamente.' AS resultado;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;
GO



CREATE OR ALTER PROCEDURE dbo.SP_LISTAR_PRODUCTOS_SHOP
    @busqueda VARCHAR(100) = NULL,
    @id_tipo_producto INT = NULL,
    @incluir_sin_stock BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.id_producto_shop,
        p.nombre,
        p.descripcion,
        p.precio,
        p.stock,
        p.id_marca,
        m.marca,
        p.id_tipo_producto,
        tp.tipo_producto
    FROM dbo.productos_shop p
    INNER JOIN dbo.marcas m
        ON m.id_marca = p.id_marca
    INNER JOIN dbo.tipos_productos tp
        ON tp.id_tipo_producto = p.id_tipo_producto
    WHERE
        (@busqueda IS NULL OR p.nombre LIKE '%' + @busqueda + '%')
        AND (@id_tipo_producto IS NULL OR p.id_tipo_producto = @id_tipo_producto)
        AND (@incluir_sin_stock = 1 OR p.stock > 0)
    ORDER BY p.nombre;
END;
GO


CREATE OR ALTER PROCEDURE dbo.SP_CREAR_COMBO
    @descripcion VARCHAR(200),
    @precio DECIMAL(10,2),
    @id_combo INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @descripcion IS NULL
       OR LEN(LTRIM(RTRIM(@descripcion))) = 0
        THROW 50200, 'La descripcion del combo es obligatoria.', 1;

    IF @precio < 0
        THROW 50201, 'El precio del combo no puede ser negativo.', 1;

    INSERT INTO dbo.combos (
        descripcion,
        precio
    )
    VALUES (
        LTRIM(RTRIM(@descripcion)),
        @precio
    );

    SET @id_combo = CONVERT(INT, SCOPE_IDENTITY());

    SELECT
        @id_combo AS id_combo,
        'Combo creado correctamente.' AS resultado;
END;
GO


CREATE OR ALTER PROCEDURE dbo.SP_MODIFICAR_COMBO
    @id_combo INT,
    @descripcion VARCHAR(200),
    @precio DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.combos
        WHERE id_combo = @id_combo
    )
        THROW 50210, 'El combo indicado no existe.', 1;

    IF @descripcion IS NULL
       OR LEN(LTRIM(RTRIM(@descripcion))) = 0
        THROW 50211, 'La descripcion del combo es obligatoria.', 1;

    IF @precio < 0
        THROW 50212, 'El precio del combo no puede ser negativo.', 1;

    UPDATE dbo.combos
    SET
        descripcion = LTRIM(RTRIM(@descripcion)),
        precio = @precio
    WHERE id_combo = @id_combo;

    SELECT
        @id_combo AS id_combo,
        'Combo modificado correctamente.' AS resultado;
END;
GO



CREATE OR ALTER PROCEDURE dbo.SP_ELIMINAR_COMBO
    @id_combo INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (
            SELECT 1
            FROM dbo.combos WITH (UPDLOCK, HOLDLOCK)
            WHERE id_combo = @id_combo
        )
            THROW 50220, 'El combo indicado no existe.', 1;

        IF EXISTS (
            SELECT 1
            FROM dbo.detalle_facturas
            WHERE id_combo = @id_combo
        )
            THROW 50221, 'No se puede eliminar: el combo tiene registros de venta.', 1;

        DELETE FROM dbo.productos_combos
        WHERE id_combo = @id_combo;

        DELETE FROM dbo.combos
        WHERE id_combo = @id_combo;

        COMMIT TRANSACTION;

        SELECT
            @id_combo AS id_combo,
            'Combo eliminado correctamente.' AS resultado;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;
GO


CREATE OR ALTER PROCEDURE dbo.SP_AGREGAR_PRODUCTO_COMBO
    @id_combo INT,
    @id_producto_shop INT,
    @cantidad INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS (
        SELECT 1 FROM dbo.combos
        WHERE id_combo = @id_combo
    )
        THROW 50230, 'El combo indicado no existe.', 1;

    IF NOT EXISTS (
        SELECT 1 FROM dbo.productos_shop
        WHERE id_producto_shop = @id_producto_shop
    )
        THROW 50231, 'El producto indicado no existe.', 1;

    IF @cantidad <= 0
        THROW 50232, 'La cantidad debe ser mayor a cero.', 1;

    IF EXISTS (
        SELECT 1
        FROM dbo.productos_combos
        WHERE id_combo = @id_combo
          AND id_producto_shop = @id_producto_shop
    )
        THROW 50233, 'El producto ya forma parte del combo.', 1;

    INSERT INTO dbo.productos_combos (
        id_producto_shop,
        id_combo,
        precio_unitario,
        cantidad
    )
    SELECT
        id_producto_shop,
        @id_combo,
        precio,
        @cantidad
    FROM dbo.productos_shop
    WHERE id_producto_shop = @id_producto_shop;

    SELECT
        @id_combo AS id_combo,
        @id_producto_shop AS id_producto_shop,
        'Producto agregado al combo correctamente.' AS resultado;
END;
GO






CREATE OR ALTER PROCEDURE dbo.SP_LISTAR_FUNCIONES_DISPONIBLES
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        f.id_funcion,
        f.fecha,
        h.horario,
        p.id_pelicula,
        p.titulo AS pelicula,
        s.id_sala,
        s.nro_sala,
        su.id_sucursal,
        su.nombre AS sucursal,
        ts.tipo_sala,
        COUNT(b.id_butaca) AS total_butacas,
        SUM(d.disponible) AS butacas_disponibles

    FROM dbo.funciones f

    INNER JOIN dbo.horarios h
        ON h.id_horario = f.id_horario

    INNER JOIN dbo.peliculas p
        ON p.id_pelicula = f.id_pelicula

    INNER JOIN dbo.salas s
        ON s.id_sala = f.id_sala

    INNER JOIN dbo.sucursales su
        ON su.id_sucursal = s.id_sucursal

    INNER JOIN dbo.tipos_salas ts
        ON ts.id_tipo_sala = s.id_tipo_sala

    INNER JOIN dbo.butacas b
        ON b.id_sala = s.id_sala

    CROSS APPLY (
        SELECT
            CASE
                WHEN EXISTS (
                    SELECT 1
                    FROM dbo.entradas e
                    WHERE e.id_funcion = f.id_funcion
                      AND e.id_butaca = b.id_butaca
                )
                OR EXISTS (
                    SELECT 1
                    FROM dbo.reserva_detalle rd
                    INNER JOIN dbo.reservas r
                        ON r.id_reserva = rd.id_reserva
                    WHERE rd.id_butaca = b.id_butaca
                      AND r.id_funcion = f.id_funcion
                      AND r.id_estado_reserva IN (1, 2)
                )
                THEN 0
                ELSE 1
            END AS disponible
    ) d

    WHERE f.fecha >= CONVERT(DATE, GETDATE())

    GROUP BY
        f.id_funcion,
        f.fecha,
        h.horario,
        p.id_pelicula,
        p.titulo,
        s.id_sala,
        s.nro_sala,
        su.id_sucursal,
        su.nombre,
        ts.tipo_sala

    ORDER BY
        f.fecha,
        h.horario,
        p.titulo;
END;
GO



