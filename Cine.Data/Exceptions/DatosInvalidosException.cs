namespace Cine.Data.Exceptions
{
    // Se lanza cuando los datos que llegan no son válidos (ej: un id que no existe).
    // El controller la atrapa y responde 400.
    public class DatosInvalidosException : Exception
    {
        public DatosInvalidosException(string mensaje) : base(mensaje)
        {
        }
    }
}
