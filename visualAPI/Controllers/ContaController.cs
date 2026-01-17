using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visualAPI.data;
using visualAPI.Entities;

namespace visualAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ContaController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ContaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Conta>> Get()
        {
            var contas = _context.Contas.ToList();
            if (contas is null)
            {
                return NotFound();
            }
            return contas;
        }

        [HttpGet("{id:guid}/saldo")]
        public async Task<IActionResult> GetSaldo(Guid id)
        {
            var result = await _context.Pessoas
                .Include(p => p.Conta)
                .Where(p => p.Id == id)
                .Select(p => new PessoaSaldoDTO
                {
                    PessoaId = p.Id,
                    Nome = p.Nome,
                    ContaId = p.Conta.Id,
                    Saldo = p.Conta.Saldo
                })
                .FirstOrDefaultAsync();

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("{id:guid}/credito")]
        public async Task<IActionResult> Credito(Guid id, ContaOperacaoDTO dto)
        {
            if (dto.Valor <= 0)
                return BadRequest("Valor inválido");

            var conta = await _context.Contas.FindAsync(id);
            if (conta == null)
                return NotFound();

            conta.Saldo += dto.Valor;
            await _context.SaveChangesAsync();

            return Ok(conta.Saldo);
        }

        [HttpPost("{id:guid}/debito")]
        public async Task<IActionResult> Debito(Guid id, ContaOperacaoDTO dto)
        {
            if (dto.Valor <= 0)
                return BadRequest("Valor inválido");

            var conta = await _context.Contas.FindAsync(id);
            if (conta == null)
                return NotFound();

            if (conta.Saldo < dto.Valor)
                return BadRequest("Saldo insuficiente");

            conta.Saldo -= dto.Valor;
            await _context.SaveChangesAsync();

            return Ok(conta.Saldo);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var conta = await _context.Contas
                .Include(c => c.Pessoa)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (conta == null)
                return NotFound();

            //if (conta.Pessoa != null)  // assim quando a tem pessoa nao da pra deletar a conta, porem quando deleta a pessoa deleta a conta :/ 
            //    return BadRequest("Conta vinculada a uma pessoa");

            _context.Contas.Remove(conta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
