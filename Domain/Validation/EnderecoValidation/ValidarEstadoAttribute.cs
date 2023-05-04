using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Validation.EnderecoValidation
{
    public class ValidarEstadoAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (!(ValidarEstadoBrasileiro(value.ToString().ToUpper()))) {
                return new ValidationResult("O campo ESTADO for inserido com valor Inválido");
            }

            return string.IsNullOrWhiteSpace(value.ToString()) ? new ValidationResult("O campo ESTADO está em branco/vazio. Tente novamente!") : ValidationResult.Success;
        }

        private bool ValidarEstadoBrasileiro(string estado)
        {
            Regex regex = new Regex(@"^(AC|AL|AP|AM|BA|CE|DF|ES|GO|MA|MT|MS|MG|PA|PB|PR|PE|PI|RJ|RN|RS|RO|RR|SC|SP|SE|TO)$");
            return regex.IsMatch(estado);
        }

    }
}
