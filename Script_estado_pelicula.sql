/*
    MIGRACION: estado de película (baja lógica)

    - Crea la tabla dbo.estados_peliculas con dos estados: Activa (1) e Inactiva (2).
    - Agrega la columna id_estado_pelicula a dbo.peliculas (por defecto 1 = Activa).
    - Es seguro ejecutarlo más de una vez: si algo ya existe, lo saltea.
    - No borra ni modifica datos existentes: las películas actuales quedan "Activa".

    Ejecutar en SSMS contra la base CINE_R.
*/
USE CINE_R;
GO

-- 1) Tabla de estados (mismo estilo que estados_reservas)
IF OBJECT_ID('dbo.estados_peliculas', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.estados_peliculas
    (
        id_estado_pelicula INT IDENTITY(1,1) PRIMARY KEY,
        estado VARCHAR(20) NOT NULL UNIQUE
    );

    -- Se cargan en este orden para que Activa sea el id 1 e Inactiva el id 2
    INSERT INTO dbo.estados_peliculas (estado)
    VALUES ('Activa'), ('Inactiva');
END
GO

-- 2) Columna en peliculas. El DEFAULT 1 deja las películas existentes como Activa
--    y hace que SP_CREAR_PELICULA siga funcionando sin cambios.
IF COL_LENGTH('dbo.peliculas', 'id_estado_pelicula') IS NULL
BEGIN
    ALTER TABLE dbo.peliculas
    ADD id_estado_pelicula INT NOT NULL
        CONSTRAINT DF_peliculas_estado DEFAULT 1;
END
GO

-- 3) Clave foránea (va en un lote aparte porque usa la columna recién creada)
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_peliculas_estados_peliculas')
BEGIN
    ALTER TABLE dbo.peliculas
    ADD CONSTRAINT FK_peliculas_estados_peliculas
        FOREIGN KEY (id_estado_pelicula)
        REFERENCES dbo.estados_peliculas (id_estado_pelicula);
END
GO

-- 4) Verificación: tiene que mostrar los 2 estados y todas las películas con estado 'Activa'
SELECT * FROM dbo.estados_peliculas;

SELECT p.id_pelicula, p.titulo, e.estado
FROM dbo.peliculas p
INNER JOIN dbo.estados_peliculas e ON e.id_estado_pelicula = p.id_estado_pelicula
ORDER BY p.id_pelicula;
GO
