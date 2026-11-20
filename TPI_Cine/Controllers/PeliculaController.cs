using Cine.Data.DTOs;
using Cine.Data.Exceptions;
using Cine.Data.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TPI_Cine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculaController : ControllerBase
    {
        private readonly IPeliculaService _peliculaService;
        private readonly IWebHostEnvironment _env;

        // Portadas: hasta 2 MB, solo imágenes JPG, PNG o WEBP
        private const long TamanioMaximoPortada = 2 * 1024 * 1024;
        private static readonly string[] ExtensionesPortada = { ".jpg", ".jpeg", ".png", ".webp" };

        public PeliculaController(IPeliculaService peliculaService, IWebHostEnvironment env)
        {
            _peliculaService = peliculaService;
            _env = env;
        }

        // incluirInactivas=true solo tiene efecto si quien pide es Admin
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? titulo, [FromQuery] int? idGenero, [FromQuery] int? idClasificacion,
            [FromQuery] bool incluirInactivas = false, [FromQuery] int pagina = 1, [FromQuery] int tamanio = 10)
        {
            try
            {
                // Solo el Admin puede ver las películas inactivas
                var verInactivas = incluirInactivas && User.IsInRole("Admin");

                var resultado = await _peliculaService.GetAllAsync(titulo, idGenero, idClasificacion, verInactivas, pagina, tamanio);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener las películas: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var pelicula = await _peliculaService.GetByIdAsync(id, User.IsInRole("Admin"));
                if (pelicula == null)
                {
                    return NotFound($"No existe la película {id}.");
                }

                return Ok(pelicula);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener la película: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] GuardarPeliculaDto dto)
        {
            try
            {
                var pelicula = await _peliculaService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = pelicula.IdPelicula }, pelicula);
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ConflictoException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al crear la película: {ex.Message}");
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] GuardarPeliculaDto dto)
        {
            try
            {
                var actualizada = await _peliculaService.UpdateAsync(id, dto);
                if (!actualizada)
                {
                    return NotFound($"No existe la película {id}.");
                }

                return NoContent();
            }
            catch (DatosInvalidosException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ConflictoException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al actualizar la película: {ex.Message}");
            }
        }

        // Baja lógica: la película no se borra, queda como Inactiva
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var dadaDeBaja = await _peliculaService.DarDeBajaAsync(id);
                if (!dadaDeBaja)
                {
                    return NotFound($"No existe la película {id}.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al dar de baja la película: {ex.Message}");
            }
        }

        // Sube (o reemplaza) la foto de portada. Se manda como multipart/form-data con el campo "archivo".
        // El archivo queda en wwwroot/portadas y la API lo sirve en /portadas/<nombre>.
        [HttpPost("{id:int}/portada")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(TamanioMaximoPortada + 100_000)]
        public async Task<IActionResult> SubirPortada(int id, IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
                return BadRequest(new { mensaje = "Tenés que elegir una imagen." });

            if (archivo.Length > TamanioMaximoPortada)
                return BadRequest(new { mensaje = "La imagen no puede superar los 2 MB." });

            var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();
            if (!ExtensionesPortada.Contains(extension) || !archivo.ContentType.StartsWith("image/"))
                return BadRequest(new { mensaje = "La portada tiene que ser una imagen JPG, PNG o WEBP." });

            var carpeta = CarpetaPortadas();
            Directory.CreateDirectory(carpeta);

            // El nombre lleva la fecha para que el navegador no muestre la imagen vieja guardada en caché
            var nombre = $"pelicula-{id}-{DateTime.UtcNow:yyyyMMddHHmmss}{extension}";
            var rutaArchivo = Path.Combine(carpeta, nombre);

            try
            {
                await using (var destino = System.IO.File.Create(rutaArchivo))
                {
                    await archivo.CopyToAsync(destino);
                }

                var (existe, portadaAnterior) = await _peliculaService.ActualizarPortadaAsync(id, $"/portadas/{nombre}");
                if (!existe)
                {
                    System.IO.File.Delete(rutaArchivo);
                    return NotFound(new { mensaje = $"No existe la película {id}." });
                }

                BorrarPortadaVieja(portadaAnterior);
                return Ok(new { mensaje = "Portada actualizada.", urlPortada = $"/portadas/{nombre}" });
            }
            catch (Exception)
            {
                if (System.IO.File.Exists(rutaArchivo)) System.IO.File.Delete(rutaArchivo);
                return StatusCode(500, new { mensaje = "No se pudo guardar la portada." });
            }
        }

        // Misma carpeta que sirve UseStaticFiles en Program.cs
        private string CarpetaPortadas() =>
            Path.Combine(_env.ContentRootPath, "wwwroot", "portadas");

        // Borra el archivo de la portada reemplazada (solo si está dentro de la carpeta de portadas)
        private void BorrarPortadaVieja(string? urlAnterior)
        {
            if (string.IsNullOrEmpty(urlAnterior) || !urlAnterior.StartsWith("/portadas/")) return;

            var ruta = Path.Combine(CarpetaPortadas(), Path.GetFileName(urlAnterior));
            if (System.IO.File.Exists(ruta)) System.IO.File.Delete(ruta);
        }

        // Vuelve a dejar la película como Activa
        [HttpPut("{id:int}/reactivar")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reactivar(int id)
        {
            try
            {
                var reactivada = await _peliculaService.ReactivarAsync(id);
                if (!reactivada)
                {
                    return NotFound($"No existe la película {id}.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al reactivar la película: {ex.Message}");
            }
        }
    }
}
