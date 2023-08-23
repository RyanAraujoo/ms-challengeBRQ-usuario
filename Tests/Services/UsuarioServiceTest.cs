using Application.InputModels;
using Application.Interfaces;
using Application.Services;
using Application.ViewModels;
using Domain.Dto;
using Domain.Entity;
using Infrastructure.Interfaces;
using Moq;
using Xunit;

namespace Tests.Services
{
    public class UsuarioServiceTest
    {
        private UsuarioService _usuarioService;
        private readonly Mock<IUsuarioRepository> _usuarioRepository;
        private readonly Mock<ICepService> _cepService;
        private readonly Usuario _usuarioResultMock;
        private readonly UsuarioInputModel _usuarioInputMock;
        private readonly UsuarioViewModel _usuarioViewModelMock;
        private readonly CepViewModel _cepViewModelMock;

        public UsuarioServiceTest()
        {
            _usuarioRepository = new Mock<IUsuarioRepository>();
            _cepService = new Mock<ICepService>();
            _usuarioService = new UsuarioService(_usuarioRepository.Object, _cepService.Object);
            _usuarioInputMock = new UsuarioInputModel
            {
                Cpf = "123.456.789-00",
                Email = "exemplo@email.com",
                DataDeNascimento = new DateTime(1990, 1, 1).ToString("yyyy-MM-dd"),
                Sexo = "M",
                NomeCompleto = "Fulano de Tal",
                Senha = "senha123",
                Apelido = "fulaninho",
                Telefone = "7799999999",
                Endereco = new EnderecoInputModel("Complemento Exemplo", "123", "45140000", "Bairro Exemplo", "Rua Exemplo")
            };
            _usuarioViewModelMock = new UsuarioViewModel(Guid.NewGuid(), "123.456.789-00", "exemplo@email.com", "1990-11-11", "M", "Fulano de Tal", "Fulaninho", "7799999999", DateTime.Now, new EnderecoInputModel("Complemento Exemplo", "123", "45140000", "Bairro Exemplo", "Rua Exemplo"));
            _usuarioResultMock = new Usuario
            {
                Cpf = "123.456.789-00",
                Email = "exemplo@gmail.com",
                NomeCompleto = "Fulano de Tal",
                Senha = "senha123",
                Apelido = "fulaninho",
                Telefone = "7799999999",
                Sexo = 1
            };
            _usuarioResultMock.AtrelarEnderecoAoUsuario("45140000", "Bairro Exemplo", "Logradouro Exemplo", "Itambé", "BA", "", "12");
            _usuarioResultMock.DefinirDataDeNascimento("1990-01-01");
            _cepViewModelMock = new CepViewModel("45140000", "", "", "", "Itambé", "BA");
        }

        [Fact(DisplayName = "Transformar Data em DateTime - YYYY-MM-DD - Quando a função for chamada - Deve tratar a data de nascimento")]
        public async Task TratarDataDeNascimento()
        {
            _usuarioResultMock.DefinirDataDeNascimento("1999-11-11");
            Assert.IsType<DateTime>(_usuarioResultMock.DataDeNascimento);
        }


