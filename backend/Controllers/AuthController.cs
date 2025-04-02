using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]//api/auth....
    public class AuthController : ControllerBase
    {   
        private readonly UsersService _usersService;//need init in program
        private readonly IConfiguration _configuration;

        public AuthController(UsersService usersService, IConfiguration configuration)
        {
            _usersService = usersService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var existingUser = await _usersService.GetUserByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return BadRequest("User with this email already exists.");
            }
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var newUser = new User
            {
                Email = dto.Email,
                PasswordHash = passwordHash,
                FullName = dto.FullName
            };
            await _usersService.CreateUserAsync(newUser);

            return Ok("User registered successfully!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var user = await _usersService.ValidateUserPasswordAsync(dto.Email, dto.Password);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
          
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _usersService.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new 
            { 
                user.Id, 
                user.Email, 
                user.FullName 
            });
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var jwtIssuer = _configuration["Jwt:Issuer"];
            var jwtAudience = _configuration["Jwt:Audience"];

            // מכינים מפתח החתימה
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id), 
                new Claim("email", user.Email),
                new Claim("fullName", user.FullName)
            };

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(9),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
