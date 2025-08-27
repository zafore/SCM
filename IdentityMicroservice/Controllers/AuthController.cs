using IdentityMicroservice.Entities;
using IdentityMicroservice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityMicroservice.Controllers
{
    
        [ApiController]
        [Route("api/auth")]
        public class AuthController : ControllerBase
        {
            private readonly IConfiguration _configuration;
            private readonly ILogger<AuthController> _logger;
            private readonly ScmdbContext _ScmdbContext;
            public AuthController(IConfiguration configuration, ILogger<AuthController> logger, ScmdbContext ScmdbContext)
            {
                _configuration = configuration;
                _logger = logger;
                _ScmdbContext= ScmdbContext;
            }

            [HttpPost("register")]
            public async Task<IActionResult> Register([FromBody] RegisterRequest model)
            {
                _logger.LogInformation($"Attempting to register user: {model.UserName}");

                // TODO: In a real application, you would:
                // 1. Hash the password.
                // 2. Save user to a database (e.g., using ASP.NET Core Identity).
                // 3. Assign a default role (e.g., "customer").
                // For simplicity, we'll use a hardcoded user for now.
                if (model.UserName == "test@example.com" && model.Password == "Password123!")
                {
                    return BadRequest("User already exists (hardcoded example).");
                }

                // Simulate saving user (e.g., to a dummy list or file)
                _logger.LogInformation($"User {model.UserName} registered successfully (simulated).");

                return Ok(new { Message = "User registered successfully." });
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login([FromBody] LoginRequest model)
            {
                if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
                {
                    return BadRequest(new { Message = "UserName and Password are required." });
                }

                _logger.LogInformation($"Attempting to log in user: {model.UserName}");

                // TODO: In a real application, you would:
                // 1. Verify user credentials against the database.
                // 2. Retrieve user roles.
                // For simplicity, hardcoded users and roles.
                int userId;
               List<string> userRole ;
            var UserInfo=_ScmdbContext.Users.Include(x=>x.UserRoles).FirstOrDefault(u => u.UserName == model.UserName && u.PassWord == model.Password);
                if (UserInfo !=null)
                {
                userId = UserInfo.UserId;
                    userRole = _ScmdbContext.UserRoles
                        .Where(r => r.UserId==UserInfo.UserId )
                        .Select(r => r.Role.RoleName)
                        .ToList();
                }
    
                else
                {
                    _logger.LogWarning($"Login failed for user: {model.UserName}");
                    return Unauthorized(new { Message = "Invalid credentials." });
                }

                // Generate JWT Token
                var token = GenerateJwtToken(UserInfo.UserName, userRole);
                _logger.LogInformation($"User {model.UserName} logged in successfully. Token generated.");

                return Ok(new AuthResponse { Token = token, ExpiresIn = int.Parse(_configuration["JwtSettings:ExpiresInMinutes"]) });
            }

            private string GenerateJwtToken(string userId,List<string> role)
            {
                var jwtSettings = _configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings["SecretKey"];
                var issuer = jwtSettings["Issuer"];
                var audience = jwtSettings["Audience"];
                var expiresInMinutes = int.Parse(jwtSettings["ExpiresInMinutes"]);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, userId) // Using userId as email for simplicity
          
            };

            foreach (var RoleName in role)
            {

                claims.Add(new Claim(ClaimTypes.Role, RoleName));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(expiresInMinutes),
                    Issuer = issuer,
               
                    Audience = jwtSettings["Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
        }
    }

