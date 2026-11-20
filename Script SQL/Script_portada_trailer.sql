/*
    PORTADA Y TRÁILER DE PELÍCULAS
    Agrega dos columnas opcionales a dbo.peliculas:
      - url_portada: ruta de la imagen que sube el admin. La API guarda el archivo en
                     TPI_Cine/wwwroot/portadas y acá queda la ruta relativa (ej: /portadas/pelicula-8.jpg).
      - url_trailer: link de YouTube del tráiler (la API valida que sea de YouTube).

    Ejecutar DESPUÉS de Script_CINER.sql. Es seguro ejecutarlo más de una vez.
*/
USE CINE_R;
GO

IF COL_LENGTH('dbo.peliculas', 'url_portada') IS NULL
    ALTER TABLE dbo.peliculas ADD url_portada VARCHAR(300) NULL;
GO

IF COL_LENGTH('dbo.peliculas', 'url_trailer') IS NULL
    ALTER TABLE dbo.peliculas ADD url_trailer VARCHAR(300) NULL;
GO

-- Comprobación
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'peliculas' AND COLUMN_NAME IN ('url_portada', 'url_trailer');
GO
