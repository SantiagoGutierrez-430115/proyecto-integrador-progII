using Cine.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetUsuarioByEmailAsync(string email);
        Task<Role?> GetRoleByNameAsync(string rolName);
        Task<bool> RegisterUserAndClientAsync(Usuario usuario, Cliente cliente);
    }
}
