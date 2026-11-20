namespace Cine.Data.DTOs
{
    // Lo que devuelve la API (listado y detalle)
    public class PeliculaDto
    {
        public int IdPelicula { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public DateOnly FechaLanzamiento { get; set; }

        public int? IdPais { get; set; }
        public string? Pais { get; set; }
        public int? IdFormatoPelicula { get; set; }
        public string? Formato { get; set; }
        public int? IdClasificacion { get; set; }
        public string? Clasificacion { get; set; }
        public int? IdDistribuidor { get; set; }
        public string? Distribuidor { get; set; }
        public int? IdDirector { get; set; }
        public string? Director { get; set; }
        public int IdEstadoPelicula { get; set; }
        public string? Estado { get; set; }

        // Ruta relativa de la portada en la API (ej: /portadas/pelicula-8.jpg) o null
        public string? UrlPortada { get; set; }
        // Link de YouTube del tráiler o null
        public string? UrlTrailer { get; set; }

        public List<ItemDto> Generos { get; set; } = new();
        public List<ItemDto> Idiomas { get; set; } = new();
        public List<ItemDto> Actores { get; set; } = new();
    }
}
