using Accounting.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Accounting.Server.Models.DTOs.AuthDTOs;

namespace Accounting.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try 
            {
                var response = await _authService.Register(request);
                return ResponseOk(response);
            }
            catch (Exception ex)
            {
                return ResponseError(ex.Message, new List<string> { ex.Message });
            }
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try 
            {
                var response = await _authService.Login(request);
                return ResponseOk(response);
            }
            catch (Exception ex)
            {
                return ResponseError(ex.Message, new List<string> { ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var response = await _authService.RefreshToken(request);
                return ResponseOk(response);
            }
            catch (Exception ex)
            {
                return ResponseError(ex.Message, new List<string> { ex.Message });
            }
        }
    }
}
