using System.ComponentModel.DataAnnotations;

namespace Cine.Data.DTOs
{
    // Se usa tanto para crear (POST) como para actualizar (PUT)
    public class GuardarPeliculaDto
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(150, ErrorMessage = "El título no puede superar los 150 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha de lanzamiento es obligatoria.")]
        public DateOnly? FechaLanzamiento { get; set; }

        public int? IdPais { get; set; }
        public int? IdFormatoPelicula { get; set; }
        public int? IdClasificacion { get; set; }
        public int? IdDistribuidor { get; set; }
        public int? IdDirector { get; set; }

        // Ids de las tablas intermedias (se reemplazan completos en cada guardado)
        public List<int> IdsGeneros { get; set; } = new();
        public List<int> IdsIdiomas { get; set; } = new();
        public List<int> IdsActores { get; set; } = new();
    }
}
