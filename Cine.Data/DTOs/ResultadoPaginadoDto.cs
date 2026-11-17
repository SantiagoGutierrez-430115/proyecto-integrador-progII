namespace Cine.Data.DTOs
{
    public class ResultadoPaginadoDto<T>
    {
        public List<T> Items { get; set; } = new();
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int Tamanio { get; set; }
    }
}
