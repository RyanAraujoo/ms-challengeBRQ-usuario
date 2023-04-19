using System.ComponentModel.DataAnnotations;

namespace Domain.Validation
{
    public class ValidarDataAtualizacaoAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value.GetType() == typeof(Nullable)) 
            {
               return ValidationResult.Success;
            }
            return new ValidationResult("A DataAtualização deve ser nula no momento de criação.");
        }
    }
}
