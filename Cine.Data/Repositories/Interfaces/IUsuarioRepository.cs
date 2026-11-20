using Cine.Data.Models;

namespace Cine.Data.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetUsuarioByEmailAsync(string email);
        Task<Role?> GetRoleByNameAsync(string rolName);
        Task<bool> RegisterUserAndClientAsync(Usuario usuario, Cliente cliente);

        // Administradores: usuarios sin cliente asociado
        Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario);
        Task CrearUsuarioAsync(Usuario usuario);
        Task<List<Usuario>> ListarPorRolAsync(string rolName);
    }
}
