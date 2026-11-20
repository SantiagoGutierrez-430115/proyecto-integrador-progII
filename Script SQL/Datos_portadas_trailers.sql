/*
    PORTADAS Y TRÁILERES DE LAS PELÍCULAS CARGADAS EN Datos_peliculas.sql

    - Portada: póster de la película en Wikipedia (upload.wikimedia.org). Se guarda la URL completa;
      el front la muestra tal cual. Si un admin sube otra portada desde el front, la reemplaza.
    - Tráiler: video del canal oficial del estudio/productora en YouTube (o del canal licenciado
      "Rotten Tomatoes Classic Trailers" cuando el estudio no publicó el tráiler original).
      Todos se verificaron como existentes e insertables (YouTube oEmbed) el 19/11/2026.

    Las 4 películas inventadas para el proyecto (El último viaje, Código perdido, La ciudad sumergida
    y Rumbo a las estrellas) no tienen póster ni tráiler reales: siguen con el póster generado del front.

    Se actualiza por título (no por id), así funciona en cualquier base aunque los ids sean distintos.
    Ejecutar DESPUÉS de Datos_peliculas.sql y Script_portada_trailer.sql. Se puede ejecutar más de una vez.
*/
USE CINE_R;
GO

SET NOCOUNT ON;

DECLARE @datos TABLE (titulo VARCHAR(150) NOT NULL, url_portada VARCHAR(300) NOT NULL, url_trailer VARCHAR(300) NOT NULL);

INSERT INTO @datos (titulo, url_portada, url_trailer) VALUES
('El secreto de sus ojos',
 'https://upload.wikimedia.org/wikipedia/en/9/91/Cartel-nuevo-de-el-secreto-de-sus-ojos.jpg',
 'https://www.youtube.com/watch?v=hKa8U-8vsfU'),   -- 100 Bares Producciones
('Relatos salvajes',
 'https://upload.wikimedia.org/wikipedia/en/5/5e/Relatos_salvajes.jpg',
 'https://www.youtube.com/watch?v=3BxE9osMt5U'),   -- Warner Bros. Pictures Argentina
('Titanic',
 'https://upload.wikimedia.org/wikipedia/en/1/18/Titanic_%281997_film%29_poster.png',
 'https://www.youtube.com/watch?v=CHekzSiZjrY'),   -- 20th Century Studios
('Avatar',
 'https://upload.wikimedia.org/wikipedia/en/d/d6/Avatar_%282009_film%29_poster.jpg',
 'https://www.youtube.com/watch?v=5PSNL1qE6VY'),   -- 20th Century Studios
('Pulp Fiction',
 'https://upload.wikimedia.org/wikipedia/en/3/3b/Pulp_Fiction_%281994%29_poster.jpg',
 'https://www.youtube.com/watch?v=tGpTpVyI_OQ'),   -- Miramax
('Forrest Gump',
 'https://upload.wikimedia.org/wikipedia/en/6/67/Forrest_Gump_poster.jpg',
 'https://www.youtube.com/watch?v=bLvqoHBptjg'),   -- Paramount Movies
('El padrino',
 'https://upload.wikimedia.org/wikipedia/en/1/1c/Godfather_ver1.jpg',
 'https://www.youtube.com/watch?v=UaVTIH8mujA'),   -- Paramount Pictures (50 aniversario)
('Jurassic Park',
 'https://upload.wikimedia.org/wikipedia/en/e/e7/Jurassic_Park_poster.jpg',
 'https://www.youtube.com/watch?v=QWBKEmWWL38'),   -- Rotten Tomatoes Classic Trailers
('Oppenheimer',
 'https://upload.wikimedia.org/wikipedia/en/4/4a/Oppenheimer_%28film%29.jpg',
 'https://www.youtube.com/watch?v=bK6ldnjE3Y0'),   -- Universal Pictures
('Toy Story',
 'https://upload.wikimedia.org/wikipedia/en/1/13/Toy_Story.jpg',
 'https://www.youtube.com/watch?v=v-PjgYDrg70'),   -- Rotten Tomatoes Classic Trailers
('Matrix',
 'https://upload.wikimedia.org/wikipedia/en/d/db/The_Matrix.png',
 'https://www.youtube.com/watch?v=tGgCqGm_6Hs'),   -- Warner Bros. Rewind
('Batman: El caballero de la noche',
 'https://upload.wikimedia.org/wikipedia/en/1/1c/The_Dark_Knight_%282008_film%29.jpg',
 'https://www.youtube.com/watch?v=TQfATDZY5Y4'),   -- Legendary
('El origen',
 'https://upload.wikimedia.org/wikipedia/en/2/2e/Inception_%282010%29_theatrical_poster.jpg',
 'https://www.youtube.com/watch?v=YoHD9XEInc0'),   -- Rotten Tomatoes Classic Trailers
('Barbie',
 'https://upload.wikimedia.org/wikipedia/en/0/0b/Barbie_2023_poster.jpg',
 'https://www.youtube.com/watch?v=pBk4NYhWNMM'),   -- Warner Bros.
('Duna: Parte dos',
 'https://upload.wikimedia.org/wikipedia/en/5/52/Dune_Part_Two_poster.jpeg',
 'https://www.youtube.com/watch?v=Way9Dexny3w');   -- Warner Bros.

UPDATE P
SET P.url_portada = D.url_portada,
    P.url_trailer = D.url_trailer
FROM dbo.peliculas P
INNER JOIN @datos D ON D.titulo = P.titulo;

PRINT CONCAT('Películas actualizadas: ', @@ROWCOUNT);

-- Títulos del script que no se encontraron en la base (deberían ser 0)
SELECT D.titulo AS no_encontrada
FROM @datos D
WHERE NOT EXISTS (SELECT 1 FROM dbo.peliculas P WHERE P.titulo = D.titulo);
GO

-- Comprobación
SELECT id_pelicula, titulo,
       CASE WHEN url_portada IS NULL THEN '-' ELSE 'sí' END AS portada,
       CASE WHEN url_trailer IS NULL THEN '-' ELSE 'sí' END AS trailer
FROM dbo.peliculas
ORDER BY id_pelicula;
GO
