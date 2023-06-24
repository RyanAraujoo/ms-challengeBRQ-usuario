
using Domain.Validation.EnderecoValidation;
using Domain.Validation.UsuarioValidation;
using Xunit;

namespace Tests.DtosValidations.EnderecoValidation
{
    public class ValidarEstadoAttributeTest
    {
        private ValidarEstadoAttribute validarEstadoAttribute = new ValidarEstadoAttribute();

        [Theory]
        [InlineData("MN", false)]
        [InlineData("ba", true)]
        [InlineData("BA", true)]
        [InlineData(" ", false)]
        [InlineData("86834622853", false)]
        public void IsValid(string value, bool result)
        { 
            var estadoIsValid = validarEstadoAttribute.IsValid(value);

            Assert.Equal(result, estadoIsValid);
        }

    }
}
