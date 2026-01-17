public class PessoaDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public ContaDTO? Conta { get; set; }
}
