using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Domain.Validation
{
    public class ValidarDataDeNascimentoAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value,
                                                     ValidationContext validationContext)
        {
            DateTime dat;
            if (DateTime.TryParse(value.ToString(), out dat))
            {
                if (dat >= DateTime.Now)
                {
                    return new ValidationResult("A DataDeNascimento precisa ser inferior a data atual");
                }
            }

            if (!ValidarPadraoDataDeNascimento(value.ToString()))
            {
                return new ValidationResult("A DataDeNascimento precisa ser no modelo 'yyyy-MM-dd'");
            }

            return ValidationResult.Success;
        }

        private bool ValidarPadraoDataDeNascimento(string data)
        {
            Regex regex = new Regex(@"^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$");
            return regex.IsMatch(data);
        }
    }
}
