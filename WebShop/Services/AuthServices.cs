using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Dtos;
using WebShop.Models;   

namespace WebShop.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDto dto);
    }

    public class  AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<Customer> _hasher = new();

        public AuthService(AppDbContext context)
        {
            _context = context;
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
