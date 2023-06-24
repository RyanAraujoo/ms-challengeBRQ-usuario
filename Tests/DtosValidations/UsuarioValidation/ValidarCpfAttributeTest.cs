using Domain.Validation.UsuarioValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.DtosValidations.UsuarioValidation
{
    public class ValidarCpfAttributeTest
    {
        private ValidarCpfAttribute validarCpfAttribute = new ValidarCpfAttribute();

        [Theory]
        [InlineData("12323432", false)]
        [InlineData("123.@56.789-10", false)]
        [InlineData("12345678911", true)]
        [InlineData(" ", false)]
        [InlineData("86834622853", true)]
        public void IsValid(string value, bool result)
        {
            var cpfIsValid = validarCpfAttribute.IsValid(value);

            Assert.Equal(result, cpfIsValid);
        }
    }
}
