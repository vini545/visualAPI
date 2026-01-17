using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    /// <summary>
    /// Controller responsável pela autenticação e geração de tokens JWT.
    /// </summary>
    /// <param name="config">Configurações da aplicação (JWT)</param>

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Realiza o login do usuário e gera um token JWT.
    /// </summary>
    /// <remarks>
    /// Este endpoint gera um token JWT contendo as claims básicas do usuário,
    /// que deverá ser utilizado no header Authorization como:
    /// Bearer {token}
    /// </remarks>
    /// <response code="200">Token JWT gerado com sucesso</response>
    /// <response code="401">Credenciais inválidas</response>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Vinicius"),
            new Claim(ClaimTypes.Role, "User")
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        );

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }
}
