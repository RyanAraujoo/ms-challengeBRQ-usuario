

using Domain.Validation.UsuarioValidation;
using Xunit;

namespace Tests.DtosValidations.UsuarioValidation
{
    public class ValidarNomeCompletoAttributeTest
    {
        private ValidarNomeCompletoAttribute validarNomeAttribute = new ValidarNomeCompletoAttribute();

        [Theory]
        [InlineData("aaa", false)]
        [InlineData(" ", false)]
        [InlineData("Ryan Pablo", true)]
        [InlineData("", false)]
     //   [InlineData("1321312412@$@", false)]
        public void IsValid(string value, bool result)
        {
            var nomeIsValid = validarNomeAttribute.IsValid(value);

            Assert.Equal(result, nomeIsValid);
        }
    }
}
