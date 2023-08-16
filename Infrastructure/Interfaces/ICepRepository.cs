using Domain.Dto;

namespace Infrastructure.Interfaces
{
    public interface ICepRepository
    {
        public Task<CepDto> BuscarCep(string cep);
    }
}
