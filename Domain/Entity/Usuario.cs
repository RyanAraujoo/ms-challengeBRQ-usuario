using Domain.Enum;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entity
{
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; }

        public Guid EnderecoId { get; set; }
        public DateTime DataDeNascimento { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string NomeCompleto { get; set; }
        public string Senha { get; set; }
        public string Apelido { get; set; }
        public string Telefone { get; set; }
        public DateTime DataCadastro { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? DataAtualizacao { get; set; }
        public int Sexo { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Endereco Endereco { get; set; }

    }
}
