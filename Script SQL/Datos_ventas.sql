/*
    DATOS DE PRUEBA: ventas (para el dashboard)

    Carga:
    - 6 clientes de prueba (usuarios demo_cliente1 ... demo_cliente6) y 1 cajero por sucursal.
    - Funciones nuevas para los próximos 10 días (desde HOY), en todas las salas y horarios
      libres. Las películas se reparten de forma rotativa.
      (La tabla funciones no admite fechas pasadas: CK_funciones_fecha_no_pasada.)
    - Ventas (facturas, entradas, detalle y pagos) como compras anticipadas: las facturas se
      fechan en los 10 días anteriores a hoy, para que los indicadores tengan historial.
      Imita lo que hace SP_REGISTRAR_VENTA: la factura nace "Pendiente" y termina "Pagada".
    - Unas pocas facturas quedan "Pendiente" y otras "Anulada" a propósito, para comprobar
      que los indicadores cuentan SOLO las pagadas.

    Cómo están armados los números (para que el dashboard no salga todo parejo):
    - Cada película tiene una "popularidad" base.
    - 19:30 vende más que 22:00 y 22:00 más que 17:00.
    - Sábado y domingo se vende un poco más.
    - Formas de pago: ~35% débito, ~30% crédito, ~25% efectivo, ~10% transferencia.

    Es seguro ejecutarlo más de una vez: si ya existe el usuario demo_cliente1, no hace nada.
    Ejecutar DESPUÉS de Script_CINER.sql (y, si querés ver películas reales, de Datos_peliculas.sql).
*/
USE CINE_R;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    -- 0) ¿Ya se cargó antes?
    IF EXISTS (SELECT 1 FROM dbo.usuarios WHERE nombre_usuario = 'demo_cliente1')
    BEGIN
        PRINT 'Los datos de ventas ya estaban cargados. No se hizo nada.';
        COMMIT TRANSACTION;
        RETURN;
    END;

    -- 1) CLIENTES DE PRUEBA (cada cliente necesita un usuario con rol Cliente)
    INSERT INTO dbo.usuarios (nombre_usuario, contraseña, id_rol, correo_electronico)
    SELECT V.nombre_usuario, 'TEMPORAL_CAMBIAR_EN_API', R.id_rol, V.correo
    FROM (VALUES
        ('demo_cliente1', 'demo.cliente1@cine.local'),
        ('demo_cliente2', 'demo.cliente2@cine.local'),
        ('demo_cliente3', 'demo.cliente3@cine.local'),
        ('demo_cliente4', 'demo.cliente4@cine.local'),
        ('demo_cliente5', 'demo.cliente5@cine.local'),
        ('demo_cliente6', 'demo.cliente6@cine.local')
    ) V(nombre_usuario, correo)
    CROSS JOIN dbo.roles R
    WHERE R.descripcion = 'Cliente';

    INSERT INTO dbo.clientes (nombre, apellido, nro_doc, id_tipo_doc, id_usuario)
    SELECT V.nombre, V.apellido, V.nro_doc, TD.id_tipo_doc, U.id_usuario
    FROM (VALUES
        ('Sofía',   'Martínez', '30111001', 'demo_cliente1'),
        ('Mateo',   'Gómez',    '30111002', 'demo_cliente2'),
        ('Valentina','Pérez',   '30111003', 'demo_cliente3'),
        ('Lucas',   'Fernández','30111004', 'demo_cliente4'),
        ('Camila',  'Rodríguez','30111005', 'demo_cliente5'),
        ('Joaquín', 'López',    '30111006', 'demo_cliente6')
    ) V(nombre, apellido, nro_doc, nombre_usuario)
    INNER JOIN dbo.usuarios U ON U.nombre_usuario = V.nombre_usuario
    CROSS JOIN dbo.tipos_documentos TD
    WHERE TD.tipo = 'DNI';

    -- 2) UN CAJERO POR SUCURSAL (la factura exige que el cajero sea de la misma sucursal)
    INSERT INTO dbo.cajeros (nombre, apellido, id_turno, id_sucursal)
    SELECT V.nombre, V.apellido, T.id_turno, S.id_sucursal
    FROM (VALUES
        ('Cajero', 'Demo Central',     'Cine Central Córdoba'),
        ('Cajero', 'Demo Carlos Paz',  'Cine Carlos Paz')
    ) V(nombre, apellido, sucursal)
    INNER JOIN dbo.sucursales S ON S.nombre = V.sucursal
    CROSS JOIN dbo.turnos T
    WHERE T.turno = 'Tarde';

    -- 3) FUNCIONES: cada combinación sala + día + horario libre recibe una película.
    --    Los días van de hoy a hoy + 9 (la tabla no acepta fechas pasadas).
    --    (Se ignoran las películas de prueba de la baja lógica, que tienen "prueba" en el título.)
    DECLARE @hoy DATE = CONVERT(DATE, GETDATE());
    DECLARE @cant_peliculas INT =
        (SELECT COUNT(*) FROM dbo.peliculas WHERE titulo NOT LIKE '%prueba%');

    INSERT INTO dbo.funciones (id_pelicula, id_sala, fecha, id_horario)
    SELECT P.id_pelicula, C.id_sala, C.fecha, C.id_horario
    FROM (
        SELECT S.id_sala,
               DATEADD(DAY, D.dia, @hoy) AS fecha,
               H.id_horario,
               ROW_NUMBER() OVER (ORDER BY D.dia, S.id_sala, H.horario) AS nro_slot
        FROM dbo.salas S
        CROSS JOIN (VALUES (0), (1), (2), (3), (4), (5), (6), (7), (8), (9)) D(dia)
        CROSS JOIN dbo.horarios H
    ) C
    INNER JOIN (
        SELECT id_pelicula, ROW_NUMBER() OVER (ORDER BY id_pelicula) AS nro
        FROM dbo.peliculas
        WHERE titulo NOT LIKE '%prueba%'
    ) P ON P.nro = (C.nro_slot % @cant_peliculas) + 1
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.funciones X
        WHERE X.id_sala = C.id_sala
          AND X.fecha = C.fecha
          AND X.id_horario = C.id_horario
    );

    -- 4) PLAN DE VENTAS: cuántas entradas se venden en cada función de hoy en adelante (máximo 8 = sala llena)
    CREATE TABLE #Plan
    (
        id_funcion INT PRIMARY KEY,
        id_sala INT NOT NULL,
        id_sucursal INT NOT NULL,
        fecha DATE NOT NULL,
        horario TIME NOT NULL,
        entradas_a_vender INT NOT NULL
    );

    INSERT INTO #Plan (id_funcion, id_sala, id_sucursal, fecha, horario, entradas_a_vender)
    SELECT
        F.id_funcion,
        F.id_sala,
        S.id_sucursal,
        F.fecha,
        H.horario,
        CASE WHEN Z.cantidad < 0 THEN 0
             WHEN Z.cantidad > 8 THEN 8
             ELSE Z.cantidad END
    FROM dbo.funciones F
    INNER JOIN dbo.salas S     ON S.id_sala = F.id_sala
    INNER JOIN dbo.horarios H  ON H.id_horario = F.id_horario
    INNER JOIN dbo.peliculas P ON P.id_pelicula = F.id_pelicula
    LEFT JOIN (VALUES
        ('Avatar', 5), ('Barbie', 5), ('Oppenheimer', 5),
        ('Duna: Parte dos', 4), ('Batman: El caballero de la noche', 4),
        ('Titanic', 4), ('Jurassic Park', 4),
        ('Toy Story', 3), ('El origen', 3), ('Matrix', 3),
        ('El secreto de sus ojos', 3), ('Relatos salvajes', 3), ('Rumbo a las estrellas', 3),
        ('El padrino', 2), ('Pulp Fiction', 2),
        ('El último viaje', 2), ('Código perdido', 2), ('La ciudad sumergida', 2),
        ('Forrest Gump', 1)
    ) W(titulo, popularidad) ON W.titulo = P.titulo
    CROSS APPLY (
        SELECT
              COALESCE(W.popularidad, 2)                                          -- película
            + CASE WHEN H.horario < '19:00' THEN -1                               -- horario
                   WHEN H.horario < '21:00' THEN 2
                   ELSE 1 END
            + CASE WHEN DATEDIFF(DAY, '2026-01-03', F.fecha) % 7 IN (0, 1)        -- sábado o domingo
                   THEN 1 ELSE 0 END
            + (F.id_funcion % 3) - 1                                              -- pequeña variación
            AS cantidad
    ) Z
    WHERE F.fecha >= @hoy;

    -- 5) DATOS DE REFERENCIA (ids que se usan en cada venta)
    DECLARE @id_iva INT                 = (SELECT id_condicion_IVA FROM dbo.condiciones_IVA WHERE condicion = 'Consumidor Final');
    DECLARE @canal_presencial INT       = (SELECT id_canal_venta FROM dbo.canales_ventas WHERE canal_venta = 'Presencial');
    DECLARE @canal_web INT              = (SELECT id_canal_venta FROM dbo.canales_ventas WHERE canal_venta = 'Web');
    DECLARE @estado_pendiente INT       = (SELECT id_estado_compra FROM dbo.estados_compras WHERE estado = 'Pendiente');
    DECLARE @estado_pagada INT          = (SELECT id_estado_compra FROM dbo.estados_compras WHERE estado = 'Pagada');
    DECLARE @estado_anulada INT         = (SELECT id_estado_compra FROM dbo.estados_compras WHERE estado = 'Anulada');
    DECLARE @fp_efectivo INT            = (SELECT id_forma_pago FROM dbo.formas_pagos WHERE forma_pago = 'Efectivo');
    DECLARE @fp_debito INT              = (SELECT id_forma_pago FROM dbo.formas_pagos WHERE forma_pago = 'Tarjeta de debito');
    DECLARE @fp_credito INT             = (SELECT id_forma_pago FROM dbo.formas_pagos WHERE forma_pago = 'Tarjeta de credito');
    DECLARE @fp_transferencia INT       = (SELECT id_forma_pago FROM dbo.formas_pagos WHERE forma_pago = 'Transferencia');
    DECLARE @fin_nacion INT             = (SELECT id_financiera FROM dbo.financieras WHERE financiera = 'Banco Nacion');
    DECLARE @fin_galicia INT            = (SELECT id_financiera FROM dbo.financieras WHERE financiera = 'Banco Galicia');

    -- Los 6 clientes demo, numerados del 1 al 6 para poder rotarlos
    CREATE TABLE #Clientes (nro INT PRIMARY KEY, cod_cliente INT NOT NULL);

    INSERT INTO #Clientes (nro, cod_cliente)
    SELECT ROW_NUMBER() OVER (ORDER BY U.nombre_usuario), C.cod_cliente
    FROM dbo.clientes C
    INNER JOIN dbo.usuarios U ON U.id_usuario = C.id_usuario
    WHERE U.nombre_usuario LIKE 'demo_cliente%';

    -- 6) VENTAS: función por función, factura por factura
    DECLARE @ent TABLE (id_entrada INT, precio DECIMAL(10,2));

    DECLARE @id_funcion INT, @id_sala INT, @id_sucursal INT, @fecha DATE, @horario TIME, @pendientes INT;
    DECLARE @n INT = 0;                       -- contador de facturas (reparte clientes, pagos, canales)
    DECLARE @tam INT, @cod_cliente INT, @id_cajero INT, @id_canal INT, @fecha_venta DATETIME2;
    DECLARE @estado_final INT, @forma_pago INT, @financiera INT, @nro_factura INT, @total DECIMAL(10,2);

    DECLARE cur_funciones CURSOR LOCAL FAST_FORWARD FOR
        SELECT id_funcion, id_sala, id_sucursal, fecha, horario, entradas_a_vender
        FROM #Plan
        WHERE entradas_a_vender > 0
        ORDER BY fecha, horario, id_funcion;

    OPEN cur_funciones;
    FETCH NEXT FROM cur_funciones INTO @id_funcion, @id_sala, @id_sucursal, @fecha, @horario, @pendientes;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Cada factura compra 1, 2 o 3 entradas, hasta completar las de la función
        WHILE @pendientes > 0
        BEGIN
            SET @n += 1;

            SET @tam = 1 + (@n % 3);
            IF @tam > @pendientes SET @tam = @pendientes;

            SET @cod_cliente = (SELECT cod_cliente FROM #Clientes WHERE nro = (@n % 6) + 1);
            SET @id_cajero   = (SELECT TOP 1 id_cajero FROM dbo.cajeros
                                WHERE id_sucursal = @id_sucursal AND apellido LIKE 'Demo%');
            SET @id_canal    = CASE WHEN @n % 4 = 0 THEN @canal_web ELSE @canal_presencial END;

            -- Compra anticipada: entre 1 y 10 días antes de hoy, entre las 10:00 y las 15:00
            SET @fecha_venta = DATEADD(MINUTE,
                                   600 + (@n % 300),
                                   CAST(DATEADD(DAY, -(1 + (@n % 10)), @hoy) AS DATETIME2));

            -- Estado final: casi todas Pagadas; 1 de cada 30 queda Pendiente y 1 de cada 30 Anulada
            SET @estado_final = CASE WHEN @n % 30 = 7  THEN @estado_pendiente
                                     WHEN @n % 30 = 19 THEN @estado_anulada
                                     ELSE @estado_pagada END;

            -- Forma de pago: 35% débito, 30% crédito, 25% efectivo, 10% transferencia
            SET @forma_pago = CASE WHEN @n % 20 < 7  THEN @fp_debito
                                   WHEN @n % 20 < 13 THEN @fp_credito
                                   WHEN @n % 20 < 18 THEN @fp_efectivo
                                   ELSE @fp_transferencia END;

            -- Las tarjetas exigen financiera; efectivo y transferencia no la llevan
            SET @financiera = CASE WHEN @forma_pago IN (@fp_debito, @fp_credito)
                                   THEN (CASE WHEN @n % 2 = 0 THEN @fin_nacion ELSE @fin_galicia END)
                                   ELSE NULL END;

            -- a) Factura pendiente
            INSERT INTO dbo.facturas
                (id_condicion_IVA, fecha, cod_cliente, id_cajero, id_sucursal, id_estado_compra, id_canal_venta)
            VALUES
                (@id_iva, @fecha_venta, @cod_cliente, @id_cajero, @id_sucursal, @estado_pendiente, @id_canal);

            SET @nro_factura = CONVERT(INT, SCOPE_IDENTITY());

            -- b) Entradas: las primeras butacas libres de la función (precio = tarifa de la sala + adicional de butaca)
            DELETE FROM @ent;

            INSERT INTO dbo.entradas (id_funcion, id_butaca, precio)
            OUTPUT inserted.id_entrada, inserted.precio INTO @ent (id_entrada, precio)
            SELECT TOP (@tam)
                @id_funcion,
                B.id_butaca,
                TE.precio + AB.adicional
            FROM dbo.butacas B
            INNER JOIN dbo.salas S               ON S.id_sala = B.id_sala
            INNER JOIN dbo.tarifas_entradas TE   ON TE.id_tipo_sala = S.id_tipo_sala
            INNER JOIN dbo.adicionales_butacas AB ON AB.id_tipo_butacas = B.id_tipo_butacas
            WHERE B.id_sala = @id_sala
              AND NOT EXISTS (
                  SELECT 1
                  FROM dbo.entradas E
                  WHERE E.id_funcion = @id_funcion
                    AND E.id_butaca = B.id_butaca
              )
            ORDER BY B.nro_butaca;

            -- c) Detalle de factura: una línea por entrada
            INSERT INTO dbo.detalle_facturas
                (nro_factura, id_entrada, cantidad, precio_unitario, descuento_unitario, precio_final_unitario)
            SELECT @nro_factura, id_entrada, 1, precio, 0, precio
            FROM @ent;

            -- d) Pago por el total (solo si la factura va a quedar Pagada)
            IF @estado_final = @estado_pagada
            BEGIN
                SET @total = (SELECT SUM(precio) FROM @ent);

                INSERT INTO dbo.facturas_formas_pagos (nro_factura, id_forma_pago, monto_pagado, id_financiera)
                VALUES (@nro_factura, @forma_pago, @total, @financiera);
            END;

            -- e) Estado final (el trigger solo permite Pendiente -> Pagada o Anulada)
            IF @estado_final <> @estado_pendiente
            BEGIN
                UPDATE dbo.facturas
                SET id_estado_compra = @estado_final
                WHERE nro_factura = @nro_factura;
            END;

            SET @pendientes -= @tam;
        END;

        FETCH NEXT FROM cur_funciones INTO @id_funcion, @id_sala, @id_sucursal, @fecha, @horario, @pendientes;
    END;

    CLOSE cur_funciones;
    DEALLOCATE cur_funciones;

    COMMIT TRANSACTION;

    PRINT 'Ventas de prueba cargadas correctamente.';

    -- 7) VERIFICACIÓN
    SELECT ec.estado, COUNT(*) AS facturas
    FROM dbo.facturas f
    INNER JOIN dbo.estados_compras ec ON ec.id_estado_compra = f.id_estado_compra
    GROUP BY ec.estado;

    SELECT TOP 5 p.titulo, COUNT(*) AS entradas_pagadas
    FROM dbo.detalle_facturas df
    INNER JOIN dbo.facturas f          ON f.nro_factura = df.nro_factura
    INNER JOIN dbo.estados_compras ec  ON ec.id_estado_compra = f.id_estado_compra
    INNER JOIN dbo.entradas e          ON e.id_entrada = df.id_entrada
    INNER JOIN dbo.funciones fu        ON fu.id_funcion = e.id_funcion
    INNER JOIN dbo.peliculas p         ON p.id_pelicula = fu.id_pelicula
    WHERE ec.estado = 'Pagada'
    GROUP BY p.titulo
    ORDER BY entradas_pagadas DESC;

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO
