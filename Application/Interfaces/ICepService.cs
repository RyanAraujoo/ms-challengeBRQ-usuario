using Application.ViewModels;

namespace Application.Interfaces
{
    public interface ICepService
    {
        public Task<CepViewModel> BuscarCep(string cep);
    }
}
