using Cine.Data.DTOs;
using Cine.Data.Repositories.Interfaces;
using Cine.Data.Respuesta;
using Cine.Data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Services.Implementations
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _ClienteRepo;
        public ClienteService(IClienteRepository clienteRepo)
        {
            _ClienteRepo = clienteRepo;
        }
        public async Task<OperationResult> ActualizarCliente(int codCliente, ActualizarClienteDto dto)
        {
            var cliente = await _ClienteRepo.ObtenerPorId(codCliente);
            if (cliente == null)
            {
                return OperationResult.Fail("El cliente especificado no existe en el sistema.");
            }

            bool huboCambio = false;

            if (!string.IsNullOrEmpty(dto.Nombre) && dto.Nombre != cliente.Nombre)
            {
                cliente.Nombre = dto.Nombre;
                huboCambio = true;
            }
            if (!string.IsNullOrEmpty(dto.Apellido) && dto.Apellido != cliente.Apellido)
            {
                cliente.Apellido = dto.Apellido;
                huboCambio = true;
            }
            if (!string.IsNullOrEmpty(dto.NroDoc) && dto.NroDoc != cliente.NroDoc)
            {
                bool dniOcupado = await _ClienteRepo.ExisteDni(dto.NroDoc, codCliente);
                if (dniOcupado)
                {
                    return OperationResult.Fail("El número de documento ingresado ya pertenece a otro cliente.");
                }
                cliente.NroDoc = dto.NroDoc;
                huboCambio = true;
            }
            if (cliente.IdUsuarioNavigation != null &&
                !string.IsNullOrEmpty(dto.CorreoElectronico) &&
                dto.CorreoElectronico != cliente.IdUsuarioNavigation.CorreoElectronico)
            {
                cliente.IdUsuarioNavigation.CorreoElectronico = dto.CorreoElectronico;
                huboCambio = true;
            }
            if (!huboCambio)
            {
                return OperationResult.Ok("No se detectaron cambios para actualizar.");
            }

            await _ClienteRepo.Actualizar(cliente);
            return OperationResult.Ok("Datos del cliente actualizados correctamente.");
        }

        public async Task<IEnumerable<ClienteDto>> BuscarClientes(string filtro)
        {
            var clientes = await _ClienteRepo.BuscarClientes(filtro);

            return clientes.Select(c => new ClienteDto
            {
                CodCliente = c.CodCliente,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                NroDoc = c.NroDoc,
                TipoDocumento = c.IdTipoDocNavigation.Tipo,
                CorreoElectronico = c.IdUsuarioNavigation.CorreoElectronico,
            });
        }

        public async Task<ClienteDto> ObtenerClientePorId(int codCliente)
        {
            var c = await _ClienteRepo.ObtenerPorId(codCliente);
            if (c == null)
            {
                return null;
            }
            return new ClienteDto
            {
                CodCliente = c.CodCliente,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                NroDoc = c.NroDoc,
                TipoDocumento = c.IdTipoDocNavigation.Tipo,
                CorreoElectronico = c.IdUsuarioNavigation.CorreoElectronico
            };
        }

        public async Task<IEnumerable<ReservaClienteDto>> ObtenerReservasDeCliente(int codCliente)
        {
            var reservas = await _ClienteRepo.ObtenerReservasPorClienteId(codCliente);

            return reservas.Select(r => new ReservaClienteDto
            {
                IdReserva = r.IdReserva,
                FechaReserva = r.Fecha,
                Pelicula = r.IdFuncionNavigation.IdPeliculaNavigation.Titulo,
                Horario = r.IdFuncionNavigation.IdHorarioNavigation.Horario1.ToString(),
                NroSala = r.IdFuncionNavigation?.IdSalaNavigation?.NroSala ?? 0,
                EstadoReserva = r.IdEstadoReservaNavigation.Estado,
                MontoReserva = r.MontoReserva
            });
        }
    }
}
