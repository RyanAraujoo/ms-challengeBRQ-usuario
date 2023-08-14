using Domain.Validation.UsuarioValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.DtosValidations.UsuarioValidation
{
    public class ValidarDataDeNascimentoAttributeTest
    {
        private ValidarDataDeNascimentoAttribute validarDataDeNascimentoAttribute = new ValidarDataDeNascimentoAttribute();

        [Theory]
        [InlineData("2023-06-25", true)]
        [InlineData("2002-11-11", true)]
        [InlineData("11-11-2002", false)]
        [InlineData("11112002", false)]
        [InlineData(" ", false)]
        public void IsValid(string value, bool result)
        {
            var dataNascimentoIsValid = validarDataDeNascimentoAttribute.IsValid(value);

            Assert.Equal(result, dataNascimentoIsValid);
        }
    }
}
