using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using visualAPI.data;
using visualAPI.DTO;
using visualAPI.Entities;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly AppDbContext _context;

    /// <summary>
    /// Controller responsável pela autenticação e geração de tokens JWT.
    /// </summary>
    /// <param name="config">Configurações da aplicação (JWT)</param>
    public AuthController(IConfiguration config, AppDbContext context)
    {
        _config = config;
        _context = context;
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
    public async Task<IActionResult> Login(UserDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Password))
            return Unauthorized("Nome ou senha inválidos");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == dto.Name);
        if (user == null)
            return Unauthorized("Usuário não encontrado");

        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(user, user.Password, dto.Password);

        if (result == PasswordVerificationResult.Failed)
            return Unauthorized("Senha incorreta");

        var claims = new[]
        {
        new Claim(ClaimTypes.Name, user.UserName),
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


    /// <summary>
    /// Cria um usuário novo para autenticação.
    /// </summary>
    /// <remarks>
    /// Este endpoint cria um usuário com claims básicas (nome e role "User") e retorna um token JWT válido.
    /// Exemplo de body:
    /// {
    ///   "name": "Vinicius",
    ///   "password": "SenhaSegura123"
    /// }
    /// </remarks>
    /// <param name="dto">Dados do usuário (nome e senha)</param>
    /// <returns>Token JWT que pode ser usado para autenticação</returns>
    /// <response code="200">Token JWT gerado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost("CreateUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(UserDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest("Nome e senha são obrigatórios");

        if (await _context.Users.AnyAsync(u => u.UserName == dto.Name))
            return BadRequest("Usuário já existe");

        var hasher = new PasswordHasher<User>();
        var user = new User { UserName = dto.Name };
        user.Password = hasher.HashPassword(user, dto.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, "User")
        };


        return Ok($"Nome: {user.UserName} criado com sucesso");
    }

}
