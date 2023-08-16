using Application.Interfaces;
using Application.ViewModels;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services
{
    public class CepService : ICepService
    {
        private readonly ICepRepository _cepRepository;
        private IMemoryCache _memoryCache;
        private CepViewModel _cepViewModel;

        public CepService(IMemoryCache memoryCache, ICepRepository cepRepository)
        {
            _memoryCache = memoryCache;
            _cepRepository = cepRepository;
        }
        public async Task<CepViewModel> BuscarCep(string cep)
        {
            if (_memoryCache.TryGetValue(cep, out _cepViewModel))
            {
                return _cepViewModel;
            }

           var _cep = _cepRepository.BuscarCep(cep);
           _cepViewModel = new CepViewModel(_cep.Result.Cep, _cep.Result.Logradouro, _cep.Result.Complemento, _cep.Result.Bairro, _cep.Result.Localidade, _cep.Result.UF);
           _memoryCache.Set(cep, _cepViewModel, System.TimeSpan.FromDays(1));
           return _cepViewModel;
        }
    }

}
