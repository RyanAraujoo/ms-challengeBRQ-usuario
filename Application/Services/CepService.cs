
using Application.Interfaces;
using Domain.Dto;
using Domain.Entity;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;

namespace Application.Services
{
    public class CepService : ICepService
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly IMemoryCache _memoryCache;

        public CepService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public async Task<CepDto> BuscarCep(string cep)
        {
            CepDto cepDto = null;
            if (!(_memoryCache.TryGetValue(cep, out cepDto)))
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

                    if (response.IsSuccessStatusCode)
                    {
                        cepDto = await response.Content.ReadFromJsonAsync<CepDto>();
                        _memoryCache.Set(cep, cepDto, System.TimeSpan.FromDays(1));
                        return cepDto;
                    }
                } catch (Exception ex)
                {
                    throw new Exception("Falha na comunicação com a API CEP.");
                }
               
            }
            return cepDto; 
        }
    }

}
