
using Domain.Dto;
using Domain.Entity;
using Infrastructure.DataBase;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;

namespace Infrastructure.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApiDbContext _context;
        public UsuarioRepository(ApiDbContext context)
        {
            _context = context;
        }

        public bool TestarConexao()
        {
            string connectionString = "Server=localhost;DataBase=ChallengeDbUsuarios;Uid=root;Pwd=";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    return true;
                }
                catch (MySqlException ex)
                {
                    return false;
                }
            }
        }
        public async Task<Usuario> AtualizarUsuario(Usuario usuarioAtualizado)
        {
            _context.Usuarios.Update(usuarioAtualizado);
            await _context.SaveChangesAsync();
            return usuarioAtualizado;
        }
        public async Task<Usuario> buscarUsuario(Guid id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<Endereco> buscarEnderecoUsuario(Usuario usuario)
        {
            return await _context.Enderecos.FirstOrDefaultAsync(e => e.Id == usuario.EnderecoId);
        }
        public async Task<bool> CadastrarUsuario(Usuario usuario)
        {
                await _context.Usuarios.AddAsync(usuario);
                await _context.SaveChangesAsync();
                return true;
        }
        public async Task<Usuario> DetalharUsuario(Guid id)
        {
            var reqUsuario = await _context.Usuarios.FirstOrDefaultAsync(e => e.Id.Equals(id));
            if (reqUsuario == null)
            {
                throw new Exception("Usuário não encontrado.");
            }
            reqUsuario.Endereco = await _context.Enderecos.FirstOrDefaultAsync(e => e.Id.Equals(reqUsuario.EnderecoId));
            return reqUsuario;
        }
        public async Task<string> ExcluirUsuario(Guid id)
        {
            var buscarUsuarioParaExcluir = buscarUsuario(id);

            if (buscarUsuarioParaExcluir == null)
            {
                throw new Exception("Cliente não encontrado para exclusão.");
            }

            _context.Usuarios.Remove(buscarUsuarioParaExcluir.Result) ;
            await _context.SaveChangesAsync();
            return "Usuario Removido com Sucesso!";
        }
        public async Task<IEnumerable<UsuarioDetalhadoDto>> ListarUsuarios()
        {
            return (IEnumerable<UsuarioDetalhadoDto>)await _context.Usuarios.Select(ItemObjUsuario => new {
                ItemObjUsuario.Id,
                ItemObjUsuario.Cpf,
                ItemObjUsuario.Email,
                ItemObjUsuario.NomeCompleto
            }).ToListAsync();
        }
    }
}
