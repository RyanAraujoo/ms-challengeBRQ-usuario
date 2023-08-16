using Domain.Dto;
using Infrastructure.Interfaces;
using System.Net.Http.Json;

namespace Infrastructure.Repository
{
    public class CepRepository : ICepRepository
    {
        private HttpClient client;
        public CepRepository(HttpClient _http)
        {
            client = _http;
        }
        public async Task<CepDto> BuscarCep(string cep)
        {
            HttpResponseMessage response = await client.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
            if (response.IsSuccessStatusCode)
            {
                var cepDto = await response.Content.ReadFromJsonAsync<CepDto>();
                return cepDto;
            } else
            {
                return new CepDto();
            }
        }
    }
}
