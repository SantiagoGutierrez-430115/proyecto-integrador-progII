using Cine.Data.DTOs;

namespace Cine.Data.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<bool> RegisterAsync(RegisterDto registerDto);
        Task<string?> LoginAsync(LoginDto loginDto);

        // Lanza ConflictoException si el correo o el nombre de usuario ya existen
        Task<AdminDto> RegisterAdminAsync(RegistroAdminDto dto);
        Task<List<AdminDto>> ListarAdminsAsync();
    }
}
