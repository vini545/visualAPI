using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visualAPI.data;
using visualAPI.DTO;
using visualAPI.Entities;

namespace visualAPI.Controllers
{
    /// <summary>
    /// Gerencia operações relacionadas a pessoas
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PessoaController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todas as pessoas cadastradas
        /// </summary>
        /// <remarks>
        /// Retorna também os dados da conta associada.
        /// </remarks>
        /// <response code="200">Lista de pessoas retornada com sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        /// <summary>
        /// Encontra uma pessoa pelo id
        /// </summary>
        /// <response code="201">Pessoa encontrada com sucesso</response>
        /// <response code="404">pessoa não encontrada</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <summary>
        /// Cadastra uma nova pessoa
        /// </summary>
        /// <response code="201">Pessoa criada com sucesso</response>
        /// <response code="400">Dados inválidos</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// Atualiza parcialmente os dados de uma pessoa
        /// </summary>
        /// <param name="id">ID da pessoa</param>
        /// <remarks>
        /// Apenas os campos enviados serão atualizados.
        /// </remarks>
        /// <response code="204">Atualização realizada com sucesso</response>
        /// <response code="404">Pessoa não encontrada</response>
        [Authorize]
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Remove uma pessoa do sistema
        /// </summary>
        /// Endpoint protegido por autenticação JWT.
        /// <param name="id">ID da pessoa</param>
        /// <response code="200">Pessoa removida com sucesso</response>
        /// <response code="404">Pessoa não encontrada</response>
        /// /// <response code="401">Usuário não autenticado</response>
        [Authorize]
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
