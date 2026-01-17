using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visualAPI.data;

namespace visualAPI.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _context;

    public HealthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("db")]
    public async Task<IActionResult> CheckDb()
    {
        try
        {
            await _context.Database.OpenConnectionAsync();
            await _context.Database.CloseConnectionAsync();
            return Ok("Conexão com PostgreSQL OK");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}

