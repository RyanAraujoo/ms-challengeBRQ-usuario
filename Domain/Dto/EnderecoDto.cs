using System.ComponentModel.DataAnnotations;

namespace Domain.Dto
{
    public class EnderecoDto
    {
        [Required(ErrorMessage = "o campo Logradouro deve estar preenchido.")]
        public string? Logradouro { get; set; }

        [Required(ErrorMessage = "o campo Numero deve estar preenchido.")]
        public string? Numero { get; set; }

        [Required(ErrorMessage = "o campo Bairro deve estar preenchido.")]
        public string? Bairro { get; set; }

        [Required(ErrorMessage = "o campo Cidade deve estar preenchido.")]
        public string? Cidade { get; set; }

        [Required(ErrorMessage = "o campo Estado deve estar preenchido.")]
        public string? Estado { get; set; }

        [Required(ErrorMessage = "o campo País deve estar preenchido.")]
        public string? Pais { get; set; }

        [Required(ErrorMessage = "o campo Cep deve estar preenchido.")]
        public string? Cep { get; set; }
    }
}
