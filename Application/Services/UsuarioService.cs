using Application.InputModels;
using Application.Interfaces;
using Application.ViewModels;
using Domain.Dto;
using Domain.Entity;
using Domain.Enum;
using Infrastructure.Interfaces;

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

        private int TrasnformarStringEnumSexoEmIntEnumSexo(string stringValueSexo)
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

        public async Task<UsuarioViewModel> CadastrarUsuario(UsuarioInputModel usuarioInputModel)
        {
            Usuario usuario = new Usuario
            {
                Cpf = usuarioInputModel.Cpf,
                Email = usuarioInputModel.Email,
                NomeCompleto = usuarioInputModel.NomeCompleto,
                Senha = usuarioInputModel.Senha,
                Apelido = usuarioInputModel.Apelido,
                Telefone = usuarioInputModel.Telefone,
                Sexo = TrasnformarStringEnumSexoEmIntEnumSexo(usuarioInputModel.Sexo)
            };
                
            CepViewModel enderecoAPI = await _cepService.BuscarCep(usuarioInputModel.Endereco.Cep);

            usuario.AtrelarEnderecoAoUsuario(
                enderecoAPI.Cep,
                enderecoAPI.Bairro,
                enderecoAPI.Logradouro,
                enderecoAPI.Localidade,
                enderecoAPI.UF,
                usuarioInputModel.Endereco.Complemento,
                usuarioInputModel.Endereco.Numero
                );

            usuario.DefinirDataDeNascimento(usuarioInputModel.DataDeNascimento);

            if (String.IsNullOrEmpty(usuario.Endereco.Logradouro))
            {
                if (String.IsNullOrEmpty(usuarioInputModel.Endereco.Logradouro))
                {
                    throw new Exception("Não foi possível identificar o Logradouro. Informe, por favor.");
                }
                    usuario.Endereco.Logradouro = usuarioInputModel.Endereco.Logradouro;
            }

            if (String.IsNullOrEmpty(usuario.Endereco.Bairro))
            {
                if (String.IsNullOrEmpty(usuarioInputModel.Endereco.Bairro))
                {
                    throw new Exception("Não foi possível identificar o Bairro. Informe, por favor.");
                }
                usuario.Endereco.Bairro = usuarioInputModel.Endereco.Bairro;
            }
            await _usuarioRepository.CadastrarUsuario(usuario);
            EnderecoInputModel _enderecoView = new EnderecoInputModel(usuarioInputModel.Endereco.Complemento,usuarioInputModel.Endereco.Numero,usuarioInputModel.Endereco.Cep,usuarioInputModel.Endereco.Bairro,usuarioInputModel.Endereco.Logradouro);
            return new UsuarioViewModel(usuario.Id,usuarioInputModel.Cpf,usuarioInputModel.Email,usuarioInputModel.DataDeNascimento,usuarioInputModel.Sexo,usuarioInputModel.NomeCompleto,usuarioInputModel.Apelido,usuarioInputModel.Telefone,usuario.DataCadastro,_enderecoView);
        }
        
        public async Task<IEnumerable<UsuarioDetalhadoDto>> ListarUsuarios()
        {
           return await _usuarioRepository.ListarUsuarios(); 
        }

        public async Task<Usuario> DetalharUsuario(Guid id)
        {
            return await _usuarioRepository.DetalharUsuario(id);
        }

        public async Task<bool> ExcluirUsuario(Guid id)
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
                usuarioInicial.Sexo = TrasnformarStringEnumSexoEmIntEnumSexo(usuarioFinal.Sexo);
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
                throw new Exception("Usuario Não Existente/Usuário incorreto");
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

            if (usuarioEncontrado.Senha == hashComSenhaNova.NovaSenha)
            {
                throw new Exception("Essa senha foi utilizada anteriormente!");
            }
            TimeSpan dataAtual = (TimeSpan)(DateTime.Now - usuarioEncontrado.DataHoraCodigoSeguranca);
            if (dataAtual.TotalMinutes >= 5)
            {
                throw new Exception("Tempo de mudança extrapolado! Gere um novo código.");
            }

            usuarioEncontrado.Senha = hashComSenhaNova.NovaSenha;
            usuarioEncontrado.DataAtualizacao = DateTime.Now;

            _usuarioRepository.AtualizarUsuario(usuarioEncontrado);

            return "Senha atualizada com sucesso.";
        }
    }
}


