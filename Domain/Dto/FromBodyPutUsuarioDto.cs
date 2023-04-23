using Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class FromBodyPutUsuarioDto
    {
        public string Email { get; set; }

        [ValidarDataDeNascimento]
        public string DataDeNascimento { get; set; }

        [ValidarSexo]
        public string Sexo { get; set; }
        public string NomeCompleto { get; set; }
        public string Senha { get; set; }
        public string? Apelido { get; set; }
        public string Telefone { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public FromBodyPatchEnderecoDto Endereco { get; set; }
    }
}
