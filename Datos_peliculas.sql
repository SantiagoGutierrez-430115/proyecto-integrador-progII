/*
    DATOS DE PELÍCULAS (reales, para una cartelera creíble)

    Qué hace:
      0) Limpia los datos de prueba ficticios que se habían cargado antes.
         Solo borra películas que NO tienen funciones ni reseñas.
      1) Agrega géneros, distribuidores, directores y actores que faltan.
      2) Agrega 15 películas conocidas con sus géneros, idiomas y actores.
      3) Deja una película como Inactiva (para probar la baja lógica).

    No toca las 4 películas originales de Script_CINER.sql, porque tienen funciones asignadas.
    Se puede ejecutar más de una vez: no duplica nada.

    Ejecutar DESPUÉS de Script_CINER.sql y de Script_estado_pelicula.sql.

    Aclaración: las fechas son los estrenos originales. Las clasificaciones y los títulos
    en español son aproximados: pueden variar según el país y el año; se pueden ajustar.
*/
USE CINE_R;
GO

/* ============================================================
   0) LIMPIEZA de datos de prueba ficticios
   ============================================================ */

-- Películas ficticias y de pruebas hechas desde Swagger (solo si no tienen funciones ni reseñas)
DECLARE @titulos_prueba TABLE (titulo VARCHAR(150));
INSERT INTO @titulos_prueba (titulo) VALUES
    ('Noches de neón'), ('El último verano'), ('Risas en cadena'), ('Frontera cero'),
    ('Planeta rojo'), ('Cartas al mar'), ('Sombras del bosque'), ('Memorias de verano'),
    ('La gran escapada'), ('Dos mundos'),
    ('Película de prueba'), ('Película editada'), ('Prueba simple'),
    ('Prueba de estado'), ('Película mínima');

DECLARE @ids_borrar TABLE (id_pelicula INT);
INSERT INTO @ids_borrar (id_pelicula)
SELECT P.id_pelicula
FROM dbo.peliculas P
WHERE P.titulo IN (SELECT titulo FROM @titulos_prueba)
  AND NOT EXISTS (SELECT 1 FROM dbo.funciones F WHERE F.id_pelicula = P.id_pelicula)
  AND NOT EXISTS (SELECT 1 FROM dbo.resenias_peliculas R WHERE R.id_pelicula = P.id_pelicula);

DELETE FROM dbo.peliculas_generos WHERE id_pelicula IN (SELECT id_pelicula FROM @ids_borrar);
DELETE FROM dbo.peliculas_idiomas WHERE id_pelicula IN (SELECT id_pelicula FROM @ids_borrar);
DELETE FROM dbo.peliculas_actores WHERE id_pelicula IN (SELECT id_pelicula FROM @ids_borrar);
DELETE FROM dbo.peliculas         WHERE id_pelicula IN (SELECT id_pelicula FROM @ids_borrar);
GO

-- Actores ficticios (solo si ninguna película los usa)
DELETE A
FROM dbo.actores A
WHERE ((A.nombre = 'Camila'  AND A.apellido = 'Rivas')
    OR (A.nombre = 'Julián'  AND A.apellido = 'Ortega')
    OR (A.nombre = 'Valeria' AND A.apellido = 'Sosa')
    OR (A.nombre = 'Mateo'   AND A.apellido = 'Herrera')
    OR (A.nombre = 'Lucía'   AND A.apellido = 'Benítez')
    OR (A.nombre = 'Nicolás' AND A.apellido = 'Paz')
    OR (A.nombre = 'Sofía'   AND A.apellido = 'Quiroga')
    OR (A.nombre = 'Tomás'   AND A.apellido = 'Aguirre'))
  AND NOT EXISTS (SELECT 1 FROM dbo.peliculas_actores PA WHERE PA.id_actor = A.id_actor);
GO

-- Directores ficticios (solo si ninguna película los usa)
DELETE D
FROM dbo.directores D
WHERE ((D.nombre = 'Paula'  AND D.apellido = 'Ríos')
    OR (D.nombre = 'Andrés' AND D.apellido = 'Molina'))
  AND NOT EXISTS (SELECT 1 FROM dbo.peliculas P WHERE P.id_director = D.id_director);
GO

/* ============================================================
   1) CATÁLOGOS que faltan
   ============================================================ */

