using Cine.Data.Models;
using Cine.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cine.Data.Repositories.Implementations
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly CineDbContext _context;
        public ClienteRepository(CineDbContext context)
        {
            _context = context;
        }

        // Búsqueda de clientes cruzando con usuarios (nombre, apellido, documento o correo).
        // AsNoTracking: es solo lectura, EF no necesita seguir los cambios de estas entidades.
        public async Task<IEnumerable<Cliente>> BuscarClientes(string? filtro)
        {
            var query = _context.Clientes
                .AsNoTracking()
                .Include(c => c.IdUsuarioNavigation)
                .Include(c => c.IdTipoDocNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                filtro = filtro.Trim();
                query = query.Where(c =>
                    c.Nombre.Contains(filtro) ||
                    c.Apellido.Contains(filtro) ||
                    c.NroDoc.Contains(filtro) ||
                    c.IdUsuarioNavigation.CorreoElectronico.Contains(filtro)
                );
            }

            return await query
                .OrderBy(c => c.Apellido)
                .ThenBy(c => c.Nombre)
                .ToListAsync();
        }

        // Obtener cliente por ID (con tracking, porque se usa también para actualizar)
        public async Task<Cliente?> ObtenerPorId(int codCliente)
        {
            return await _context.Clientes
                .Include(c => c.IdUsuarioNavigation)
                .Include(c => c.IdTipoDocNavigation)
                .FirstOrDefaultAsync(c => c.CodCliente == codCliente);
        }

        // Verificación liviana de existencia (no trae el registro completo)
        public async Task<bool> Existe(int codCliente)
        {
            return await _context.Clientes.AnyAsync(c => c.CodCliente == codCliente);
        }

        // Actualizar datos del cliente.
        // El cliente ya viene "trackeado" desde ObtenerPorId, así que alcanza con SaveChanges:
        // EF detecta solo las columnas modificadas (Update() marcaba TODAS como modificadas).
        public async Task Actualizar(Cliente cliente)
        {
            await _context.SaveChangesAsync();
        }

        // Revisar las reservas de funciones del cliente
        public async Task<IEnumerable<Reserva>> ObtenerReservasPorClienteId(int codCliente)
        {
            return await _context.Reservas
                .AsNoTracking()
                .Include(r => r.IdEstadoReservaNavigation)
                .Include(r => r.IdFuncionNavigation)
                    .ThenInclude(f => f.IdPeliculaNavigation)
                .Include(r => r.IdFuncionNavigation)
                    .ThenInclude(f => f.IdHorarioNavigation)
                .Include(r => r.IdFuncionNavigation)
                    .ThenInclude(f => f.IdSalaNavigation)
                .Where(r => r.CodCliente == codCliente)
                .OrderByDescending(r => r.Fecha)
                .ToListAsync();
        }

        // Revisa si el documento ya está registrado en otro cliente.
        // En la BD la restricción única es (id_tipo_doc, nro_doc), así que se comparan ambos.
        public async Task<bool> ExisteDocumento(int idTipoDoc, string nroDoc, int codClienteExcluido)
        {
            return await _context.Clientes
                .AnyAsync(c => c.IdTipoDoc == idTipoDoc
                            && c.NroDoc == nroDoc
                            && c.CodCliente != codClienteExcluido);
        }

        // Revisa si el correo ya lo usa otro usuario (en la BD hay un índice único sobre correo_electronico)
        public async Task<bool> ExisteCorreo(string correo, int idUsuarioExcluido)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.CorreoElectronico == correo && u.IdUsuario != idUsuarioExcluido);
        }
    }
}
