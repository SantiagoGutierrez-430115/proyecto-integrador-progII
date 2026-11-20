using Cine.Data.DTOs;
using Cine.Data.Models;
using Cine.Data.Repositories.Interfaces;
using Cine.Data.Respuesta;
using Cine.Data.Services.Interfaces;

namespace Cine.Data.Services.Implementations
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepo;
        public ClienteService(IClienteRepository clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        public async Task<OperationResult> ActualizarCliente(int codCliente, ActualizarClienteDto dto)
        {
            var cliente = await _clienteRepo.ObtenerPorId(codCliente);
            if (cliente == null)
            {
                return OperationResult.NotFound("El cliente especificado no existe en el sistema.");
            }

            // Se limpian espacios al principio y al final ("  Juan " -> "Juan")
            var nombre = dto.Nombre?.Trim();
            var apellido = dto.Apellido?.Trim();
            var nroDoc = dto.NroDoc?.Trim();
            var correo = dto.CorreoElectronico?.Trim();

            bool huboCambio = false;

            if (!string.IsNullOrEmpty(nombre) && nombre != cliente.Nombre)
            {
                cliente.Nombre = nombre;
                huboCambio = true;
            }
            if (!string.IsNullOrEmpty(apellido) && apellido != cliente.Apellido)
            {
                cliente.Apellido = apellido;
                huboCambio = true;
            }
            if (!string.IsNullOrEmpty(nroDoc) && nroDoc != cliente.NroDoc)
            {
                if (await _clienteRepo.ExisteDocumento(cliente.IdTipoDoc, nroDoc, codCliente))
                {
                    return OperationResult.Conflict("El número de documento ingresado ya pertenece a otro cliente.");
                }
                cliente.NroDoc = nroDoc;
                huboCambio = true;
            }
            if (!string.IsNullOrEmpty(correo) &&
                !string.Equals(correo, cliente.IdUsuarioNavigation.CorreoElectronico, StringComparison.OrdinalIgnoreCase))
            {
                // NUEVO: el correo es único en la BD; sin este control, SaveChanges explotaba con un error 500
                if (await _clienteRepo.ExisteCorreo(correo, cliente.IdUsuario))
                {
                    return OperationResult.Conflict("El correo electrónico ingresado ya pertenece a otro usuario.");
                }
                cliente.IdUsuarioNavigation.CorreoElectronico = correo;
                huboCambio = true;
            }

            if (!huboCambio)
            {
                return OperationResult.Ok("No se detectaron cambios para actualizar.");
            }

            await _clienteRepo.Actualizar(cliente);
            return OperationResult.Ok("Datos del cliente actualizados correctamente.");
        }

        public async Task<OperationResult<IEnumerable<ClienteDto>>> BuscarClientes(string? filtro)
        {
            var clientes = (await _clienteRepo.BuscarClientes(filtro))
                .Select(MapearCliente)
                .ToList();

            // NUEVO: aviso cuando el filtro no coincide con ningún cliente
            if (clientes.Count == 0)
            {
                var mensaje = string.IsNullOrWhiteSpace(filtro)
                    ? "No hay clientes registrados en el sistema."
                    : $"No se encontró ningún cliente que coincida con '{filtro.Trim()}'.";
                return OperationResult<IEnumerable<ClienteDto>>.NotFound(mensaje);
            }

            return OperationResult<IEnumerable<ClienteDto>>.Ok(clientes, $"Se encontraron {clientes.Count} cliente(s).");
        }

        public async Task<ClienteDto?> ObtenerClientePorId(int codCliente)
        {
            var cliente = await _clienteRepo.ObtenerPorId(codCliente);
            return cliente == null ? null : MapearCliente(cliente);
        }

        public async Task<OperationResult<IEnumerable<ReservaClienteDto>>> ObtenerReservasDeCliente(int codCliente)
        {
            // La validación de existencia ahora vive en el servicio (antes estaba en el controller)
            if (!await _clienteRepo.Existe(codCliente))
            {
                return OperationResult<IEnumerable<ReservaClienteDto>>.NotFound("El cliente no existe, por lo tanto no tiene reservas.");
            }

            var reservas = (await _clienteRepo.ObtenerReservasPorClienteId(codCliente))
                .Select(r => new ReservaClienteDto
                {
                    IdReserva = r.IdReserva,
                    FechaReserva = r.Fecha,
                    Pelicula = r.IdFuncionNavigation.IdPeliculaNavigation.Titulo,
                    FechaFuncion = r.IdFuncionNavigation.Fecha,
                    Horario = r.IdFuncionNavigation.IdHorarioNavigation.Horario1.ToString("HH:mm"),
                    NroSala = r.IdFuncionNavigation.IdSalaNavigation.NroSala,
                    EstadoReserva = r.IdEstadoReservaNavigation.Estado,
                    MontoReserva = r.MontoReserva
                })
                .ToList();

            var mensaje = reservas.Count == 0
                ? "El cliente no tiene reservas registradas."
                : $"El cliente tiene {reservas.Count} reserva(s).";

            return OperationResult<IEnumerable<ReservaClienteDto>>.Ok(reservas, mensaje);
        }

        // Mapeo centralizado Cliente -> ClienteDto (antes estaba duplicado y era inconsistente
        // con los nulos: un método usaba ?. y el otro no)
        private static ClienteDto MapearCliente(Cliente c) => new ClienteDto
        {
            CodCliente = c.CodCliente,
            Nombre = c.Nombre,
            Apellido = c.Apellido,
            NroDoc = c.NroDoc,
            TipoDocumento = c.IdTipoDocNavigation?.Tipo ?? "Sin tipo",
            CorreoElectronico = c.IdUsuarioNavigation?.CorreoElectronico ?? "Sin correo"
        };
    }
}
