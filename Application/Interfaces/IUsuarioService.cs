using Domain.Dto;
using Domain.Entity;

namespace Application.Interfaces
{
   public interface IUsuarioService
    { 
        // Task - Metodo Assincrono - Pesquisar
       public Task<Usuario> CadastrarUsuario(UsuarioDto usuarioDto);
    }
}
