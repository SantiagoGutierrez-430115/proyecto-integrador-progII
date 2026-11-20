/*
    INDICADORES DEL DASHBOARD
    Procedimientos que usa DashboardRepository (api/Dashboard):
      - SP_INDICADOR_PELICULAS_MAS_VENDIDAS
      - SP_INDICADOR_HORARIOS_MAS_POPULARES
      - SP_INDICADOR_FORMAS_PAGO_MAS_USADAS

    Reglas:
    - Solo cuentan las facturas en estado 'Pagada' (las Pendientes y Anuladas no son ventas).
    - Las fechas filtran por la fecha de la factura y son opcionales (NULL = sin límite).
      @fecha_hasta incluye todo ese día.
    - Los alias de las columnas coinciden con las propiedades de los DTOs de C#
      (PeliculaMasVendidaDto, HorarioPopularDto, FormaPagoUsadaDto): EF los mapea por nombre.

    Es seguro ejecutarlo más de una vez (CREATE OR ALTER).
*/
USE CINE_R;
GO

CREATE OR ALTER PROCEDURE dbo.SP_INDICADOR_PELICULAS_MAS_VENDIDAS
    @fecha_desde DATETIME2 = NULL,
    @fecha_hasta DATETIME2 = NULL,
    @top INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    IF @top IS NULL OR @top < 1 SET @top = 10;

    SELECT TOP (@top)
        P.id_pelicula                                   AS IdPelicula,
        P.titulo                                        AS Titulo,
        CAST(SUM(DF.cantidad) AS INT)                   AS EntradasVendidas,
        CAST(SUM(DF.precio_final_unitario * DF.cantidad) AS DECIMAL(18, 2)) AS Recaudacion
    FROM dbo.facturas F
    INNER JOIN dbo.estados_compras EC ON EC.id_estado_compra = F.id_estado_compra
    INNER JOIN dbo.detalle_facturas DF ON DF.nro_factura = F.nro_factura
    INNER JOIN dbo.entradas E          ON E.id_entrada = DF.id_entrada
    INNER JOIN dbo.funciones FU        ON FU.id_funcion = E.id_funcion
    INNER JOIN dbo.peliculas P         ON P.id_pelicula = FU.id_pelicula
    WHERE EC.estado = 'Pagada'
      AND (@fecha_desde IS NULL OR F.fecha >= CAST(@fecha_desde AS DATE))
      AND (@fecha_hasta IS NULL OR F.fecha < DATEADD(DAY, 1, CAST(@fecha_hasta AS DATE)))
    GROUP BY P.id_pelicula, P.titulo
    ORDER BY EntradasVendidas DESC, Recaudacion DESC, P.titulo;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_INDICADOR_HORARIOS_MAS_POPULARES
    @fecha_desde DATETIME2 = NULL,
    @fecha_hasta DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        H.id_horario                                    AS IdHorario,
        CONVERT(VARCHAR(5), H.horario, 108)             AS Horario,
        CAST(SUM(DF.cantidad) AS INT)                   AS EntradasVendidas,
        COUNT(DISTINCT FU.id_funcion)                   AS FuncionesConVentas,
        CAST(SUM(DF.precio_final_unitario * DF.cantidad) AS DECIMAL(18, 2)) AS Recaudacion
    FROM dbo.facturas F
    INNER JOIN dbo.estados_compras EC ON EC.id_estado_compra = F.id_estado_compra
    INNER JOIN dbo.detalle_facturas DF ON DF.nro_factura = F.nro_factura
    INNER JOIN dbo.entradas E          ON E.id_entrada = DF.id_entrada
    INNER JOIN dbo.funciones FU        ON FU.id_funcion = E.id_funcion
    INNER JOIN dbo.horarios H          ON H.id_horario = FU.id_horario
    WHERE EC.estado = 'Pagada'
      AND (@fecha_desde IS NULL OR F.fecha >= CAST(@fecha_desde AS DATE))
      AND (@fecha_hasta IS NULL OR F.fecha < DATEADD(DAY, 1, CAST(@fecha_hasta AS DATE)))
    GROUP BY H.id_horario, H.horario
    ORDER BY EntradasVendidas DESC, H.horario;
END;
GO

CREATE OR ALTER PROCEDURE dbo.SP_INDICADOR_FORMAS_PAGO_MAS_USADAS
    @fecha_desde DATETIME2 = NULL,
    @fecha_hasta DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        FP.id_forma_pago                                AS IdFormaPago,
        FP.forma_pago                                   AS FormaPago,
        COUNT(*)                                        AS CantidadPagos,
        CAST(SUM(FFP.monto_pagado) AS DECIMAL(18, 2))   AS MontoTotal,
        CAST(100.0 * COUNT(*) / SUM(COUNT(*)) OVER () AS DECIMAL(5, 2)) AS Porcentaje
    FROM dbo.facturas F
    INNER JOIN dbo.estados_compras EC        ON EC.id_estado_compra = F.id_estado_compra
    INNER JOIN dbo.facturas_formas_pagos FFP ON FFP.nro_factura = F.nro_factura
    INNER JOIN dbo.formas_pagos FP           ON FP.id_forma_pago = FFP.id_forma_pago
    WHERE EC.estado = 'Pagada'
      AND (@fecha_desde IS NULL OR F.fecha >= CAST(@fecha_desde AS DATE))
      AND (@fecha_hasta IS NULL OR F.fecha < DATEADD(DAY, 1, CAST(@fecha_hasta AS DATE)))
    GROUP BY FP.id_forma_pago, FP.forma_pago
    ORDER BY CantidadPagos DESC, MontoTotal DESC;
END;
GO
