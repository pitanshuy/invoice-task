using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using invoice_task.API.Authentication;
using invoice_task.DTO;
using invoice_task.Interfaces;
using invoice_task.Repositories;
using System.Text;
using System;

namespace invoice_task.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthService _authService;

        public AuthController(IUserRepository userRepository, AuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            var user = _userRepository.GetUserByUsername(request.Username);
            //if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            //    return Unauthorized("Invalid username or password.");

            // If user is not found or password does not match directly
            if (user == null || user.PasswordHash != request.Password)
                return Unauthorized("Invalid username or password.");

            var token = _authService.GenerateJwtToken(user.Username, user.Role);
            return Ok(new { Token = token });

            //var token = _authService.GenerateJwtToken(user.Username, user.Role);
            //return Ok(new { Token = token });
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedInput = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return hashedInput == hashedPassword;
        }
    }


}

