
using Domain.Validation.EnderecoValidation;
using Xunit;

namespace Tests.DtosValidations.EnderecoValidation
{
    public class ValidarPaisAttributeTest
    {
        private ValidarPaisAttribute validarPaisAttribute = new ValidarPaisAttribute();

        [Theory]
        [InlineData("ARG", false)]
        [InlineData("SP", false)]
        [InlineData("BR", true)]
        [InlineData("br", true)]
        [InlineData("86834622853", false)]
        public void IsValid(string value, bool result)
        {
            var paisIsValid = validarPaisAttribute.IsValid(value);

            Assert.Equal(result, paisIsValid);
        }
    }
}
