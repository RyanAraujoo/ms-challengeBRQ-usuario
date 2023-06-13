using Application.Interfaces;
using Domain.Dto;
using Domain.Entity;
using Domain.Enum;
using Infrastructure.Interfaces;
using System.Globalization;

namespace Application.Services
{

    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICepService _cepService;

        public UsuarioService(IUsuarioRepository usuarioRepository, ICepService cepService)
        {
            _usuarioRepository = usuarioRepository;
            _cepService = cepService;
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
            CepDto enderecoAPI = await _cepService.BuscarCep(usuarioDto.Endereco.Cep);
            usuario.Endereco = _cepService.EnriquecerEndereco(usuario.Endereco, enderecoAPI);
            usuario.Endereco.Cep = usuarioDto.Endereco.Cep;
            usuario.Endereco.Numero = usuarioDto.Endereco.Numero;

            if (String.IsNullOrEmpty(usuario.Endereco.Logradouro))
            {
                if (String.IsNullOrEmpty(usuarioDto.Endereco.Logradouro))
                {
                    throw new Exception("Não foi possível identificar o Logradouro. Informe, por favor.");
                }
                    usuario.Endereco.Logradouro = usuarioDto.Endereco.Logradouro;
            }

            if (String.IsNullOrEmpty(usuario.Endereco.Bairro))
            {
                if (String.IsNullOrEmpty(usuarioDto.Endereco.Bairro))
                {
                    throw new Exception("Não foi possível identificar o Bairro. Informe, por favor.");
                }
                usuario.Endereco.Bairro = usuarioDto.Endereco.Bairro;
            }

            await _usuarioRepository.CadastrarUsuario(usuario);
           
            return usuario;
        }
        
        public async Task<IEnumerable<UsuarioDetalhadoDto>> ListarUsuarios()
        {
           return await _usuarioRepository.ListarUsuarios(); 
        }

        public async Task<Usuario> DetalharUsuario(Guid id)
        {
            return await _usuarioRepository.DetalharUsuario(id);
        }

        public async Task<string> ExcluirUsuario(Guid id)
        {
           return await _usuarioRepository.ExcluirUsuario(id);
        }
        public async Task<Usuario> AtualizarUsuario(Guid id, PatchUsuarioDto fromBodyPutUsuario)
        {
            var buscarUsuarioParaAtualizar = await _usuarioRepository.buscarUsuario(id);
            if (buscarUsuarioParaAtualizar == null)
            {
                throw new Exception("Usuario não identificado.");
            }
            buscarUsuarioParaAtualizar = AtualizarCamposUsuario(buscarUsuarioParaAtualizar, fromBodyPutUsuario);

            await _usuarioRepository.AtualizarUsuario(buscarUsuarioParaAtualizar);

            return buscarUsuarioParaAtualizar;
        }

        private Usuario AtualizarCamposUsuario(Usuario usuarioInicial, PatchUsuarioDto usuarioFinal)
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

            usuarioInicial.Endereco = _usuarioRepository.buscarEnderecoUsuario(usuarioInicial).Result;

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
            var procurarUsuarioParaTrocarSenha = await _usuarioRepository.buscarUsuario(id);

            if (procurarUsuarioParaTrocarSenha == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            if (!(procurarUsuarioParaTrocarSenha.Senha == senhas.SenhaAtual))
            {
                throw new Exception("Senha atual incorreta.");
            }

            procurarUsuarioParaTrocarSenha.Senha = senhas.SenhaNova;
            procurarUsuarioParaTrocarSenha.DataAtualizacao = DateTime.Now;
            await _usuarioRepository.AtualizarUsuario(procurarUsuarioParaTrocarSenha);

            return "Senha atualizada com sucesso.";
        }
        public async Task<HashDto> EsquecerSenha(Guid id)
        {
            var usuarioEncontrado = await _usuarioRepository.buscarUsuario(id);

            if (usuarioEncontrado == null)
            {
                throw new Exception("Email Inválido/Não Existente");
            }
            HashDto hash = new HashDto();
            hash.CodigoSeguranca = Guid.NewGuid();
            hash.DataHoraCodigoSeguranca = DateTime.Now;

            usuarioEncontrado.CodigoSeguranca = hash.CodigoSeguranca;
            usuarioEncontrado.DataHoraCodigoSeguranca = hash.DataHoraCodigoSeguranca;
           await _usuarioRepository.AtualizarUsuario(usuarioEncontrado);
            return hash;
        }
        public async Task<string> AlterarSenhaViaHash(Guid id,EsquecerSenhaDto hashComSenhaNova)
        {
            var usuarioEncontrado = await _usuarioRepository.buscarUsuario(id);

            if (usuarioEncontrado == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            string minhaStringDeGuid = hashComSenhaNova.HashDeSeguranca;
            Guid meuGuid;
            Guid.TryParse(minhaStringDeGuid, out meuGuid);

            if (!(Guid.TryParse(minhaStringDeGuid, out meuGuid)))
            {
                throw new Exception("Hash Inválido. Tente novamente.");
            }

            if (!(meuGuid == usuarioEncontrado.CodigoSeguranca))
            {
                throw new Exception("Código de Segurança Inválido!");
            }

            if (usuarioEncontrado.Senha == hashComSenhaNova.novaSenha)
            {
                throw new Exception("Essa senha foi utilizada anteriormente!");
            }
            TimeSpan dataAtual = (TimeSpan)(DateTime.Now - usuarioEncontrado.DataHoraCodigoSeguranca);
            if (dataAtual.TotalMinutes >= 5)
            {
                throw new Exception("Tempo de mudança extrapolado! Gere um novo código.");
            }

            usuarioEncontrado.Senha = hashComSenhaNova.novaSenha;
            usuarioEncontrado.DataAtualizacao = DateTime.Now;

            _usuarioRepository.AtualizarUsuario(usuarioEncontrado);

            return "Senha atualizada com sucesso.";
        }
    }
}


