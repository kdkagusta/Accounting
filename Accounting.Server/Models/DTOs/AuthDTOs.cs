namespace Accounting.Server.Models.DTOs
{
    public class AuthDTOs
    {
        public record LoginRequest(string Email, string Password);
        public record RegisterRequest(string Email, string Password, string ConfirmPassword, string? Role = "User");
        public record RefreshTokenRequest(string AccessToken, string RefreshToken);
        public record AuthResponse(string Token, string RefreshToken);
    }
}