INSERT INTO dbo.generos (genero)
SELECT V.valor
FROM (VALUES ('Animacion'), ('Aventura'), ('Suspenso'), ('Biografia')) V(valor)
WHERE NOT EXISTS (SELECT 1 FROM dbo.generos T WHERE T.genero = V.valor);
GO

INSERT INTO dbo.distribuidores (distribuidor)
SELECT V.valor
FROM (VALUES
    ('Paramount Pictures'),
    ('Universal Pictures'),
    ('Warner Bros. Pictures'),
    ('Walt Disney Pictures'),
    ('20th Century Fox'),
    ('Miramax')
) V(valor)
WHERE NOT EXISTS (SELECT 1 FROM dbo.distribuidores T WHERE T.distribuidor = V.valor);
GO

INSERT INTO dbo.directores (nombre, apellido, fecha_nacimiento, bio)
SELECT V.nombre, V.apellido, V.fecha_nacimiento, V.bio
FROM (VALUES
    ('Francis Ford', 'Coppola',     '1939-04-07', 'Director estadounidense.'),
    ('James',        'Cameron',     '1954-08-16', 'Director canadiense.'),
    ('Steven',       'Spielberg',   '1946-12-18', 'Director estadounidense.'),
    ('John',         'Lasseter',    '1957-01-12', 'Director estadounidense de cine de animación.'),
    ('Lana',         'Wachowski',   '1965-06-21', 'Directora estadounidense.'),
    ('Christopher',  'Nolan',       '1970-07-30', 'Director británico.'),
    ('Quentin',      'Tarantino',   '1963-03-27', 'Director estadounidense.'),
    ('Robert',       'Zemeckis',    '1952-05-14', 'Director estadounidense.'),
    ('Juan José',    'Campanella',  '1959-07-19', 'Director argentino.'),
    ('Damián',       'Szifron',     '1975-07-09', 'Director argentino.'),
    ('Greta',        'Gerwig',      '1983-08-04', 'Directora estadounidense.'),
    ('Denis',        'Villeneuve',  '1967-10-03', 'Director canadiense.')
) V(nombre, apellido, fecha_nacimiento, bio)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.directores D
    WHERE D.nombre = V.nombre AND D.apellido = V.apellido
);
GO

INSERT INTO dbo.actores (nombre, apellido)
SELECT V.nombre, V.apellido
FROM (VALUES
    ('Marlon', 'Brando'), ('Al', 'Pacino'), ('James', 'Caan'),
    ('Leonardo', 'DiCaprio'), ('Kate', 'Winslet'),
    ('Sam', 'Neill'), ('Laura', 'Dern'), ('Jeff', 'Goldblum'),
    ('Tom', 'Hanks'), ('Tim', 'Allen'),
    ('Keanu', 'Reeves'), ('Laurence', 'Fishburne'), ('Carrie-Anne', 'Moss'),
    ('Christian', 'Bale'), ('Heath', 'Ledger'), ('Aaron', 'Eckhart'),
    ('Joseph', 'Gordon-Levitt'), ('Elliot', 'Page'), ('Tom', 'Hardy'),
    ('Sam', 'Worthington'), ('Zoe', 'Saldaña'), ('Sigourney', 'Weaver'),
    ('John', 'Travolta'), ('Samuel L.', 'Jackson'), ('Uma', 'Thurman'),
    ('Robin', 'Wright'),
    ('Ricardo', 'Darín'), ('Soledad', 'Villamil'), ('Guillermo', 'Francella'),
    ('Oscar', 'Martínez'), ('Érica', 'Rivas'), ('Leonardo', 'Sbaraglia'),
    ('Cillian', 'Murphy'), ('Emily', 'Blunt'), ('Matt', 'Damon'), ('Robert', 'Downey Jr.'),
    ('Margot', 'Robbie'), ('Ryan', 'Gosling'), ('America', 'Ferrera'),
    ('Timothée', 'Chalamet'), ('Zendaya', 'Coleman'), ('Rebecca', 'Ferguson')
) V(nombre, apellido)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.actores A
    WHERE A.nombre = V.nombre AND A.apellido = V.apellido
);
GO

/* ============================================================
   2) PELÍCULAS
   ============================================================ */

