using System.ComponentModel.DataAnnotations;

namespace Domain.Dto
{
    public class TrocarSenhaDto
    {
        [Required(ErrorMessage ="A senha atual deve ser passada!")]
        [MaxLength(100, ErrorMessage = "A SENHA deve ter, no máximo, 100 caracteres.")]
        public string? SenhaAtual { get; set; }

        [Required(ErrorMessage = "A senha nova deve ser passada!")]
        [MaxLength(100, ErrorMessage = "A SENHA deve ter, no máximo, 100 caracteres.")]
        public string? SenhaNova { get; set; }
    }
}
