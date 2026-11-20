-- ============================================================================
-- Crea un usuario ADMINISTRADOR para entrar a "Soporte y gestión".
--
-- Por qué un INSERT y no un UPDATE: el trigger TR_USUARIOS_VALIDAR_CAMBIO_ROL
-- no deja cambiar el rol de un usuario que ya tiene un cliente asociado.
-- Un admin es un usuario SIN fila en la tabla clientes.
--
-- La contraseña se guarda encriptada con BCrypt (igual que hace la API al registrar).
--   Correo:     admin@cine.com
--   Contraseña: Admin1234
-- Cambiala desde la base si el proyecto se publica.
-- ============================================================================
USE CINE_R;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.usuarios WHERE correo_electronico = 'admin@cine.com')
BEGIN
    INSERT INTO dbo.usuarios (nombre_usuario, contraseña, id_rol, correo_electronico)
    VALUES
    (
        'admin',
        '$2a$11$01myWRQD5mWAHPCg4TfUuuyDBk8JUhqh2TKIQUOr3qix59nuXW..m',  -- BCrypt de "Admin1234"
        (SELECT id_rol FROM dbo.roles WHERE descripcion = 'Admin'),
        'admin@cine.com'
    );
END;
GO

-- Comprobación
SELECT U.id_usuario, U.nombre_usuario, U.correo_electronico, R.descripcion AS rol
FROM dbo.usuarios U
INNER JOIN dbo.roles R ON R.id_rol = U.id_rol
WHERE U.correo_electronico = 'admin@cine.com';
GO
