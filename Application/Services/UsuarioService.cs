using Application.Interfaces;
using Domain.Dto;
using Domain.Entity;
using Domain.Enum;
using Infrastructure.DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System;
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
            usuario.DataDeNascimento = DateTime.ParseExact(usuarioDto.DataDeNascimento, "yyyy-MM-dd", CultureInfo.InvariantCulture);
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

        public async Task<IEnumerable<object>> ListarUsuarios()
        {
            return await _context.Usuarios.Select(ItemObjUsuario => new {
                ItemObjUsuario.Id,
                ItemObjUsuario.Cpf,
                ItemObjUsuario.Email,
                ItemObjUsuario.NomeCompleto
            }).ToListAsync();
        }

        public async Task<Usuario> DetalharUsuario(Guid id)
        {
            var reqUsuario = await _context.Usuarios.FirstOrDefaultAsync(e => e.Id.Equals(id));

            if (reqUsuario == null)
            {
                return null;
            }

            reqUsuario.Endereco = await _context.Enderecos.FirstOrDefaultAsync(e => e.Id.Equals(reqUsuario.EnderecoId));

            return reqUsuario;
        }

        public async Task<string> ExcluirUsuario(Guid id)
        {
            var buscarUsuarioParaExcluir = await _context.Usuarios.FirstOrDefaultAsync(e => e.Id == id);

            if (buscarUsuarioParaExcluir == null)
            {
                return "Cliente não encontrado para remoção!";
            }

            _context.Usuarios.Remove(buscarUsuarioParaExcluir);
            await _context.SaveChangesAsync();

            return "Usuario Removido com Sucesso!";
            
        }
        public async Task<Usuario> AtualizarUsuario(Guid id, FromBodyPutUsuarioDto fromBodyPutUsuario)
        {
            var buscarUsuarioParaAtualizar = _context.Usuarios.FirstOrDefault(e => e.Id == id);

            if (buscarUsuarioParaAtualizar == null)
            {
                return null;
            }

            buscarUsuarioParaAtualizar = AtualizarCamposUsuario(buscarUsuarioParaAtualizar, fromBodyPutUsuario);
            _context.Usuarios.Update(buscarUsuarioParaAtualizar);
            await _context.SaveChangesAsync();

            return buscarUsuarioParaAtualizar;
        }

        private Usuario AtualizarCamposUsuario(Usuario usuarioInicial, FromBodyPutUsuarioDto usuarioFinal)
        {

            if(!string.IsNullOrEmpty(usuarioFinal.NomeCompleto))
            {
                usuarioInicial.NomeCompleto = usuarioFinal.NomeCompleto;
            }

            if(!string.IsNullOrEmpty(usuarioFinal.Telefone))
            {
                usuarioInicial.Telefone = usuarioFinal.Telefone;
            }

            if (!string.IsNullOrEmpty(usuarioFinal.Sexo))
            {
                usuarioInicial.Sexo = TrasnformarStringEmIntEnumSexo(usuarioFinal.Sexo);
            }

            if (!string.IsNullOrEmpty(usuarioFinal.Apelido))
            {
                usuarioInicial.Apelido = usuarioFinal.Apelido;
            }

            if (!string.IsNullOrEmpty(usuarioFinal.Senha))
            {
                usuarioInicial.Senha = usuarioFinal.Senha;
            }

            if (!string.IsNullOrEmpty(usuarioFinal.Senha))
            {
                usuarioInicial.Senha = usuarioFinal.Senha;
            }

            usuarioInicial.Endereco = new Endereco();
            usuarioInicial.Endereco.Id = usuarioInicial.EnderecoId;

            usuarioInicial.Endereco = _context.Enderecos.FirstOrDefault(e => e.Id == usuarioInicial.EnderecoId);
            if (!string.IsNullOrEmpty(usuarioFinal.Endereco.Cidade))
            {
                usuarioInicial.Endereco.Cidade = usuarioFinal.Endereco.Cidade;
            }

            if (!string.IsNullOrEmpty(usuarioFinal.Endereco.Numero))
            {
                usuarioInicial.Endereco.Numero = usuarioFinal.Endereco.Numero;
            }

            if (!string.IsNullOrEmpty(usuarioFinal.Endereco.Cep))
            {
                usuarioInicial.Endereco.Cep = usuarioFinal.Endereco.Cep;
            }

            if (!string.IsNullOrEmpty(usuarioFinal.Endereco.Logradouro))
            {
                usuarioInicial.Endereco.Logradouro = usuarioFinal.Endereco.Logradouro;
            }

            if (!string.IsNullOrEmpty(usuarioFinal.Endereco.Bairro))
            {
                usuarioInicial.Endereco.Bairro = usuarioFinal.Endereco.Bairro;
            }

            if (!string.IsNullOrEmpty(usuarioFinal.Endereco.Pais))
            {
                usuarioInicial.Endereco.Pais = usuarioFinal.Endereco.Pais;
            }

            usuarioInicial.DataAtualizacao = DateTime.Now;

            return usuarioInicial;
        }
        public async Task<string> AlterarSenha(Guid id, TrocarSenhaDto senhas)
        {
            var procurarUsuarioParaTrocarSenha = _context.Usuarios.FirstAsync(x => x.Id == id);

            if (procurarUsuarioParaTrocarSenha == null)
            {
                throw new Exception("Usuário não encontrado");
            }

           if (!(procurarUsuarioParaTrocarSenha.Result.Senha == senhas.SenhaAtual))
            {
                throw new Exception("Senha atual incorreta.");
            }

            procurarUsuarioParaTrocarSenha.Result.Senha = senhas.SenhaNova;
            _context.Usuarios.Update(procurarUsuarioParaTrocarSenha.Result);
            await _context.SaveChangesAsync();

            return "Senha atualizada com sucesso.";
        }
    }
}
