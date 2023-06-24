
using Domain.Validation.UsuarioValidation;
using Xunit;

namespace Tests.DtosValidations.UsuarioValidation
{
    public class ValidarEmailAttributeTest
    {
        private ValidarEmailAttribute validarEmailAttribute = new ValidarEmailAttribute();

        [Theory]
        [InlineData(" ", false)]
        [InlineData("sadsadasasd@gmail.com", true)]
        [InlineData("asdasdasds.com", false)]
        [InlineData(" ryanpablo@gmail.com.br", false)]
        [InlineData("@gmail.com.br ", false)]
        public void IsValid(string value, bool result)
        {
            var emailIsValid = validarEmailAttribute.IsValid(value);

            Assert.Equal(result, emailIsValid);
        }
    }
}
