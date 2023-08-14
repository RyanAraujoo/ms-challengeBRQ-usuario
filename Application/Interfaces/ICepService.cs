using Domain.Dto;
using Domain.Entity;

namespace Application.Interfaces
{
    public interface ICepService
    {
        public Task<CepDto> BuscarCep(string cep);
    }
}
