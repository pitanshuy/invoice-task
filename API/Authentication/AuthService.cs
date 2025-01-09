using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using invoice_task.Domain.Entities;
using Microsoft.AspNetCore.Identity;



namespace invoice_task.API.Authentication
{

    //    public class AuthService
    //    {
    //        private readonly IConfiguration _configuration;


    //        public AuthService(IConfiguration configuration)
    //        {
    //            _configuration = configuration;
    //        }

    //        public string GenerateJwtToken(string username, string role)
    //        {
    //            var claims = new List<Claim>
    //        {
    //            new Claim(ClaimTypes.Name, username),
    //            new Claim(ClaimTypes.Role, role)
    //        };

    //            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
    //            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //            var token = new JwtSecurityToken(
    //                issuer: _configuration["Jwt:Issuer"],
    //                audience: _configuration["Jwt:Audience"],
    //                claims: claims,
    //                expires: DateTime.UtcNow.AddHours(1),
    //                signingCredentials: creds);

    //            return new JwtSecurityTokenHandler().WriteToken(token);
    //        }
    //}




        public class AuthService 
    {
            private readonly IConfiguration _configuration;
            private readonly IPasswordHasher<User> _passwordHasher;

            public AuthService(IConfiguration configuration, IPasswordHasher<User> passwordHasher)
            {
                _configuration = configuration;
                _passwordHasher = passwordHasher;
            }

            // Your method to generate JWT tokens or handle login
            public string GenerateJwtToken(string username, string role)
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }

            // Hashing passwords using IPasswordHasher
            public string HashPassword(string password)
            {
                User user = new User(); // Create a new user instance
                return _passwordHasher.HashPassword(user, password); // Hash the password
            }

            // Verifying password
            public bool VerifyPassword(string hashedPassword, string password)
            {
                User user = new User(); // Create a new user instance
                var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, password);
                return result == PasswordVerificationResult.Success;
            }
        }


    }


//}


