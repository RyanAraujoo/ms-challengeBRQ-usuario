
using Application.Interfaces;
using Domain.Dto;
using Domain.Entity;
using System.Net.Http.Json;

namespace Application.Services
{
    public class CepService : ICepService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<CepDto> BuscarCep(string cep)
        {
            CepDto cepDto = null;
            HttpResponseMessage response = await client.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

            if (response.IsSuccessStatusCode)
            {
                cepDto = await response.Content.ReadFromJsonAsync<CepDto>();
                return cepDto;
            }
                throw new Exception("Falha na comunicação com a API CEP.");
        }

        public Endereco EnriquecerEndereco(Endereco Endereco, CepDto EnderecoAPI)
        {
            Endereco.Cep = EnderecoAPI.Cep;
            Endereco.Logradouro = EnderecoAPI.Logradouro;
            Endereco.Bairro = EnderecoAPI.UF;
            Endereco.Cidade = EnderecoAPI.Localidade;
            Endereco.Complemento = EnderecoAPI.Complemento;
            return Endereco;
        }
    }

}
