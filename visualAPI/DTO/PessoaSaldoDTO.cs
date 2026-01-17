public class PessoaSaldoDTO
{
    public Guid PessoaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public Guid ContaId { get; set; }
    public decimal Saldo { get; set; }
}