INSERT INTO dbo.peliculas
    (titulo, descripcion, fecha_lanzamiento,
     id_pais, id_formato_pelicula, id_clasificacion, id_distribuidor, id_director)
SELECT
    V.titulo, V.descripcion, V.fecha_lanzamiento,
    PA.id_pais, FO.id_formato_pelicula, CL.id_clasificacion, DI.id_distribuidor, DR.id_director
FROM (VALUES
    ('El padrino',        'La saga de una familia mafiosa de Nueva York y el traspaso del poder de padre a hijo.',          '1972-03-24', 'Estados Unidos', '2D', '+16', 'Paramount Pictures',    'Francis Ford', 'Coppola'),
    ('Titanic',           'Una historia de amor entre dos jóvenes de distinta clase a bordo del célebre transatlántico.',   '1997-12-19', 'Estados Unidos', '2D', '+13', '20th Century Fox',      'James',        'Cameron'),
    ('Jurassic Park',     'Un parque temático con dinosaurios clonados se vuelve incontrolable.',                          '1993-06-11', 'Estados Unidos', '2D', '+13', 'Universal Pictures',    'Steven',       'Spielberg'),
    ('Toy Story',         'Los juguetes de un niño cobran vida cuando nadie los mira.',                                    '1995-11-22', 'Estados Unidos', '2D', 'ATP', 'Walt Disney Pictures',  'John',         'Lasseter'),
    ('Matrix',            'Un programador descubre que la realidad que conoce es una simulación.',                         '1999-03-31', 'Estados Unidos', '2D', '+16', 'Warner Bros. Pictures', 'Lana',         'Wachowski'),
    ('Batman: El caballero de la noche', 'Batman enfrenta al Guasón, que sume a Ciudad Gótica en el caos.',                '2008-07-18', 'Estados Unidos', '2D', '+13', 'Warner Bros. Pictures', 'Christopher',  'Nolan'),
    ('El origen',         'Un ladrón especializado en robar secretos a través de los sueños recibe un último trabajo.',    '2010-07-16', 'Estados Unidos', '2D', '+13', 'Warner Bros. Pictures', 'Christopher',  'Nolan'),
    ('Avatar',            'Un exsoldado se une a los habitantes de Pandora en el conflicto contra los colonos humanos.',   '2009-12-18', 'Estados Unidos', '3D', '+13', '20th Century Fox',      'James',        'Cameron'),
    ('Pulp Fiction',      'Varias historias de criminales de Los Ángeles se cruzan de forma inesperada.',                  '1994-10-14', 'Estados Unidos', '2D', '+18', 'Miramax',               'Quentin',      'Tarantino'),
    ('Forrest Gump',      'Un hombre de gran corazón atraviesa décadas de historia estadounidense.',                       '1994-07-06', 'Estados Unidos', '2D', '+13', 'Paramount Pictures',    'Robert',       'Zemeckis'),
    ('El secreto de sus ojos', 'Un exempleado judicial vuelve a un caso sin resolver que marcó su vida.',                  '2009-08-13', 'Argentina',      '2D', '+16', NULL,                    'Juan José',    'Campanella'),
    ('Relatos salvajes',  'Seis historias de venganza y situaciones límite en la vida cotidiana.',                         '2014-08-21', 'Argentina',      '2D', '+16', NULL,                    'Damián',       'Szifron'),
    ('Oppenheimer',       'La historia del físico que lideró el desarrollo de la bomba atómica.',                          '2023-07-21', 'Estados Unidos', '2D', '+16', 'Universal Pictures',    'Christopher',  'Nolan'),
    ('Barbie',            'Barbie deja Barbieland y descubre el mundo real.',                                              '2023-07-21', 'Estados Unidos', '2D', '+13', 'Warner Bros. Pictures', 'Greta',        'Gerwig'),
    ('Duna: Parte dos',   'Paul Atreides se une a los Fremen para vengar a su familia en el planeta desierto.',            '2024-03-01', 'Estados Unidos', '2D', '+13', 'Warner Bros. Pictures', 'Denis',        'Villeneuve')
) V(titulo, descripcion, fecha_lanzamiento, pais, formato, clasificacion, distribuidor, nombre_director, apellido_director)
INNER JOIN dbo.paises PA             ON PA.pais = V.pais
INNER JOIN dbo.formatos_peliculas FO ON FO.formato = V.formato
INNER JOIN dbo.clasificaciones CL    ON CL.clasificacion = V.clasificacion
LEFT  JOIN dbo.distribuidores DI     ON DI.distribuidor = V.distribuidor
INNER JOIN dbo.directores DR         ON DR.nombre = V.nombre_director AND DR.apellido = V.apellido_director
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.peliculas P
    WHERE P.titulo = V.titulo
      AND P.fecha_lanzamiento = CAST(V.fecha_lanzamiento AS DATE)
);
GO

