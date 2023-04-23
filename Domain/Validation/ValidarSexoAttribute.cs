using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Validation
{
    public class ValidarSexoAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value,
                                                     ValidationContext validationContext)
        {
            if (value.ToString().Length == 1)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("O Sexo não foi especificado corretamente");
        }
    }
}
