
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    public class Endereco
    {
        [Key]
        public Guid Id { get; set; }
        public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? Pais { get; set; }
        public string? Cep { get; set; }
    }
}