/* ============================================================
   3) GÉNEROS, IDIOMAS y ACTORES de cada película
   ============================================================ */

INSERT INTO dbo.peliculas_generos (id_pelicula, id_genero)
SELECT P.id_pelicula, G.id_genero
FROM (VALUES
    ('El padrino', 'Drama'),
    ('Titanic', 'Drama'),
    ('Jurassic Park', 'Aventura'), ('Jurassic Park', 'Ciencia ficcion'),
    ('Toy Story', 'Animacion'), ('Toy Story', 'Comedia'),
    ('Matrix', 'Accion'), ('Matrix', 'Ciencia ficcion'),
    ('Batman: El caballero de la noche', 'Accion'), ('Batman: El caballero de la noche', 'Suspenso'),
    ('El origen', 'Ciencia ficcion'), ('El origen', 'Accion'),
    ('Avatar', 'Ciencia ficcion'), ('Avatar', 'Aventura'),
    ('Pulp Fiction', 'Suspenso'), ('Pulp Fiction', 'Drama'),
    ('Forrest Gump', 'Drama'), ('Forrest Gump', 'Comedia'),
    ('El secreto de sus ojos', 'Suspenso'), ('El secreto de sus ojos', 'Drama'),
    ('Relatos salvajes', 'Comedia'), ('Relatos salvajes', 'Drama'),
    ('Oppenheimer', 'Biografia'), ('Oppenheimer', 'Drama'),
    ('Barbie', 'Comedia'), ('Barbie', 'Aventura'),
    ('Duna: Parte dos', 'Ciencia ficcion'), ('Duna: Parte dos', 'Aventura')
) V(titulo, genero)
INNER JOIN dbo.peliculas P ON P.titulo = V.titulo
INNER JOIN dbo.generos G   ON G.genero = V.genero
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.peliculas_generos PG
    WHERE PG.id_pelicula = P.id_pelicula AND PG.id_genero = G.id_genero
);
GO

INSERT INTO dbo.peliculas_idiomas (id_pelicula, id_idioma)
SELECT P.id_pelicula, I.id_idioma
FROM (VALUES
    ('El padrino', 'Ingles'), ('El padrino', 'Español'),
    ('Titanic', 'Ingles'), ('Titanic', 'Español'),
    ('Jurassic Park', 'Ingles'), ('Jurassic Park', 'Español'),
    ('Toy Story', 'Ingles'), ('Toy Story', 'Español'),
    ('Matrix', 'Ingles'), ('Matrix', 'Español'),
    ('Batman: El caballero de la noche', 'Ingles'), ('Batman: El caballero de la noche', 'Español'),
    ('El origen', 'Ingles'), ('El origen', 'Español'),
    ('Avatar', 'Ingles'), ('Avatar', 'Español'),
    ('Pulp Fiction', 'Ingles'),
    ('Forrest Gump', 'Ingles'), ('Forrest Gump', 'Español'),
    ('El secreto de sus ojos', 'Español'),
    ('Relatos salvajes', 'Español'),
    ('Oppenheimer', 'Ingles'), ('Oppenheimer', 'Español'),
    ('Barbie', 'Ingles'), ('Barbie', 'Español'),
    ('Duna: Parte dos', 'Ingles'), ('Duna: Parte dos', 'Español')
) V(titulo, idioma)
INNER JOIN dbo.peliculas P ON P.titulo = V.titulo
INNER JOIN dbo.idiomas I   ON I.idioma = V.idioma
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.peliculas_idiomas PI
    WHERE PI.id_pelicula = P.id_pelicula AND PI.id_idioma = I.id_idioma
);
GO

