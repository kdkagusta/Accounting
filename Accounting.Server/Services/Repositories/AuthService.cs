using System.IdentityModel.Tokens.Jwt;
using Accounting.Server.Helpers;
using Accounting.Server.Models;
using Accounting.Server.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using static Accounting.Server.Models.DTOs.AuthDTOs;

namespace Accounting.Server.Services.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtHandler _jwtHandler;
        private readonly IConfigurationSection _jwtSettings;
        private readonly IConfiguration _configuration;

        public AuthService(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            JwtHandler jwtHandler)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtHandler = jwtHandler;
            _configuration = configuration;
            _jwtSettings = _configuration.GetSection("Jwt");
        }

        public async Task<AuthResponse> Register(RegisterRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            if (!await _roleManager.RoleExistsAsync(request.Role!))
            {
                await _roleManager.CreateAsync(new IdentityRole(request.Role!));
            }

            await _userManager.AddToRoleAsync(user, request.Role!);

            var roles = await _userManager.GetRolesAsync(user);
            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = _jwtHandler.GetClaims(user, roles);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(claims, signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            var refreshToken = _jwtHandler.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(Convert.ToDouble(_jwtSettings["RefreshExpireDays"]));
            await _userManager.UpdateAsync(user);

            return new AuthResponse(token, refreshToken);
        }
        public async Task<AuthResponse> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new Exception("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = _jwtHandler.GetClaims(user, roles);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(claims, signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            var refreshToken = _jwtHandler.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return new AuthResponse(token, refreshToken);
        }
        public async Task<AuthResponse> RefreshToken(RefreshTokenRequest request)
        {
            var principal = _jwtHandler.GetPrincipalFromExpiredToken(request.AccessToken);
            var user = await _userManager.FindByEmailAsync(principal.Identity!.Name!);

            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new Exception("Invalid refresh token");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var signingCredentials = _jwtHandler.GetSigningCredentials();
            var claims = _jwtHandler.GetClaims(user, roles);
            var tokenOptions = _jwtHandler.GenerateTokenOptions(claims, signingCredentials);
            var newToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            var newRefreshToken = _jwtHandler.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(Convert.ToDouble(_jwtSettings["RefreshExpireDays"]));
            await _userManager.UpdateAsync(user);

            return new AuthResponse(newToken, newRefreshToken);
        }
    }
}
