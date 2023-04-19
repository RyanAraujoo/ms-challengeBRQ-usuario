using Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain.Validation
{
    public class ValidarSexoAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value,
                                                   ValidationContext validationContext)
        {
            int outInt;
            if (StringToNullableInt(value.ToString()))
            {
                Sexo[] valores = (Sexo[])Sexo.GetValues(typeof(String));

                foreach (Sexo valor in valores)
                {
                    if (Convert.ToInt32(value) == (int)valor)
                    {
                        return ValidationResult.Success;
                    }
                } 
            }
            return new ValidationResult("Campo Sexo não especificado corretamente.");
        }

        private bool StringToNullableInt(string strNum)
        {
            int outInt;
            return int.TryParse(strNum, out outInt);
        }
    }
}
