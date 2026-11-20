namespace Cine.Data.DTOs
{
    // Una fila del indicador "formas de pago más usadas".
    public class FormaPagoUsadaDto
    {
        public int IdFormaPago { get; set; }
        public string FormaPago { get; set; } = string.Empty;
        public int CantidadPagos { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal Porcentaje { get; set; }
    }
}
