using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visualAPI.data;
using visualAPI.DTO;
using visualAPI.Entities;

namespace visualAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PessoaController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var pessoas = await _context.Pessoas
                .Include(p => p.Conta)
                .Select(p => new PessoaDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Conta = p.Conta == null ? null : new ContaDTO
                    {
                        Id = p.Conta.Id,
                        Saldo = p.Conta.Saldo
                    }
                })
                .ToListAsync();

            return Ok(pessoas);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var pessoa = await _context.Pessoas
                .Include(p => p.Conta)
                .Select(p => new PessoaDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Conta = p.Conta == null ? null : new ContaDTO
                    {
                        Id = p.Conta.Id,
                        Saldo = p.Conta.Saldo
                    }
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pessoa == null)
                return NotFound();

            return Ok(pessoa);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(PessoaCreateDTO dto)
        {
            var pessoa = new Pessoa
            {
                Id = Guid.NewGuid(),
                Nome = dto.Nome,
                Conta = new Conta
                {
                    Id = Guid.NewGuid(),
                    Saldo = dto.SaldoInicial
                }
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = pessoa.Id }, pessoa.Id);
        }

        [Authorize]
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Patch(Guid id, PessoaPatchDTO dto)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null)
                return NotFound();

            if (dto.Nome != null)
                pessoa.Nome = dto.Nome;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        [Authorize]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);

            if (pessoa == null)
                return NotFound("Pessoa não localizada");

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