INSERT INTO dbo.peliculas_actores (id_pelicula, id_actor)
SELECT P.id_pelicula, A.id_actor
FROM (VALUES
    ('El padrino', 'Marlon', 'Brando'), ('El padrino', 'Al', 'Pacino'), ('El padrino', 'James', 'Caan'),
    ('Titanic', 'Leonardo', 'DiCaprio'), ('Titanic', 'Kate', 'Winslet'),
    ('Jurassic Park', 'Sam', 'Neill'), ('Jurassic Park', 'Laura', 'Dern'), ('Jurassic Park', 'Jeff', 'Goldblum'),
    ('Toy Story', 'Tom', 'Hanks'), ('Toy Story', 'Tim', 'Allen'),
    ('Matrix', 'Keanu', 'Reeves'), ('Matrix', 'Laurence', 'Fishburne'), ('Matrix', 'Carrie-Anne', 'Moss'),
    ('Batman: El caballero de la noche', 'Christian', 'Bale'), ('Batman: El caballero de la noche', 'Heath', 'Ledger'), ('Batman: El caballero de la noche', 'Aaron', 'Eckhart'),
    ('El origen', 'Leonardo', 'DiCaprio'), ('El origen', 'Joseph', 'Gordon-Levitt'), ('El origen', 'Elliot', 'Page'), ('El origen', 'Tom', 'Hardy'),
    ('Avatar', 'Sam', 'Worthington'), ('Avatar', 'Zoe', 'Saldaña'), ('Avatar', 'Sigourney', 'Weaver'),
    ('Pulp Fiction', 'John', 'Travolta'), ('Pulp Fiction', 'Samuel L.', 'Jackson'), ('Pulp Fiction', 'Uma', 'Thurman'),
    ('Forrest Gump', 'Tom', 'Hanks'), ('Forrest Gump', 'Robin', 'Wright'),
    ('El secreto de sus ojos', 'Ricardo', 'Darín'), ('El secreto de sus ojos', 'Soledad', 'Villamil'), ('El secreto de sus ojos', 'Guillermo', 'Francella'),
    ('Relatos salvajes', 'Ricardo', 'Darín'), ('Relatos salvajes', 'Oscar', 'Martínez'), ('Relatos salvajes', 'Érica', 'Rivas'), ('Relatos salvajes', 'Leonardo', 'Sbaraglia'),
    ('Oppenheimer', 'Cillian', 'Murphy'), ('Oppenheimer', 'Emily', 'Blunt'), ('Oppenheimer', 'Matt', 'Damon'), ('Oppenheimer', 'Robert', 'Downey Jr.'),
    ('Barbie', 'Margot', 'Robbie'), ('Barbie', 'Ryan', 'Gosling'), ('Barbie', 'America', 'Ferrera'),
    ('Duna: Parte dos', 'Timothée', 'Chalamet'), ('Duna: Parte dos', 'Zendaya', 'Coleman'), ('Duna: Parte dos', 'Rebecca', 'Ferguson')
) V(titulo, nombre, apellido)
INNER JOIN dbo.peliculas P ON P.titulo = V.titulo
INNER JOIN dbo.actores A   ON A.nombre = V.nombre AND A.apellido = V.apellido
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.peliculas_actores PA
    WHERE PA.id_pelicula = P.id_pelicula AND PA.id_actor = A.id_actor
);
GO

/* ============================================================
   4) Una película inactiva, para probar la baja lógica
      (solo si ya existe la columna de estado)
   ============================================================ */
IF COL_LENGTH('dbo.peliculas', 'id_estado_pelicula') IS NOT NULL
BEGIN
    EXEC sp_executesql N'
        UPDATE P
        SET P.id_estado_pelicula = E.id_estado_pelicula
        FROM dbo.peliculas P
        CROSS JOIN dbo.estados_peliculas E
        WHERE P.titulo = ''Forrest Gump''
          AND E.estado = ''Inactiva'';';
END
GO

/* ============================================================
   5) Verificación
   ============================================================ */
SELECT COUNT(*) AS peliculas_total FROM dbo.peliculas;

-- Películas que quedaron (las 4 originales + las 15 reales; más alguna tuya si la creaste)
SELECT p.id_pelicula, p.titulo, p.fecha_lanzamiento
FROM dbo.peliculas p
ORDER BY p.id_pelicula;
GO