        [Fact(DisplayName = "CadastrarUsuario - Quando a função for chamada - Deve retornar o usuário criado")]
        public async Task CadastrarUsuario()
        {
            Task<bool> _repository = Task.Run(() => true);
            _usuarioRepository.Setup(s => s.CadastrarUsuario(_usuarioResultMock)).Returns(_repository);
            Task<CepViewModel> _cepDto = Task.Run(() => _cepViewModelMock);
            _cepService.Setup(s => s.BuscarCep(It.IsAny<string>())).Returns(_cepDto);
            var result = await _usuarioService.CadastrarUsuario(_usuarioInputMock);
            Assert.IsType<UsuarioViewModel>(result);
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
            Task<Usuario> _usuario = Task.Run(() => _usuarioResultMock);
            _usuarioRepository.Setup(s => s.DetalharUsuario(_usuarioResultMock.Id)).Returns(_usuario);
            var result = _usuarioService.DetalharUsuario(_usuarioResultMock.Id).Result;
            Assert.Equal(_usuarioResultMock, result);
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

        [Fact(DisplayName = "AtualizarUsuario - Quando a função for chamada - Deve retornar o usuário atualizado")]
        public async Task AtualizarUsuario()
        {
            Guid UserId = Guid.NewGuid();
            var enderecoId = Guid.NewGuid();
            var patchUsuarioDto = new PatchUsuarioDto
            {
                Email = "joao@example.com",
                Endereco = new PatchEnderecoDto
                {
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
            var mockEndereco = new Endereco
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
            };
            Task<Usuario> _usuario = Task.Run(() => _usuarioResultMock);
            Task<Usuario> _usuario2 = Task.Run(() => _usuarioResultMock);
            Task<Endereco> _enderecoUsuario = Task.Run(() => mockEndereco);
            _usuarioRepository.Setup(s => s.buscarUsuario(UserId)).Returns(_usuario2);
            _usuarioRepository.Setup(s => s.AtualizarUsuario(_usuarioResultMock)).Returns(_usuario);
            _usuarioRepository.Setup(s => s.buscarEnderecoUsuario(_usuarioResultMock)).Returns(_enderecoUsuario);
            var result = _usuarioService.AtualizarUsuario(UserId, patchUsuarioDto).Result;
            Assert.Equal(_usuarioResultMock, result);
        }

        [Fact(DisplayName = "TrocarSenha - Quando a função for chamada - Deve retornar uma string de confirmação")]
        public async Task TrocarSenha()
        {
            var _usuarioResultMockAtualizadoComSenhaNova = new Usuario
            {
                Cpf = "123.456.789-00",
                Email = "exemplo@gmail.com",
                NomeCompleto = "Fulano de Tal",
                Senha = "senha123456789",
                Apelido = "fulaninho",
                Telefone = "7799999999",
                Sexo = 1
            };
            Task<Usuario> _usuario = Task.Run(() => _usuarioResultMockAtualizadoComSenhaNova);
            Task<Usuario> _usuario2 = Task.Run(() => _usuarioResultMock);
            _usuarioRepository.Setup(s => s.buscarUsuario(_usuarioResultMock.Id)).Returns(_usuario2);
            _usuarioRepository.Setup(s => s.AtualizarUsuario(_usuarioResultMock)).Returns(_usuario);

            var _body = new TrocarSenhaDto
            {
                SenhaAtual = "senha123",
                SenhaNova = "senha123456789",
            };

            var result = _usuarioService.AlterarSenha(_usuarioResultMock.Id, _body).Result;

            Assert.Equal("Senha atualizada com sucesso.", result);
        }

        [Fact(DisplayName = "EsquecerSenha - Quando a função for chamada - Deve retornar um valor booleano de confirmação da exclusão usuário")]
        public async Task EsquecerSenha()
        {
            Guid UserId = Guid.NewGuid();
            var enderecoId = Guid.NewGuid();
            Task<Usuario> _usuario = Task.Run(() => _usuarioResultMock);
            Task<Usuario> _usuario2 = Task.Run(() => _usuarioResultMock);
            _usuarioRepository.Setup(s => s.buscarUsuario(UserId)).Returns(_usuario2);
            _usuarioRepository.Setup(s => s.AtualizarUsuario(_usuarioResultMock)).Returns(_usuario);

            HashDto result = _usuarioService.EsquecerSenha(UserId).Result;

            Assert.NotNull(result.DataHoraCodigoSeguranca);
            Assert.NotNull(result.CodigoSeguranca);
        }

        [Fact(DisplayName = "AlterarSenhaViaHash - Quando a função for chamada - Deve retornar um valor booleano de confirmação da exclusão usuário")]
        public async Task AlterarSenhaViaHash()
        {
            Guid UserId = Guid.NewGuid();
            var enderecoId = Guid.NewGuid();
            EsquecerSenhaDto _body = new EsquecerSenhaDto
            {
                HashDeSeguranca = "72a6a75d-f8a2-410a-b774-db133019d18d",
                NovaSenha = "123445566789876543"
            };
            _usuarioResultMock.CodigoSeguranca = new Guid(_body.HashDeSeguranca);
            _usuarioResultMock.DataHoraCodigoSeguranca = DateTime.Now;
            Task<Usuario> _usuario = Task.Run(() => _usuarioResultMock);
            Task<Usuario> _usuario2 = Task.Run(() => _usuarioResultMock);
            _usuarioRepository.Setup(s => s.buscarUsuario(UserId)).Returns(_usuario2);
            _usuarioRepository.Setup(s => s.AtualizarUsuario(_usuarioResultMock)).Returns(_usuario);

            string result = _usuarioService.AlterarSenhaViaHash(UserId, _body).Result;

            Assert.Equal("Senha atualizada com sucesso.", result);
        }
    }
}