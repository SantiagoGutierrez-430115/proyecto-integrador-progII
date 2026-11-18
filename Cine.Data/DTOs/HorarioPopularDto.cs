namespace Cine.Data.DTOs
{
    // Una fila del indicador "horarios más populares". Horario viene como texto "HH:mm" (ej: "19:30").
    public class HorarioPopularDto
    {
        public int IdHorario { get; set; }
        public string Horario { get; set; } = string.Empty;
        public int EntradasVendidas { get; set; }
        public int FuncionesConVentas { get; set; }
        public decimal Recaudacion { get; set; }
    }
}
