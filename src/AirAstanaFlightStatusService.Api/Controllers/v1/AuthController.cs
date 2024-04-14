using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AirAstanaFlightStatusService.Api.Requests;
using AirAstanaFlightStatusService.Application.Auths.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AirAstanaFlightStatusService.Api.Controllers.v1;

//todo по хорошему надо вынести в отдельный сервис
/// <summary>
/// Предоставляет метод для получения токена
/// </summary>
[Route("api/v{version:apiVersion}/auth")]
[ApiVersion("1.0")]
public class AuthController : BaseController
{
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Метод получения токена по username и password
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var roleCode = await Mediator.Send(new GetRoleCodeByCheckUserCommand(request.UserName, request.Password));
        if (roleCode.IsFailed)
        {
            return BadRequest("Не корректный имя пользователя или пароль");
        }
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("f2a1ed52710d4533bde25be6da03b6e3");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim(ClaimTypes.Role, "Moderator")
            }),
            // Время жизни токена
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        
        return Ok(new { Token = tokenString });
    }
}