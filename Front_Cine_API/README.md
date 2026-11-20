# Front_Cine_API — Front conectado a la API TPI_Cine

Front en HTML, CSS (Bootstrap 5) y JavaScript con módulos, con el estilo del front de referencia
"CINE NOIR" (negro, blanco y rojo). A diferencia de ese front, **todos los datos salen de la API**:
no usa datos de ejemplo ni `localStorage` para la información del cine.

## Cómo levantarlo

1. **Base de datos:** ejecutar, en este orden, los scripts de la carpeta `Script SQL`:
   1. `Script_CINER.sql`
   2. `Script_estado_pelicula.sql`
   3. `Datos_peliculas.sql`
   4. `Script_portada_trailer.sql` (columnas de portada y tráiler)
      y después `Datos_portadas_trailers.sql` (pósters y tráileres de las películas reales)
   5. `Script_indicadores_dashboard.sql` (procedimientos del dashboard)
   6. `Script_usuario_admin.sql` (primer administrador)
   7. `Datos_ventas.sql` (opcional: ventas de prueba para que el dashboard tenga datos)
2. **API:** abrir `TPI_Cine.slnx` en Visual Studio e iniciar con el perfil **https**
   (queda en `https://localhost:7032`). La primera vez puede hacer falta confiar en el certificado:
   ```bash
   dotnet dev-certs https --trust
   ```
3. **Front:** abrir la carpeta `Front_Cine_API` en VS Code y abrir `index.html` con la extensión
   **Live Server**. Tiene que quedar en `http://127.0.0.1:5500`, porque es el único origen que permite
   CORS en `Program.cs` (la carpeta `.vscode` ya fija ese puerto).

> Abrir el `index.html` con doble clic (`file:///…`) **no funciona**: el navegador bloquea los módulos
> de JavaScript y las llamadas a la API.

Si la API corre en otra dirección (por ejemplo `http://localhost:5082`), se cambia desde el enlace
**"Dirección de la API"** de la pantalla de login, sin tocar el código. El indicador verde/rojo de esa
pantalla muestra si la API responde.

## Secciones

| Sección | Quién la ve | Endpoints que usa |
|---|---|---|
| Iniciar sesión | Todos | `POST /api/Usuario/login` |
| Crear cuenta | Todos | `POST /api/Usuario/registro` (siempre crea rol Cliente) |
| Entrar como invitado | Todos | Sin token: ve todo lo público pero no compra ni reseña |
| Cartelera + detalle (portada y tráiler) | Todos, incluido invitado | `GET /api/Pelicula`, `GET /api/Pelicula/{id}`, `GET /api/Catalogo/generos` y `clasificaciones` |
| Funciones | Todos, incluido invitado | `GET /api/Funcion`, `/api/Funcion/fecha`, `/api/Funcion/pelicula/{id}` |
| Butacas | Todos (el invitado solo mira) | `GET /api/funciones/{id}/butacas` |
| Compra de entradas | Clientes | `POST /api/compras` |
| Reseñas | Todos ven, clientes crean | `GET /api/Resenia/pelicula/{id}`, `POST /api/Resenia` |
| Candy bar | Todos, incluido invitado | `GET /api/Combo`, `GET /api/ProductoShop` |
| Dashboard | **Admin** | `GET /api/Dashboard/peliculas-mas-vendidas`, `horarios-mas-populares`, `formas-pago-mas-usadas` |
| Portada de película | **Admin** | `POST /api/Pelicula/{id}/portada` (multipart, campo `archivo`) |
| Administradores | **Admin** | `POST /api/Usuario/registro-admin`, `GET /api/Usuario/admins` |
| Soporte de clientes | **Admin** | `GET /api/Clientes?filtro=`, `PUT /api/Clientes/{id}`, `GET /api/Clientes/{id}/reservas` |
| Películas (alta, edición, baja, reactivar) | **Admin** | `POST/PUT/DELETE /api/Pelicula`, `PUT /api/Pelicula/{id}/reactivar`, `GET /api/Catalogo/*` |
| Funciones (programar) | **Admin** | `POST /api/Funcion` |
| Butacas (agregar a una sala) | **Admin** | `POST /api/Butaca`, `GET /api/Butaca/sala/{id}` y `/disponibles` |
| Productos y combos (ABM) | **Admin** | `POST/PUT/DELETE /api/ProductoShop` y `/api/Combo` |
| Reseñas (listado) | **Admin** | `GET /api/Resenia` |

