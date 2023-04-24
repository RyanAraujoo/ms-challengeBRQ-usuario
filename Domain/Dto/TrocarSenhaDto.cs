using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dto
{
    public class TrocarSenhaDto
    {
        [Required(ErrorMessage ="A senha atual deve ser passada!")]
        public string? SenhaAtual { get; set; }

        [Required(ErrorMessage = "A senha nova deve ser passada!")]
        public string? SenhaNova { get; set; }
    }
}
