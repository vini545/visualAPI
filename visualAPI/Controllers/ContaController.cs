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

        /// <summary>
        /// Retorna todas as contas cadastradas.
        /// </summary>
        /// <returns>Lista de contas</returns>
        /// <response code="200">Lista retornada com sucesso</response>
        /// <response code="404">Nenhuma conta encontrada</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Conta>> Get()
        {
            var contas = _context.Contas.ToList();
            if (contas is null)
            {
                return NotFound();
            }
            return contas;
        }

        /// <summary>
        /// Retorna o saldo de uma pessoa pelo seu ID.
        /// </summary>
        /// <param name="id">ID da pessoa</param>
        /// <returns>Informações da pessoa e saldo da conta</returns>
        /// <response code="200">Saldo encontrado com sucesso</response>
        /// <response code="404">Pessoa não encontrada</response>
        [HttpGet("{id:guid}/saldo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Credita um valor em uma conta pelo ID da conta.
        /// </summary>
        /// <param name="id">ID da conta</param>
        /// <param name="dto">Objeto contendo o valor a ser creditado</param>
        /// <returns>Saldo atualizado</returns>
        /// <response code="200">Crédito realizado com sucesso</response>
        /// <response code="400">Valor inválido</response>
        /// <response code="404">Conta não encontrada</response>
        [HttpPost("{id:guid}/credito")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Debita um valor de uma conta pelo ID da conta.
        /// </summary>
        /// <param name="id">ID da conta</param>
        /// <param name="dto">Objeto contendo o valor a ser debitado</param>
        /// <returns>Saldo atualizado</returns>
        /// <response code="200">Débito realizado com sucesso</response>
        /// <response code="400">Valor inválido ou saldo insuficiente</response>
        /// <response code="404">Conta não encontrada</response>
        [HttpPost("{id:guid}/debito")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// Deleta uma conta pelo seu ID.
        /// </summary>
        /// <param name="id">ID da conta</param>
        /// <returns>NoContent se deletado com sucesso</returns>
        /// <response code="204">Conta deletada com sucesso</response>
        /// <response code="404">Conta não encontrada</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
