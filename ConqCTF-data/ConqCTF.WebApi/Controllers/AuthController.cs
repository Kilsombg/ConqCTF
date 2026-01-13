using ConqCTF.Application.Auth.Commands.LoginUser;
using ConqCTF.Application.Auth.Commands.LogoutUser;
using ConqCTF.Application.Auth.Commands.RefreshToken;
using ConqCTF.Application.Auth.Commands.RegisterUser;
using ConqCTF.WebApi.Models.Auth.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConqCTF.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var (result, accessToken, refreshToken) = await _sender.Send(new LoginUserCommand()
            {
                Email = request.Email,
                Password = request.Password
            });

            return result.Succeeded
                ? Ok(new { accessToken, refreshToken })
                : Unauthorized(result.Errors);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh(RefreshRequest request)
        {
            var (result, accessToken) = await _sender.Send(new RefreshTokenCommand()
            {
                RefreshToken = request.RefreshToken
            });

            return result.Succeeded
                ? Ok(new { accessToken })
                : Unauthorized(result.Errors);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _sender.Send(new RegisterUserCommand()
            {
                Email = request.Email,
                Password = request.Password
            });

            return result.Succeeded
                ? Ok()
                : BadRequest(result.Errors);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutRequest request)
        {
            var result = await _sender.Send(new LogoutUserCommand()
            {
                RefreshToken = request.RefreshToken
            });

            return Ok();
        }
    }
}
