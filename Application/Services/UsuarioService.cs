using Application.Interfaces;
using Domain.Dto;
using Domain.Entity;
using Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApiDbContext _context;

        public UsuarioService(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> CadastrarUsuario(UsuarioDto usuarioDto)
        {
            Usuario usuario = new Usuario();
            usuario.Id = new Guid();
            usuario.EnderecoId = new Guid();
            usuario.NomeCompleto = usuarioDto.NomeCompleto;
            usuario.Apelido = usuarioDto.Apelido;
            usuario.Email = usuarioDto.Email;
            usuario.Cpf = usuarioDto.Cpf;
            usuario.Senha = usuarioDto.Senha;
            usuario.Telefone = usuarioDto.Telefone;
            usuario.DataDeNascimento = DateTime.ParseExact(usuarioDto.DataDeNascimento, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            usuario.DataCadastro = DateTime.Now;
            usuario.Sexo = (Domain.Enum.Sexo)(int?)usuarioDto.Sexo;

            usuario.Endereco = new Endereco();
            usuario.Endereco.Id = new Guid();
            usuario.Endereco.Logradouro = usuarioDto.Endereco.Logradouro;
            usuario.Endereco.Cep = usuarioDto.Endereco.Cep;
            usuario.Endereco.Numero = usuarioDto.Endereco.Numero;
            usuario.Endereco.Bairro = usuarioDto.Endereco.Bairro;
            usuario.Endereco.Cidade = usuarioDto.Endereco.Cidade;
            usuario.Endereco.Pais = usuarioDto.Endereco.Pais;

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }
    }
}
