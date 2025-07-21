using EmployeeManagementService_Backend.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmployeeManagementService_Backend.Service;

namespace EmployeeManagementService_Backend.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IConfiguration configuration, IJwtService jwtService, ILogger<AuthController> logger, IAuthenticationService authenticationService)
    {
        _configuration = configuration;
        _jwtService = jwtService;
        _logger = logger;
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User login)
    {
        try
        {
            var user = await _authenticationService.ValidateUser(login.Username, login.PasswordHash);
            if (user == null)
                return Unauthorized();

            var token = _jwtService.GenerateJwtToken(user);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing your request.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] User newUser)
    {
        try
        {
            var registered = await _authenticationService.RegisterUser(newUser.Username, newUser.PasswordHash, newUser.Role);
            if (!registered)
                return Conflict("Username already exists.");
            return Ok(new { message = "User registered successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing your request.");
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
} 