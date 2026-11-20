namespace Cine.Data.Respuesta
{
    // Indica POR QUÉ falló una operación, para que el controller
    // pueda devolver el código HTTP correcto (404, 409, 400...).
    public enum TipoError
    {
        Ninguno,
        NoEncontrado,   // -> 404 Not Found
        Conflicto,      // -> 409 Conflict (ej: DNI o correo ya usados)
        Validacion      // -> 400 Bad Request
    }

    public class OperationResult
    {
        public bool Success { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public TipoError Error { get; set; } = TipoError.Ninguno;

        public static OperationResult Ok(string message = "Operación exitosa.") =>
            new OperationResult { Success = true, Mensaje = message };

        public static OperationResult Fail(string message, TipoError error = TipoError.Validacion) =>
            new OperationResult { Success = false, Mensaje = message, Error = error };

        public static OperationResult NotFound(string message) => Fail(message, TipoError.NoEncontrado);

        public static OperationResult Conflict(string message) => Fail(message, TipoError.Conflicto);
    }

    // Versión genérica: además del mensaje devuelve datos (ej: la lista de clientes encontrados).
    public class OperationResult<T> : OperationResult
    {
        public T? Data { get; set; }

        public static OperationResult<T> Ok(T data, string message = "Operación exitosa.") =>
            new OperationResult<T> { Success = true, Mensaje = message, Data = data };

        public static new OperationResult<T> NotFound(string message) =>
            new OperationResult<T> { Success = false, Mensaje = message, Error = TipoError.NoEncontrado };

        public static new OperationResult<T> Fail(string message, TipoError error = TipoError.Validacion) =>
            new OperationResult<T> { Success = false, Mensaje = message, Error = error };
    }
}
