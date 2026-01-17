using System.Text.Json.Serialization;

namespace visualAPI.Entities
{
    public class Pessoa
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public Conta Conta { get; set; }
    }
}
