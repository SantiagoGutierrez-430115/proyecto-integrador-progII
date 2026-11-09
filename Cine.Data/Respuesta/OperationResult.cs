using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Respuesta
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string Mensaje { get; set; }
        public static OperationResult Ok(string message = "Operación exitosa.") =>
            new OperationResult { Success = true, Mensaje = message };

        public static OperationResult Fail(string message) =>
            new OperationResult { Success = false, Mensaje = message };
    }
}
