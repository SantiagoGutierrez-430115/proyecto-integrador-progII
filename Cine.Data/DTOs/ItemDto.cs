namespace Cine.Data.DTOs
{
    // Par id + nombre, sirve para los desplegables y para las listas dentro de PeliculaDto
    public class ItemDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
}
