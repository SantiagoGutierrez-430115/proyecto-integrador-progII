/*
    INDICADORES DEL DASHBOARD

    Crea tres procedimientos almacenados de solo lectura (no modifican datos):
      1) SP_INDICADOR_PELICULAS_MAS_VENDIDAS
      2) SP_INDICADOR_HORARIOS_MAS_POPULARES
      3) SP_INDICADOR_FORMAS_PAGO_MAS_USADAS

    Reglas comunes:
    - Solo cuentan las facturas en estado 'Pagada' (las Pendientes y Anuladas se ignoran).
    - @fecha_desde y @fecha_hasta son opcionales (ambas inclusive). Se filtran por la
      fecha en que se hizo la VENTA (facturas.fecha), no por la fecha de la función.
      Si no se pasan, se cuenta todo el historial.
    - Es seguro ejecutarlo más de una vez (CREATE OR ALTER).

    Las columnas del resultado llevan nombres en PascalCase (IdPelicula, EntradasVendidas, ...)
    para que coincidan con las propiedades de los DTOs de C#.

    Ejecutar en SSMS contra la base CINE_R.
    No depende de Script_estado_pelicula.sql.
*/
USE CINE_R;
GO

-- 1) PELÍCULAS MÁS VENDIDAS
--    Devuelve, por película, cuántas entradas se vendieron y cuánto se recaudó.
CREATE OR ALTER PROCEDURE dbo.SP_INDICADOR_PELICULAS_MAS_VENDIDAS
    @fecha_desde DATE = NULL,
    @fecha_hasta DATE = NULL,
    @top INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    SET @top = ISNULL(@top, 2147483647);

    SELECT TOP (@top)
        p.id_pelicula AS IdPelicula,
        p.titulo AS Titulo,
        SUM(df.cantidad) AS EntradasVendidas,
        SUM(df.cantidad * df.precio_final_unitario) AS Recaudacion
    FROM dbo.detalle_facturas df
    INNER JOIN dbo.facturas fa         ON fa.nro_factura = df.nro_factura
    INNER JOIN dbo.estados_compras ec  ON ec.id_estado_compra = fa.id_estado_compra
    INNER JOIN dbo.entradas e          ON e.id_entrada = df.id_entrada
    INNER JOIN dbo.funciones fu        ON fu.id_funcion = e.id_funcion
    INNER JOIN dbo.peliculas p         ON p.id_pelicula = fu.id_pelicula
    WHERE ec.estado = 'Pagada'
      AND (@fecha_desde IS NULL OR fa.fecha >= @fecha_desde)
      AND (@fecha_hasta IS NULL OR fa.fecha <  DATEADD(DAY, 1, @fecha_hasta))
    GROUP BY p.id_pelicula, p.titulo
    ORDER BY EntradasVendidas DESC, p.titulo;
END;
GO

-- 2) HORARIOS MÁS POPULARES
--    Devuelve, por horario de función, las entradas vendidas, cuántas funciones tuvieron ventas
--    y la recaudación. El horario sale como texto "HH:mm" para que el front lo muestre directo.
CREATE OR ALTER PROCEDURE dbo.SP_INDICADOR_HORARIOS_MAS_POPULARES
    @fecha_desde DATE = NULL,
    @fecha_hasta DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        h.id_horario AS IdHorario,
        CONVERT(VARCHAR(5), h.horario, 108) AS Horario,
        SUM(df.cantidad) AS EntradasVendidas,
        COUNT(DISTINCT fu.id_funcion) AS FuncionesConVentas,
        SUM(df.cantidad * df.precio_final_unitario) AS Recaudacion
    FROM dbo.detalle_facturas df
    INNER JOIN dbo.facturas fa         ON fa.nro_factura = df.nro_factura
    INNER JOIN dbo.estados_compras ec  ON ec.id_estado_compra = fa.id_estado_compra
    INNER JOIN dbo.entradas e          ON e.id_entrada = df.id_entrada
    INNER JOIN dbo.funciones fu        ON fu.id_funcion = e.id_funcion
    INNER JOIN dbo.horarios h          ON h.id_horario = fu.id_horario
    WHERE ec.estado = 'Pagada'
      AND (@fecha_desde IS NULL OR fa.fecha >= @fecha_desde)
      AND (@fecha_hasta IS NULL OR fa.fecha <  DATEADD(DAY, 1, @fecha_hasta))
    GROUP BY h.id_horario, h.horario
    ORDER BY EntradasVendidas DESC, h.horario;
END;
GO

-- 3) FORMAS DE PAGO MÁS USADAS
--    Devuelve, por forma de pago, cuántos pagos se registraron, el monto total y el porcentaje.
--    (Una factura puede tener más de una forma de pago; cada una cuenta como un pago.)
CREATE OR ALTER PROCEDURE dbo.SP_INDICADOR_FORMAS_PAGO_MAS_USADAS
    @fecha_desde DATE = NULL,
    @fecha_hasta DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        fp.id_forma_pago AS IdFormaPago,
        fp.forma_pago AS FormaPago,
        COUNT(*) AS CantidadPagos,
        SUM(ffp.monto_pagado) AS MontoTotal,
        CAST(100.0 * COUNT(*) / SUM(COUNT(*)) OVER () AS DECIMAL(5,2)) AS Porcentaje
    FROM dbo.facturas_formas_pagos ffp
    INNER JOIN dbo.facturas fa         ON fa.nro_factura = ffp.nro_factura
    INNER JOIN dbo.estados_compras ec  ON ec.id_estado_compra = fa.id_estado_compra
    INNER JOIN dbo.formas_pagos fp     ON fp.id_forma_pago = ffp.id_forma_pago
    WHERE ec.estado = 'Pagada'
      AND (@fecha_desde IS NULL OR fa.fecha >= @fecha_desde)
      AND (@fecha_hasta IS NULL OR fa.fecha <  DATEADD(DAY, 1, @fecha_hasta))
    GROUP BY fp.id_forma_pago, fp.forma_pago
    ORDER BY CantidadPagos DESC, fp.forma_pago;
END;
GO

-- 4) PRUEBA RÁPIDA (descomentar para probar)
-- EXEC dbo.SP_INDICADOR_PELICULAS_MAS_VENDIDAS;
-- EXEC dbo.SP_INDICADOR_PELICULAS_MAS_VENDIDAS @top = 5, @fecha_desde = '2026-11-10', @fecha_hasta = '2026-11-16';
-- EXEC dbo.SP_INDICADOR_HORARIOS_MAS_POPULARES;
-- EXEC dbo.SP_INDICADOR_FORMAS_PAGO_MAS_USADAS;
