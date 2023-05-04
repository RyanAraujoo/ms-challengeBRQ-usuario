using Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class PatchUsuarioDto
    {
        public string? Email { get; set; }
        public string? DataDeNascimento { get; set; }
        public string? Sexo { get; set; }
        public string? NomeCompleto { get; set; }
        public string? Senha { get; set; }
        public string? Apelido { get; set; }
        public string? Telefone { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public PatchEnderecoDto? Endereco { get; set; }
    }
}
