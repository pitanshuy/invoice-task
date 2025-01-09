using invoice_task.Domain.Entities;
using invoice_task.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using invoice_task.DTO;

namespace invoice_task.Service
{
  
        public class AuthenticationService : IAuthenticationService
        {
            private readonly IConfiguration _configuration;
            private readonly IUserRepository _userRepository; // A repository to fetch user data from the database
        private readonly IPasswordHasher<User> _passwordHasher;
        public AuthenticationService(IConfiguration configuration, IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
            {
                _configuration = configuration;
                _userRepository = userRepository;
                _passwordHasher = passwordHasher;
            }

    

        public async Task<string> AuthenticateUser(LoginRequestDto loginRequest)
        {
            var user = await _userRepository.GetByEmailAsync(loginRequest.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // Verify the hashed password
            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            return GenerateJwtToken(user);
        }


        private string GenerateJwtToken(User user)
            {
                var claims = new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }


    }

