using System.ComponentModel.DataAnnotations.Schema;

namespace visualAPI.Entities
{
    public class Conta
    {
        public Guid Id { get; set; }
        public decimal Saldo { get; set; }
        [ForeignKey("Pessoa")]
        public Guid PessoaId { get; set; }
        public Pessoa Pessoa { get; set; }
    }
}