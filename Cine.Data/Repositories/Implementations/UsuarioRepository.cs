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
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly CineDbContext _context;

        public UsuarioRepository(CineDbContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetRoleByNameAsync(string rolName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Descripcion == rolName);
        }

        public async Task<Usuario?> GetUsuarioByEmailAsync(string email)
        {
            return await _context.Usuarios.Include(x => x.Rol).FirstOrDefaultAsync(x => x.CorreoElectronico == email);
        }

        public async Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario)
        {
            // Un trigger de la BD tampoco permite que el nombre de usuario coincida con el correo de otra cuenta
            return await _context.Usuarios.AnyAsync(u =>
                u.NombreUsuario == nombreUsuario || u.CorreoElectronico == nombreUsuario);
        }

        public async Task CrearUsuarioAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Usuario>> ListarPorRolAsync(string rolName)
        {
            return await _context.Usuarios
                .AsNoTracking()
                .Where(u => u.Rol.Descripcion == rolName)
                .OrderBy(u => u.NombreUsuario)
                .ToListAsync();
        }

        public async Task<bool> RegisterUserAndClientAsync(Usuario usuario, Cliente cliente)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Guardar el usuario en la base de datos
                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();

                // Mapear el IdUsuario generado al cliente con su fecha de registro en el día de hoy y guardar el cliente en la base de datos
                cliente.IdUsuario = usuario.IdUsuario;
                cliente.FechaRegistro = DateOnly.FromDateTime(DateTime.Now);

                await _context.Clientes.AddAsync(cliente);
                await _context.SaveChangesAsync();

                // Confirmar la transacción si salió todo bien
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                // Revertir la transacción si hubo algún error
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
