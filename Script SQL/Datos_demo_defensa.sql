/*
    DATOS PARA LA DEFENSA: butacas realistas, clientes de demostración y reservas

    1) BUTACAS: lleva cada sala a una capacidad más parecida a la real (filas de 10 butacas):
         - Tradicional: 80 butacas (filas A-H). Preferenciales: filas E y F (centro de la sala).
         - 3D:          60 butacas (filas A-F). Preferenciales: filas D y E.
         - Premium:     40 butacas (filas A-D). Todas preferenciales.
       Las butacas que ya existen se conservan (tienen entradas vendidas); solo se ajusta su tipo
       para que el mapa quede coherente. En las salas tradicionales dos butacas nuevas quedan
       "Fuera de servicio", como pasa en un cine real.

    2) CLIENTES: 24 clientes de demostración con usuario (rol Cliente) para poder iniciar sesión.
         Usuario: cliente01 ... cliente24        Correo: ver tabla (dominio @demo.cine)
         Contraseña de todos: Cliente123        (guardada con BCrypt, igual que la API)

    3) RESERVAS: entre 1 y 3 reservas por cliente en funciones de los próximos días, con 1 a 4
       butacas cada una, en distintos estados (Confirmada, Pendiente y algunas Canceladas).
       Respeta los triggers: la butaca es de la sala de la función, no tiene entrada vendida
       ni otra reserva activa.

    ORDEN DE EJECUCIÓN (en la carpeta Script SQL):
        Script_CINER.sql -> Script_estado_pelicula.sql -> Datos_peliculas.sql
        -> Script_portada_trailer.sql -> Datos_portadas_trailers.sql
        -> Script_indicadores_dashboard.sql -> Script_usuario_admin.sql
        -> Datos_ventas.sql -> Datos_demo_defensa.sql  (este, SIEMPRE el último)

    Va después de Datos_ventas.sql porque ese script vende las primeras butacas libres sin mirar
    reservas, y la base no deja vender una butaca reservada. Si no hay funciones futuras
    (por ejemplo, porque no se ejecutó Datos_ventas.sql), este script las crea.

    Se puede ejecutar más de una vez: las butacas solo se agregan si faltan y los clientes y
    reservas solo se cargan si no existe el usuario cliente01.
*/
USE CINE_R;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

-- ============================================================================
-- 1) BUTACAS
-- ============================================================================
BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @comun INT        = (SELECT id_tipo_butacas FROM dbo.tipos_butacas WHERE tipo_butaca = 'Comun');
    DECLARE @preferencial INT = (SELECT id_tipo_butacas FROM dbo.tipos_butacas WHERE tipo_butaca = 'Preferencial');
    DECLARE @disponible INT   = (SELECT id_estado_butaca FROM dbo.estados_butacas WHERE estado_butaca = 'Disponible');
    DECLARE @fuera INT        = (SELECT id_estado_butaca FROM dbo.estados_butacas WHERE estado_butaca = 'Fuera de servicio');

    IF @comun IS NULL OR @preferencial IS NULL OR @disponible IS NULL OR @fuera IS NULL
        THROW 51000, 'Faltan tipos o estados de butaca: ¿se ejecutó Script_CINER.sql?', 1;

    -- Capacidad y filas preferenciales según el tipo de sala
    DECLARE @layout TABLE (tipo_sala VARCHAR(50) PRIMARY KEY, capacidad INT, pref_desde INT, pref_hasta INT);
    INSERT INTO @layout VALUES
        ('Tradicional', 80, 41, 60),   -- filas E y F
        ('3D',          60, 31, 50),   -- filas D y E
        ('Premium',     40,  1, 40);   -- toda la sala

    -- Números del 1 al 80
    DECLARE @numeros TABLE (n INT PRIMARY KEY);
    INSERT INTO @numeros (n)
    SELECT TOP (80) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) FROM sys.all_objects;

    -- a) Agrega las butacas que faltan
    INSERT INTO dbo.butacas (id_sala, nro_butaca, id_tipo_butacas, id_estado_butaca)
    SELECT S.id_sala,
           N.n,
           CASE WHEN N.n BETWEEN L.pref_desde AND L.pref_hasta THEN @preferencial ELSE @comun END,
           -- Dos butacas rotas en las salas tradicionales (puntas de la fila C y de la fila H)
           CASE WHEN TS.tipo_sala = 'Tradicional' AND N.n IN (30, 71) THEN @fuera ELSE @disponible END
    FROM dbo.salas S
    INNER JOIN dbo.tipos_salas TS ON TS.id_tipo_sala = S.id_tipo_sala
    INNER JOIN @layout L          ON L.tipo_sala = TS.tipo_sala
    INNER JOIN @numeros N         ON N.n <= L.capacidad
    WHERE NOT EXISTS (SELECT 1 FROM dbo.butacas B WHERE B.id_sala = S.id_sala AND B.nro_butaca = N.n);

    PRINT CONCAT('Butacas agregadas: ', @@ROWCOUNT);

    -- b) Ajusta el tipo de las butacas que ya existían, para que el mapa sea coherente
    UPDATE B
    SET B.id_tipo_butacas = CASE WHEN B.nro_butaca BETWEEN L.pref_desde AND L.pref_hasta THEN @preferencial ELSE @comun END
    FROM dbo.butacas B
    INNER JOIN dbo.salas S        ON S.id_sala = B.id_sala
    INNER JOIN dbo.tipos_salas TS ON TS.id_tipo_sala = S.id_tipo_sala
    INNER JOIN @layout L          ON L.tipo_sala = TS.tipo_sala
    WHERE B.id_tipo_butacas <> CASE WHEN B.nro_butaca BETWEEN L.pref_desde AND L.pref_hasta THEN @preferencial ELSE @comun END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

