using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;


namespace Domain.Validation
{
    public class ValidarDataCadastroAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value,
                                                     ValidationContext validationContext)
        {
            if (!ValidarDataHora(value.ToString()))
            {
                return new ValidationResult("A DataDeCadastro deve conter data e hora.");
            }
            return ValidationResult.Success;
        }

        private bool ValidarDataHora(string dataHora)
        {
            string padrao = @"^\d{2}\/\d{2}\/\d{4} \d{2}:\d{2}:\d{2}$";
            return Regex.IsMatch(dataHora, padrao);
        }
    }
}
