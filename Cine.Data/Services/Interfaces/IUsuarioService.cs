using Cine.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Data.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<bool> RegisterAsync(RegisterDto registerDto);
        Task<string?> LoginAsync(LoginDto loginDto);
    }
}
