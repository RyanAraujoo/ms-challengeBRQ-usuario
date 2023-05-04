
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.Validation.UsuarioValidation
{
    public class ValidarEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (!ValidarEmailPorExpressãoRegular(value.ToString()))
            {
                return new ValidationResult("o campo Email é inválido.");
            }

            return string.IsNullOrWhiteSpace(value.ToString()) ? new ValidationResult("O campo EMAIL está em branco/vazio. Tente novamente!") : ValidationResult.Success;
        }
        private bool ValidarEmailPorExpressãoRegular(string email)
        {
            Regex regex = new Regex(@"^[a-z0-9.]+@[a-z0-9]+\.[a-z]+(\.([a-z]+))?$");
            return regex.IsMatch(email);
        }
    }
}
