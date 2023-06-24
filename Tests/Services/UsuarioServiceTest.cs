using Application.Interfaces;
using Application.Services;
using Domain.Dto;
using Domain.Entity;
using Infrastructure.Interfaces;
using Moq;
using Presentation.Controllers;
using Xunit;

namespace Tests.Services
{
    public class UsuarioServiceTest
    {
        private UsuarioService _usuarioService;
        private readonly Mock<IUsuarioRepository> _usuarioRepository;
        private readonly Mock<ICepService> _cepService;

        public UsuarioServiceTest()
        {
            _usuarioRepository = new Mock<IUsuarioRepository>();
            _cepService = new Mock<ICepService>();
            _usuarioService = new UsuarioService(_usuarioRepository.Object, _cepService.Object);
        }

        [Fact(DisplayName = "CadastrarUsuario - Quando a função for chamada - Deve retornar o usuário criado")]
        public async Task CadastrarUsuario()
        {
            var enderecoId = Guid.NewGuid();

            var mockUsuarioDtoParaService = new UsuarioDto
            {
                Cpf = "123.456.789-00",
                Email = "exemplo@email.com",
                DataDeNascimento = new DateTime(1990, 1, 1).ToString("yyyy-MM-dd"),
                Sexo = "M",
                NomeCompleto = "Fulano de Tal",
                Senha = "senha123",
                Apelido = "fulaninho",
                Telefone = "7799999999",
                Endereco = new EnderecoDto
                {
                    Complemento = "Complemento Exemplo",
                    Numero = "123",
                    Cep = "45140000",
                    Bairro = "Bairro Exemplo",
                    Logradouro = "Rua Exemplo"
                },
            };

            var mockUsuarioResult = new Usuario
            {
                Id = Guid.NewGuid(),
                EnderecoId = enderecoId,
                DataDeNascimento = new DateTime(1990, 1, 1),
                Cpf = "123.456.789-00",
                Email = mockUsuarioDtoParaService.Email,
                NomeCompleto = mockUsuarioDtoParaService.NomeCompleto,
                Senha = mockUsuarioDtoParaService.Senha,
                Apelido = mockUsuarioDtoParaService.Apelido,
                Telefone = mockUsuarioDtoParaService.Telefone,
                CodigoSeguranca = null,
                DataHoraCodigoSeguranca = null,
                DataCadastro = DateTime.Now,
                DataAtualizacao = null,
                Sexo = 1,
                Endereco = new Endereco
                {
                    Id = enderecoId,
                    Logradouro = mockUsuarioDtoParaService.Endereco.Logradouro,
                    Complemento = mockUsuarioDtoParaService.Endereco.Complemento,
                    Numero = mockUsuarioDtoParaService.Endereco.Numero,
                    Bairro = mockUsuarioDtoParaService.Endereco.Bairro,
                    Cidade = "Itambé",
                    Estado = "BA",
                    Pais = "BR",
                    Cep = mockUsuarioDtoParaService.Endereco.Cep
                }
            };

            var mockCepDto = new CepDto
            {
                Cep = "45140-000",
                Logradouro = "",
                Complemento = "",
                Bairro = "",
                Localidade = "Itambé",
                UF = "BA",
                IBGE = "2915809",
                GIA = "",
                DDD = "77",
                Siafi = "3617"
            };
            var enderecoCriadoNoServiceViaCep = new Endereco
            {
                Complemento = mockUsuarioDtoParaService.Endereco.Complemento,
                Numero = mockUsuarioDtoParaService.Endereco.Numero,
                Cep = mockUsuarioDtoParaService.Endereco.Cep,
                Bairro = mockUsuarioDtoParaService.Endereco.Bairro,
                Logradouro = mockUsuarioDtoParaService.Endereco.Logradouro,
                Cidade = "Itambé",
                Estado = "BA",
                Pais = "BR",
                Id = enderecoId
            };
            var enderecoEnriquecidoComDadosApiCEP = new Endereco
            {
                Complemento = mockUsuarioDtoParaService.Endereco.Complemento,
                Numero = mockUsuarioDtoParaService.Endereco.Numero,
                Cep = mockUsuarioDtoParaService.Endereco.Cep,
                Bairro = mockUsuarioDtoParaService.Endereco.Bairro,
                Logradouro = mockUsuarioDtoParaService.Endereco.Logradouro,
                Cidade = mockCepDto.Localidade,
                Estado = mockCepDto.UF,
                Pais = "BR",
                Id = enderecoId
            };

            Task<bool> _repository = Task.Run(() => true);
            _usuarioRepository.Setup(s => s.CadastrarUsuario(mockUsuarioResult)).Returns(_repository);

            Task<CepDto> _cepDto = Task.Run(() => mockCepDto);
            _cepService.Setup(s => s.BuscarCep(It.IsAny<string>())).Returns(_cepDto);

            var result = await _usuarioService.CadastrarUsuario(mockUsuarioDtoParaService);

            Assert.IsType<Usuario>(result);
        }

