using Domain.Dto;
using Domain.Entity;

namespace Application.Interfaces
{
    public interface ICepService
    {
        public Task<CepDto> BuscarCep(string cep);
        public Endereco EnriquecerEndereco(Endereco Endereco, CepDto EnderecoAPI);
    }
}
