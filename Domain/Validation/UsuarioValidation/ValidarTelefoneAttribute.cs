using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace Domain.Validation.UsuarioValidation
{
    public class ValidarTelefoneAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (!ValidarTelefonePorExpressãoRegular(value.ToString()))
            {
                return new ValidationResult("o campo TELEFONE necessita está no modelo '(xx)9xxxx-xxxx'");
            }

            return string.IsNullOrWhiteSpace(value.ToString()) ? new ValidationResult("O campo Telefone está em branco/vazio. Tente novamente!") : ValidationResult.Success;
        }
        private bool ValidarTelefonePorExpressãoRegular(string telefone)
        {
            Regex regex = new Regex(@"^\(?[1-9]{2}\)?9[1-9]{4}\-?[1-9]{4}$");
            return regex.IsMatch(telefone);
        }
    }
}
