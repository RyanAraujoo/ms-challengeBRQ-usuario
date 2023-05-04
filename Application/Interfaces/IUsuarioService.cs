﻿using Domain.Dto;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces
{
   public interface IUsuarioService
    { 
       public Task<Usuario> CadastrarUsuario(UsuarioDto usuarioDto);
       public Task<IEnumerable<object>> ListarUsuarios();
       public Task<Usuario> DetalharUsuario(Guid id);
       public Task<string> ExcluirUsuario(Guid id);
       public Task<Usuario> AtualizarUsuario(Guid id, PatchUsuarioDto fromBodyPutUsuarioDto);
       public Task<string> AlterarSenha(Guid id, TrocarSenhaDto senhas);
       public Task<HashDto> EsquecerSenha(Guid id);
       public Task<string> AlterarSenhaViaHash (Guid id,EsquecerSenhaDto hashComSenhaNova);
    }
}
