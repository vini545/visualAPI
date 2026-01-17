namespace visualAPI.Entities
{
    public class Pessoa
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public Guid ContaId { get; set; }
        public Conta Conta { get; set; }
    }
}
