namespace Cine.Data.Exceptions
{
    // Se lanza cuando la operación choca con algo que ya existe
    // (ej: película duplicada, o película con funciones que no se puede borrar).
    // El controller la atrapa y responde 409.
    public class ConflictoException : Exception
    {
        public ConflictoException(string mensaje) : base(mensaje)
        {
        }
    }
}