-- ============================================================================
-- 2) CLIENTES Y 3) RESERVAS
-- ============================================================================
SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    IF EXISTS (SELECT 1 FROM dbo.usuarios WHERE nombre_usuario = 'cliente01')
    BEGIN
        PRINT 'Los clientes y reservas de demostración ya estaban cargados. No se hizo nada.';
        COMMIT TRANSACTION;
        RETURN;
    END;

    -- Hash BCrypt de "Cliente123" (lo genera la misma librería que usa la API)
    DECLARE @hash VARCHAR(255) = '$2a$11$JtVdSMdIGl8JKO5rT/RBseDfErnfdZ8uWb/IL4gSfgCaibYM12crW';
    DECLARE @rol_cliente INT   = (SELECT id_rol FROM dbo.roles WHERE descripcion = 'Cliente');
    DECLARE @dni INT           = (SELECT id_tipo_doc FROM dbo.tipos_documentos WHERE tipo = 'DNI');

    DECLARE @clientes TABLE
    (
        nro INT PRIMARY KEY, usuario VARCHAR(50), correo VARCHAR(254),
        nombre VARCHAR(100), apellido VARCHAR(100), nro_doc VARCHAR(20), fecha_nac DATE, dias_registro INT
    );

    INSERT INTO @clientes VALUES
    ( 1, 'cliente01', 'martina.alvarez@demo.cine',   'Martina',   'Álvarez',   '40100001', '1998-03-14', 180),
    ( 2, 'cliente02', 'tomas.benitez@demo.cine',     'Tomás',     'Benítez',   '40100002', '1995-07-22', 175),
    ( 3, 'cliente03', 'lucia.castro@demo.cine',      'Lucía',     'Castro',    '40100003', '2001-11-05', 160),
    ( 4, 'cliente04', 'nicolas.diaz@demo.cine',      'Nicolás',   'Díaz',      '40100004', '1989-01-30', 150),
    ( 5, 'cliente05', 'agustina.flores@demo.cine',   'Agustina',  'Flores',    '40100005', '2003-06-18', 140),
    ( 6, 'cliente06', 'franco.gimenez@demo.cine',    'Franco',    'Giménez',   '40100006', '1992-09-09', 132),
    ( 7, 'cliente07', 'micaela.herrera@demo.cine',   'Micaela',   'Herrera',   '40100007', '1999-12-01', 120),
    ( 8, 'cliente08', 'santiago.ibarra@demo.cine',   'Santiago',  'Ibarra',    '40100008', '1987-04-27', 115),
    ( 9, 'cliente09', 'julieta.juarez@demo.cine',    'Julieta',   'Juárez',    '40100009', '2000-08-15', 101),
    (10, 'cliente10', 'facundo.luna@demo.cine',      'Facundo',   'Luna',      '40100010', '1994-02-11',  95),
    (11, 'cliente11', 'florencia.medina@demo.cine',  'Florencia', 'Medina',    '40100011', '1997-10-03',  88),
    (12, 'cliente12', 'ignacio.molina@demo.cine',    'Ignacio',   'Molina',    '40100012', '1985-05-20',  80),
    (13, 'cliente13', 'paula.navarro@demo.cine',     'Paula',     'Navarro',   '40100013', '2002-01-08',  72),
    (14, 'cliente14', 'bruno.ortiz@demo.cine',       'Bruno',     'Ortiz',     '40100014', '1991-07-29',  66),
    (15, 'cliente15', 'carolina.paz@demo.cine',      'Carolina',  'Paz',       '40100015', '1996-03-23',  60),
    (16, 'cliente16', 'emiliano.quiroga@demo.cine',  'Emiliano',  'Quiroga',   '40100016', '1993-11-12',  52),
    (17, 'cliente17', 'rocio.ramirez@demo.cine',     'Rocío',     'Ramírez',   '40100017', '2004-04-04',  45),
    (18, 'cliente18', 'gonzalo.sosa@demo.cine',      'Gonzalo',   'Sosa',      '40100018', '1988-08-08',  38),
    (19, 'cliente19', 'abril.suarez@demo.cine',      'Abril',     'Suárez',    '40100019', '2005-02-17',  30),
    (20, 'cliente20', 'matias.torres@demo.cine',     'Matías',    'Torres',    '40100020', '1990-06-06',  24),
    (21, 'cliente21', 'milagros.vera@demo.cine',     'Milagros',  'Vera',      '40100021', '1999-09-19',  17),
    (22, 'cliente22', 'lautaro.villalba@demo.cine',  'Lautaro',   'Villalba',  '40100022', '2002-12-24',  10),
    (23, 'cliente23', 'josefina.romero@demo.cine',   'Josefina',  'Romero',    '40100023', '1986-10-10',   5),
    (24, 'cliente24', 'thiago.acosta@demo.cine',     'Thiago',    'Acosta',    '40100024', '2001-05-31',   1);

    INSERT INTO dbo.usuarios (nombre_usuario, contraseña, id_rol, correo_electronico)
    SELECT usuario, @hash, @rol_cliente, correo FROM @clientes;

    INSERT INTO dbo.clientes (nombre, apellido, nro_doc, id_tipo_doc, fecha_nacimiento, fecha_registro, id_usuario)
    SELECT C.nombre, C.apellido, C.nro_doc, @dni, C.fecha_nac,
           DATEADD(DAY, -C.dias_registro, CONVERT(DATE, GETDATE())), U.id_usuario
    FROM @clientes C
    INNER JOIN dbo.usuarios U ON U.nombre_usuario = C.usuario;

    PRINT CONCAT('Clientes de demostración creados: ', @@ROWCOUNT);

    -- ------------------------------------------------------------------------
    -- Funciones futuras: si no hay, se crean para los próximos 7 días
    -- ------------------------------------------------------------------------
    DECLARE @hoy DATE = CONVERT(DATE, GETDATE());

    IF NOT EXISTS (SELECT 1 FROM dbo.funciones WHERE fecha > @hoy)
    BEGIN
        DECLARE @cant_peliculas INT = (SELECT COUNT(*) FROM dbo.peliculas WHERE titulo NOT LIKE '%prueba%');

        INSERT INTO dbo.funciones (id_pelicula, id_sala, fecha, id_horario)
        SELECT P.id_pelicula, C.id_sala, C.fecha, C.id_horario
        FROM (
            SELECT S.id_sala, DATEADD(DAY, D.dia, @hoy) AS fecha, H.id_horario,
                   ROW_NUMBER() OVER (ORDER BY D.dia, S.id_sala, H.horario) AS nro_slot
            FROM dbo.salas S
            CROSS JOIN (VALUES (1), (2), (3), (4), (5), (6), (7)) D(dia)
            CROSS JOIN dbo.horarios H
        ) C
        INNER JOIN (
            SELECT id_pelicula, ROW_NUMBER() OVER (ORDER BY id_pelicula) AS nro
            FROM dbo.peliculas WHERE titulo NOT LIKE '%prueba%'
        ) P ON P.nro = (C.nro_slot % @cant_peliculas) + 1
        WHERE NOT EXISTS (SELECT 1 FROM dbo.funciones X
                          WHERE X.id_sala = C.id_sala AND X.fecha = C.fecha AND X.id_horario = C.id_horario);

        PRINT CONCAT('Funciones creadas para las reservas: ', @@ROWCOUNT);
    END;

    -- Funciones disponibles para reservar (desde mañana), numeradas para repartirlas
    CREATE TABLE #Funciones (nro INT PRIMARY KEY, id_funcion INT, id_sala INT, precio_base DECIMAL(10,2));

    INSERT INTO #Funciones (nro, id_funcion, id_sala, precio_base)
    SELECT ROW_NUMBER() OVER (ORDER BY F.fecha, H.horario, F.id_sala), F.id_funcion, F.id_sala, TE.precio
    FROM dbo.funciones F
    INNER JOIN dbo.horarios H         ON H.id_horario = F.id_horario
    INNER JOIN dbo.salas S            ON S.id_sala = F.id_sala
    INNER JOIN dbo.tarifas_entradas TE ON TE.id_tipo_sala = S.id_tipo_sala
    INNER JOIN dbo.peliculas P        ON P.id_pelicula = F.id_pelicula
    INNER JOIN dbo.estados_peliculas EP ON EP.id_estado_pelicula = P.id_estado_pelicula
    WHERE F.fecha > @hoy AND EP.estado = 'Activa';

    DECLARE @total_funciones INT = (SELECT COUNT(*) FROM #Funciones);
    IF @total_funciones = 0
        THROW 51001, 'No hay funciones futuras de películas activas para cargar reservas.', 1;

    DECLARE @pendiente INT  = (SELECT id_estado_reserva FROM dbo.estados_reservas WHERE estado = 'Pendiente');
    DECLARE @confirmada INT = (SELECT id_estado_reserva FROM dbo.estados_reservas WHERE estado = 'Confirmada');
    DECLARE @cancelada INT  = (SELECT id_estado_reserva FROM dbo.estados_reservas WHERE estado = 'Cancelada');
    DECLARE @disponible INT = (SELECT id_estado_butaca FROM dbo.estados_butacas WHERE estado_butaca = 'Disponible');

    -- ------------------------------------------------------------------------
    -- Reservas: cliente por cliente
    -- ------------------------------------------------------------------------
    DECLARE @butacas TABLE (id_butaca INT PRIMARY KEY, precio DECIMAL(10,2));
    DECLARE @nro INT, @cod_cliente INT, @k INT, @cant_reservas INT, @cant_butacas INT;
    DECLARE @id_funcion INT, @id_sala INT, @precio_base DECIMAL(10,2), @estado INT, @monto DECIMAL(10,2);
    DECLARE @id_reserva INT, @reservas_creadas INT = 0;

    DECLARE cur_clientes CURSOR LOCAL FAST_FORWARD FOR
        SELECT C.nro, CL.cod_cliente
        FROM @clientes C
        INNER JOIN dbo.usuarios U  ON U.nombre_usuario = C.usuario
        INNER JOIN dbo.clientes CL ON CL.id_usuario = U.id_usuario
        ORDER BY C.nro;

    OPEN cur_clientes;
    FETCH NEXT FROM cur_clientes INTO @nro, @cod_cliente;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @cant_reservas = 1 + (@nro % 3);          -- 1, 2 o 3 reservas por cliente
        SET @k = 1;

        WHILE @k <= @cant_reservas
        BEGIN
            -- Función elegida "al azar" pero siempre igual (el script da el mismo resultado cada vez)
            SELECT @id_funcion = id_funcion, @id_sala = id_sala, @precio_base = precio_base
            FROM #Funciones
            WHERE nro = ((@nro * 7 + @k * 13) % @total_funciones) + 1;

            SET @cant_butacas = 1 + ((@nro + @k) % 4);   -- 1 a 4 butacas

            -- Butacas libres, empezando por el centro de la sala y salteando las primeras 10
            -- (Datos_ventas.sql vende desde la butaca 1 hacia arriba)
            DELETE FROM @butacas;
            INSERT INTO @butacas (id_butaca, precio)
            SELECT TOP (@cant_butacas) B.id_butaca, @precio_base + AB.adicional
            FROM dbo.butacas B
            INNER JOIN dbo.adicionales_butacas AB ON AB.id_tipo_butacas = B.id_tipo_butacas
            WHERE B.id_sala = @id_sala
              AND B.nro_butaca > 10
              AND B.id_estado_butaca = @disponible
              AND NOT EXISTS (SELECT 1 FROM dbo.entradas E
                              WHERE E.id_funcion = @id_funcion AND E.id_butaca = B.id_butaca)
              AND NOT EXISTS (SELECT 1
                              FROM dbo.reserva_detalle RD
                              INNER JOIN dbo.reservas R ON R.id_reserva = RD.id_reserva
                              WHERE R.id_funcion = @id_funcion AND RD.id_butaca = B.id_butaca
                                AND R.id_estado_reserva IN (@pendiente, @confirmada))
            ORDER BY ABS(B.nro_butaca - 45) + ((B.nro_butaca * 7 + @nro) % 9);

            IF (SELECT COUNT(*) FROM @butacas) = @cant_butacas
            BEGIN
                -- 60% confirmadas, 30% pendientes, 10% canceladas
                SET @estado = CASE WHEN (@nro + @k) % 10 < 6 THEN @confirmada
                                   WHEN (@nro + @k) % 10 < 9 THEN @pendiente
                                   ELSE @cancelada END;
                SET @monto = (SELECT SUM(precio) FROM @butacas);

                INSERT INTO dbo.reservas (fecha, id_funcion, cod_cliente, id_estado_reserva, monto_reserva)
                VALUES (DATEADD(MINUTE, -((@nro * 97 + @k * 211) % 4320), SYSDATETIME()),   -- últimos 3 días
                        @id_funcion, @cod_cliente, @estado, @monto);

                SET @id_reserva = CONVERT(INT, SCOPE_IDENTITY());

                INSERT INTO dbo.reserva_detalle (id_reserva, id_butaca)
                SELECT @id_reserva, id_butaca FROM @butacas;

                SET @reservas_creadas += 1;
            END;

            SET @k += 1;
        END;

        FETCH NEXT FROM cur_clientes INTO @nro, @cod_cliente;
    END;

    CLOSE cur_clientes;
    DEALLOCATE cur_clientes;

    DROP TABLE #Funciones;

    COMMIT TRANSACTION;
    PRINT CONCAT('Reservas de demostración creadas: ', @reservas_creadas);
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
GO

-- ============================================================================
-- VERIFICACIÓN
-- ============================================================================
SELECT S.id_sala, TS.tipo_sala, COUNT(*) AS butacas,
       SUM(CASE WHEN TB.tipo_butaca = 'Preferencial' THEN 1 ELSE 0 END) AS preferenciales,
       SUM(CASE WHEN EB.estado_butaca = 'Fuera de servicio' THEN 1 ELSE 0 END) AS fuera_de_servicio
FROM dbo.butacas B
INNER JOIN dbo.salas S            ON S.id_sala = B.id_sala
INNER JOIN dbo.tipos_salas TS     ON TS.id_tipo_sala = S.id_tipo_sala
INNER JOIN dbo.tipos_butacas TB   ON TB.id_tipo_butacas = B.id_tipo_butacas
INNER JOIN dbo.estados_butacas EB ON EB.id_estado_butaca = B.id_estado_butaca
GROUP BY S.id_sala, TS.tipo_sala
ORDER BY S.id_sala;

SELECT ER.estado, COUNT(DISTINCT R.id_reserva) AS reservas, COUNT(*) AS butacas_reservadas
FROM dbo.reservas R
INNER JOIN dbo.estados_reservas ER ON ER.id_estado_reserva = R.id_estado_reserva
INNER JOIN dbo.reserva_detalle RD  ON RD.id_reserva = R.id_reserva
GROUP BY ER.estado;

SELECT TOP 5 U.nombre_usuario, U.correo_electronico, C.nombre, C.apellido, COUNT(R.id_reserva) AS reservas
FROM dbo.clientes C
INNER JOIN dbo.usuarios U  ON U.id_usuario = C.id_usuario
LEFT JOIN dbo.reservas R   ON R.cod_cliente = C.cod_cliente
WHERE U.nombre_usuario LIKE 'cliente[0-9][0-9]'
GROUP BY U.nombre_usuario, U.correo_electronico, C.nombre, C.apellido
ORDER BY U.nombre_usuario;
GO
