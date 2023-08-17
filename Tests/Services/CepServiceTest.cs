using Application.Interfaces;
using Application.Services;
using Application.ViewModels;
using Domain.Dto;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace Tests.Services
{
    public class CepServiceTest
    {
        private ICepService _cepService;
        private readonly Mock<IMemoryCache> _memoryCache;
        private readonly Mock<ICepRepository> _cepRepository;
        public CepServiceTest() {
            _memoryCache = new Mock<IMemoryCache>();
            _cepRepository = new Mock<ICepRepository>();
            _cepService = new CepService(_memoryCache.Object,_cepRepository.Object);
        }

        [Fact(DisplayName = "BuscarCep - Quando a função for chamada - Deve buscar os dados de um CEP")]
        public async Task BuscarCep()
        {
            var mockCepDto = new CepDto
            {
                Cep = "45140-000",
                Logradouro = "",
                Complemento = "",
                Bairro = "",
                Localidade = "Itambé",
                UF = "BA",
                IBGE = "2915809",
                GIA = "",
                DDD = "77",
                Siafi = "3617"
            };
            Task<CepDto> _repository = Task.Run(() => mockCepDto);
            _cepRepository.Setup(x => x.BuscarCep("45140000")).Returns(_repository);
            _memoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);
            var result = await _cepService.BuscarCep("45140000");
            Assert.IsType<CepViewModel>(result);
        }

        [Fact(DisplayName = "BuscarCepComFalha - Quando a função for chamada - Deve retornar uma exceptions")]
        public async Task BuscarCepComFalha()
        {
            var mockCepDto = new CepDto
            {
                Cep = "45140-000",
                Logradouro = "",
                Complemento = "",
                Bairro = "",
                Localidade = "Itambé",
                UF = "BA",
                IBGE = "2915809",
                GIA = "",
                DDD = "77",
                Siafi = "3617"
            };
            Task<CepDto> _repository = Task.Run(() => mockCepDto);
            _cepRepository.Setup(x => x.BuscarCep("45140000")).Returns(_repository);
            _memoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Throws(new Exception("Falha na comunicação com a API CEP."));
            await Assert.ThrowsAsync<Exception>(async () => await _cepService.BuscarCep("45140000"));
        }
    }
}
