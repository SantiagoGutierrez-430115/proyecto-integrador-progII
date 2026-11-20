namespace Cine.Data.DTOs
{
    // Una fila del indicador "películas más vendidas".
    // Los nombres de las propiedades coinciden con las columnas que devuelve el SP.
    public class PeliculaMasVendidaDto
    {
        public int IdPelicula { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int EntradasVendidas { get; set; }
        public decimal Recaudacion { get; set; }
    }
}
