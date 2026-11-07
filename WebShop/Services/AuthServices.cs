using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Dtos;
using WebShop.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity.Data;

namespace WebShop.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDto dto);
        Task<string?> LoginAsync(LoginDto dto);
    }

    public class  AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<Customer> _hasher = new();

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var email = dto.Email.Trim().ToLower();

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null)
                return null;

            var result = _hasher.VerifyHashedPassword(customer, customer.PasswordHash, dto.Password);
            if(result == PasswordVerificationResult.Failed)
                return null;

            return CreateToken(customer);
        }

        private string CreateToken(Customer customer)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
                new Claim(ClaimTypes.Email, customer.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            var email = dto.Email.Trim().ToLower();

            var exist = await _context.Customers.AnyAsync(c => c.Email == email);
            if (exist)
                return false;

            var customer = new Customer
            {
                 Name = dto.Name,
                 Email = email
            };
            customer.PasswordHash = _hasher.HashPassword(customer, dto.Password);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
