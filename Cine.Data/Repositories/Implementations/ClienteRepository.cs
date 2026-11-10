using Cine.Data.Models;
using Cine.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Repositories.Implementations
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly CineDbContext _context;
        public ClienteRepository(CineDbContext context)
        {
            _context = context;
        }

        // Búsqueda de clientes cruzando con usuarios 
        public async Task<IEnumerable<Cliente>> BuscarClientes(string filtro)
        {
            var query = _context.Clientes
                .Include(c => c.IdUsuarioNavigation)
                .Include(c => c.IdTipoDocNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                filtro = filtro.ToLower();
                query = query.Where(c =>
                    c.Nombre.ToLower().Contains(filtro) ||
                    c.Apellido.ToLower().Contains(filtro) ||
                    c.NroDoc.Contains(filtro) ||
                    c.IdUsuarioNavigation.CorreoElectronico.ToLower().Contains(filtro) // Columna creada en tu script de usuarios[cite: 6]
                );
            }

            return await query.ToListAsync();
        }

        // Obtener cliente por ID 
        public async Task<Cliente?> ObtenerPorId(int? codCliente)
        {
            return await _context.Clientes
                .Include(c => c.IdUsuarioNavigation)
                .Include(c => c.IdTipoDocNavigation)     // Por si usás el tipo de documento en el DTO
                .Include(c => c.IdDireccionNavigation)   // Por si usás la dirección en el DTO
                .FirstOrDefaultAsync(c => c.CodCliente == codCliente);
        }

        //  Actualizar datos del cliente
        public async Task Actualizar(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        // Revisar las reservas de funciones del cliente
        public async Task<IEnumerable<Reserva>> ObtenerReservasPorClienteId(int codCliente)
        {
            return await _context.Reservas
                .Include(r => r.IdEstadoReservaNavigation)
                .Include(r => r.IdFuncionNavigation)
                    .ThenInclude(f => f.IdPeliculaNavigation)
                .Include(r => r.IdFuncionNavigation)
                    .ThenInclude(f => f.IdHorarioNavigation)
                .Include(r => r.IdFuncionNavigation)
                    .ThenInclude(f => f.IdSalaNavigation)
                .Include(r => r.ReservaDetalles)
                    .ThenInclude(rd => rd.IdButacaNavigation)
                .Where(r => r.CodCliente == codCliente)
                .OrderByDescending(r => r.Fecha)
                .ToListAsync();
        }
        // Revisar si el DNI ya esta registrado en otra cuenta
        public async Task<bool> ExisteDni(string nroDoc, int codClienteExcluido)
        {
            return await _context.Clientes
                .AnyAsync(c => c.NroDoc == nroDoc && c.CodCliente != codClienteExcluido);
        }
    }
}
