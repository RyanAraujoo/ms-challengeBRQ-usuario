using Domain.Dto;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IUsuarioRepository
    {
        public Task<bool> CadastrarUsuario(Usuario usuario);
        public Task<IEnumerable<object>> ListarUsuarios();
        public Task<Usuario> DetalharUsuario(Guid id);
        public Task<string> ExcluirUsuario(Guid id);
        public Task<Usuario> AtualizarUsuario(Usuario usuarioAtualizado);
        public Task<Usuario> buscarUsuario(Guid id);
        public Task<Endereco> buscarEnderecoUsuario(Usuario usuario);
    }
}