La pestaña **Soporte y gestión** solo aparece si el token es de un usuario con rol `Admin`.
Ocultarla en el front es una comodidad: la seguridad real la pone la API con `[Authorize(Roles = "Admin")]`.

## Cómo crear un administrador

La API solo registra clientes, y en la base no se puede pasar un cliente a Admin: el trigger
`TR_USUARIOS_VALIDAR_CAMBIO_ROL` lo impide. El admin tiene que ser un usuario **sin** cliente asociado.

El **primer** admin se crea con `Script SQL/Script_usuario_admin.sql`: usuario `admin@cine.com`,
contraseña `Admin1234` (ya encriptada con BCrypt como la guarda la API). Los siguientes se crean desde el
front, en **Soporte y gestión → Administradores**, que usa `POST /api/Usuario/registro-admin`.

## Portadas y tráileres

- La portada se sube desde el formulario de la película (JPG, PNG o WEBP, hasta 2 MB). La API guarda el
  archivo en `TPI_Cine/wwwroot/portadas` y en la base queda la ruta. Esa carpeta no se sube a Git
  (cada uno tiene sus propias portadas, igual que su propia base).
- `url_portada` también acepta una URL completa: así vienen los pósters de Wikipedia que carga
  `Datos_portadas_trailers.sql`. Si una imagen no carga, el front muestra el póster generado con la inicial.
- El tráiler es un link de YouTube. La API acepta `youtube.com/watch?v=…`, `youtu.be/…`, `/embed/…` y
  `/shorts/…`, y lo guarda siempre como `https://www.youtube.com/watch?v=ID`. En el detalle de la película
  se reproduce embebido y hay un botón "Ver en YouTube".
Los administradores no tienen un cliente asociado, así que **no pueden comprar entradas** ni dejar reseñas
(la API lo rechaza). Para probar compras se usa una cuenta de cliente.

## Estructura

```
Front_Cine_API/
├── index.html        Todas las pantallas (acceso, cartelera, funciones, candy bar, soporte) y los modales
├── css/styles.css    Estilos (paleta del front de referencia)
└── js/
    ├── config.js     Dirección de la API y catálogos que la API todavía no expone
    ├── api.js        fetch con token JWT y manejo unificado de errores
    ├── auth.js       Login, registro y lectura del token (rol, correo, vencimiento)
    ├── ui.js         Avisos, modales, formatos de fecha/moneda, escape de HTML
    ├── cliente.js    Cartelera, funciones, butacas, compra, reseñas y candy bar
    ├── admin.js      Soporte de clientes y ABM de películas, funciones, butacas, productos y combos
    └── main.js       Arranque y navegación
```

## Limitaciones conocidas (dependen de la API)

- **Salas, horarios, tipos de documento, tipos de butaca, marcas, tipos de producto y tipos de combo**
  no tienen endpoint. Sus valores están copiados de la base en `js/config.js`; si se agregan registros
  en la base hay que actualizarlos ahí. Lo ideal es sumar esos endpoints en `CatalogoController`.
- `FuncionDto` solo devuelve IDs. El front arma el título, la sala y la hora cruzando datos.
- La sucursal 2 no tiene cajero cargado: comprar entradas para las salas 3 y 4 da error 500 en la API.
- `GET /api/funciones/{id}/butacas` no informa el tipo de butaca: el front marca como "preferencial"
  las que cuestan más que la tarifa base.
- La compra registra el pago siempre como transferencia (así está hecho en `CompraRepository`).
