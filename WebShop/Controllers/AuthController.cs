using Microsoft.AspNetCore.Mvc;
using WebShop.Dtos;
using WebShop.Services;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

            public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var ok = await _service.RegisterAsync(dto);
            if (!ok)
                return Conflict("Korisnik s tim emailom vec postoji.");

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _service.LoginAsync(dto);
            if (token == null)
                return Unauthorized("Neispravan email ili lozinka.");

            return Ok(new { token });
        }
    }
}
