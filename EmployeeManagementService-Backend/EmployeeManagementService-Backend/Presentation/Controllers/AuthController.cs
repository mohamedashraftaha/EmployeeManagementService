using EmployeeManagementService_Backend.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagementService_Backend.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    // For demo: static user list
    private static readonly List<User> Users = new()
    {
        new User { Username = "admin", Password = "password" },
        new User { Username = "user", Password = "password" }
    };

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User login)
    {
        var user = Users.FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);
        if (user == null)
            return Unauthorized();

        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] User newUser)
    {
        if (Users.Any(u => u.Username == newUser.Username))
            return Conflict("Username already exists.");
        if (string.IsNullOrWhiteSpace(newUser.Role))
            newUser.Role = "User";
        Users.Add(newUser);
        return Ok(new { message = "User registered successfully." });
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiryMinutes"]!)),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
} 