using System.ComponentModel.DataAnnotations;

namespace Domain.Validation.EnderecoValidation
{
    public class ValidarPaisAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value.ToString().ToUpper() != "BR")
            {
                return new ValidationResult("O campo PAIS, neste momento, só aceita BR. Tente novamente!");
            }

            return string.IsNullOrWhiteSpace(value.ToString()) ? new ValidationResult("O campo PAÍS está em branco/vazio. Tente novamente!") : ValidationResult.Success;
        }
    }
}
