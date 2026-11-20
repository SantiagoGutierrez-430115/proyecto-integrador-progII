using Cine.Data.DTOs;

namespace Cine.Data.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<bool> RegisterAsync(RegisterDto registerDto);
        Task<string?> LoginAsync(LoginDto loginDto);
        Task<AdminDto> RegisterAdminAsync(RegistroAdminDto dto);
        Task<List<AdminDto>> ListarAdminsAsync();
    }
}
