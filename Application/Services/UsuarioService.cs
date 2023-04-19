using Application.Interfaces;
using Domain.Dto;
using Domain.Entity;
using Domain.Enum;
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

        private int TrasnformarStringEmIntEnumSexo(string stringValueSexo)
        {
            foreach (var item in Enum.GetValues(typeof(Sexo)))
            {
                if (stringValueSexo == item.ToString())
                {
                    return (int)item;
                }
            }
            return 4;
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
            usuario.DataDeNascimento = usuarioDto.DataDeNascimento;
            usuario.Sexo = this.TrasnformarStringEmIntEnumSexo(usuarioDto.Sexo);
            usuario.DataCadastro = DateTime.Now;

            usuario.Endereco = new Endereco();
            usuario.Endereco.Id = new Guid();
            usuario.Endereco.Logradouro = usuarioDto.Endereco.Logradouro;
            usuario.Endereco.Cep = usuarioDto.Endereco.Cep;
            usuario.Endereco.Numero = usuarioDto.Endereco.Numero;
            usuario.Endereco.Bairro = usuarioDto.Endereco.Bairro;
            usuario.Endereco.Estado = usuarioDto.Endereco.Estado;
            usuario.Endereco.Cidade = usuarioDto.Endereco.Cidade;
            usuario.Endereco.Pais = usuarioDto.Endereco.Pais;

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }
    }
}
