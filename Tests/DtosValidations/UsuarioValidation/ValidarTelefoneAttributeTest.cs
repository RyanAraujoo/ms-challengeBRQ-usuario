
using Domain.Validation.UsuarioValidation;
using Xunit;

namespace Tests.DtosValidations.UsuarioValidation
{
    public class ValidarTelefoneAttributeTest
    {
        private ValidarTelefoneAttribute validarTelefoneAttribute = new ValidarTelefoneAttribute();

        [Theory]
        [InlineData("77998578542", true)]
        [InlineData("7798578542", false)]
        [InlineData("779857-8542", false)]
        [InlineData("123", false)]
        [InlineData(" ", false)]
        [InlineData("aaaaaaaaaaaaaaa$%¨&**", false)]
        public void IsValid(string value, bool result)
        {
            var telefoneIsValid = validarTelefoneAttribute.IsValid(value);

            Assert.Equal(result, telefoneIsValid);
        }
    }
}
