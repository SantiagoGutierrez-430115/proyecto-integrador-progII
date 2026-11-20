namespace Cine.Data.DTOs
{
    // Una fila del indicador "películas más vendidas".
    public class PeliculaMasVendidaDto
    {
        public int IdPelicula { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public int EntradasVendidas { get; set; }
        public decimal Recaudacion { get; set; }
    }
}
