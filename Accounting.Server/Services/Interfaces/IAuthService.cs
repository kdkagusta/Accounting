using static Accounting.Server.Models.DTOs.AuthDTOs;

namespace Accounting.Server.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> Register(RegisterRequest request);
        Task<AuthResponse> Login(LoginRequest request);
        Task<AuthResponse> RefreshToken(RefreshTokenRequest request);
    }
}
