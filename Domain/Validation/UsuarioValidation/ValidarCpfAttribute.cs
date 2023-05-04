using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.Validation.UsuarioValidation
{
    public class ValidarCpfAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (!ValidarCpfPorExpressãoRegular(value.ToString()))
            {
                return new ValidationResult("O campo CPF inválido.");
            }

            return string.IsNullOrWhiteSpace(value.ToString()) ? new ValidationResult("O campo CPF está em branco/vazio. Tente novamente!") : ValidationResult.Success;

        }

        private bool ValidarCpfPorExpressãoRegular(string cpf)
        {
            Regex regex = new Regex(@"^[0-9]{3}\.?[0-9]{3}\.?[0-9]{3}\-?[0-9]{2}$");
            return regex.IsMatch(cpf);
        }
    }
}
