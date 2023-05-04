
using System.ComponentModel.DataAnnotations;

namespace Domain.Dto
{
    public class EsquecerSenhaDto
    {
        [Required(ErrorMessage = "O campo HashDeSegurança deve ser preenchido.")]
        public string? HashDeSeguranca { get; set; }

        [Required(ErrorMessage = "O campo NovaSenha deve estar preenchido.")]
        public string? novaSenha { get; set; }
    }
}
