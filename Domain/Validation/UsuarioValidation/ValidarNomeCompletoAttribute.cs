using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.Validation.UsuarioValidation
{
    public class ValidarNomeCompletoAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (ValidarNomePorExpressãoRegular(value.ToString()))
            {
                return new ValidationResult("Campo NOME com atribuição repetitiva não esperada.");
            }

            return string.IsNullOrWhiteSpace(value.ToString()) ? new ValidationResult("O campo NomeCompleto está em branco. Tente novamente!") : ValidationResult.Success;
        }

        private bool ValidarNomePorExpressãoRegular(string nome)
        {
            Regex regex = new Regex(@"^([a-zA-Z])\1+$");
            return regex.IsMatch(nome);
        }
    }
}