        [Fact(DisplayName = "ListarUsuario - Quando a função for chamada - Deve retornar uma lista de usuários")]
        public async Task ListarUsuarios()
        {
            IEnumerable<UsuarioDetalhadoDto> listaUsuariosMock = new List<UsuarioDetalhadoDto>
            {
                new UsuarioDetalhadoDto
                {
                    Id = Guid.NewGuid(),
                    Cpf = "12345678901",
                    Email = "exemplo1@email.com",
                    NomeCompleto = "Fulano de Tal 1"
                },
                new UsuarioDetalhadoDto
                {
                    Id = Guid.NewGuid(),
                    Cpf = "12345678902",
                    Email = "exemplo2@email.com",
                    NomeCompleto = "Fulano de Tal 2"
                },
                new UsuarioDetalhadoDto
                {
                    Id = Guid.NewGuid(),
                    Cpf = "12345678903",
                    Email = "exemplo3@email.com",
                    NomeCompleto = "Fulano de Tal 3"
                }
            };

            Task<IEnumerable<UsuarioDetalhadoDto>> _usuario = Task.Run(() => listaUsuariosMock);
            _usuarioRepository.Setup(s => s.ListarUsuarios()).Returns(_usuario);

            var result = _usuarioService.ListarUsuarios().Result;

            Assert.Equal(listaUsuariosMock, result);
        }

        [Fact(DisplayName = "DetalharUsuario - Quando a função for chamada - Deve retornar o usuario especificado detalhado")]
        public async Task DetalharUsuario()
        {
            Guid enderecoId = Guid.NewGuid();
            Guid UserId = Guid.NewGuid();
            var mockUsuarioResult = new Usuario
            {
                Id = UserId,
                EnderecoId = enderecoId,
                DataDeNascimento = new DateTime(1990, 1, 1),
                Cpf = "123.456.789-00",
                Email = "exemplo@email.com",
                NomeCompleto = "Fulano de Tal",
                Senha = "senha123",
                Apelido = "fulaninho",
                Telefone = "7799999999",
                CodigoSeguranca = null,
                DataHoraCodigoSeguranca = null,
                DataCadastro = DateTime.Now,
                DataAtualizacao = null,
                Sexo = 1,
                Endereco = new Endereco
                {
                    Id = enderecoId,
                    Logradouro = "Rua Exemplo",
                    Complemento = "Complemento Exemplo",
                    Numero = "123",
                    Bairro = "Bairro Exemplo",
                    Cidade = "Itambé",
                    Estado = "BA",
                    Pais = "País Exemplo",
                    Cep = "45140000"
                }
            };

            Task<Usuario> _usuario = Task.Run(() => mockUsuarioResult);
            _usuarioRepository.Setup(s => s.DetalharUsuario(UserId)).Returns(_usuario);

            var result = _usuarioService.DetalharUsuario(UserId).Result;
            Assert.Equal(mockUsuarioResult, result);
        }

        [Fact(DisplayName = "ExcluirUsuario - Quando a função for chamada - Deve retornar um valor booleano de confirmação da exclusão usuário")]
        public async Task ExcluirUsuario()
        {
            Guid UserId = Guid.NewGuid();

            Task<bool> _usuario = Task.Run(() => true);

            _usuarioRepository.Setup(s => s.ExcluirUsuario(UserId)).Returns(_usuario);

            var result = _usuarioService.ExcluirUsuario(UserId).Result;
            Assert.Equal(true, result);
        }

      /*[Fact(Displ*ayName = "AtualizarUsuario - Quando a função for chamada - Deve retornar um valor booleano de confirmação da exclusão usuário")]
        public async Task AtualizarUsuario()
        {
            Guid UserId = Guid.NewGuid();
            var enderecoId = Guid.NewGuid();
            var patchUsuarioDto = new PatchUsuarioDto
            {
                Email = "joao@example.com",
            };
            var mockUsuarioResult = new Usuario
            {
                Id = UserId,
                EnderecoId = enderecoId,
                DataDeNascimento = new DateTime(1990, 1, 1),
                Cpf = "123.456.789-00",
                Email = "joao@example.com",
                NomeCompleto = "Fulano de Tal",
                Senha = "senha123",
                Apelido = "fulaninho",
                Telefone = "7799999999",
                CodigoSeguranca = null,
                DataHoraCodigoSeguranca = null,
                DataCadastro = DateTime.Now,
                DataAtualizacao = null,
                Sexo = 1,
                Endereco = new Endereco
                {
                    Id = enderecoId,
                    Logradouro = "Rua Exemplo",
                    Complemento = "Complemento Exemplo",
                    Numero = "123",
                    Bairro = "Bairro Exemplo",
                    Cidade = "Itambé",
                    Estado = "BA",
                    Pais = "País Exemplo",
                    Cep = "45140000"
                }
            };

            Task<Usuario> _usuario = Task.Run(() => mockUsuarioResult);
            Task<Usuario> _usuario2 = Task.Run(() => mockUsuarioResult);
            _usuarioRepository.Setup(s => s.buscarUsuario(UserId)).Returns(_usuario2);
            _usuarioRepository.Setup(s => s.AtualizarUsuario(mockUsuarioResult)).Returns(_usuario);

            var result = _usuarioService.AtualizarUsuario(UserId, patchUsuarioDto).Result;
            Assert.Equal(mockUsuarioResult, result);
        } */

    }
}