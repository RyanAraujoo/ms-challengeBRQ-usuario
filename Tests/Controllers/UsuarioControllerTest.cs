using Application.InputModels;
using Application.Interfaces;
using Application.ViewModels;
using Domain.Dto;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Xunit;

namespace Tests.Controllers
{
    public class UsuarioControllerTest
    {
        private UsuarioController _usuarioController;
        private Guid enderecoId;
        private IActionResult resultado;
        private readonly Mock<IUsuarioService> _usuarioService;
        private readonly Usuario _usuarioResultMock;
        private readonly UsuarioInputModel _usuarioInputMock;
        private readonly UsuarioViewModel _usuarioViewModelMock;

        public UsuarioControllerTest()
        {
            _usuarioService = new Mock<IUsuarioService>();
            _usuarioController = new UsuarioController(_usuarioService.Object);
            enderecoId = Guid.NewGuid();
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
        }
        [Fact(DisplayName = "CadastrarUsuario - Quando a função for chamada - Deve criar um Usuário")]

        public async Task CadastrarUsuarioCriado()
        {
            Task<UsuarioViewModel> _usuario = Task.Run(() => _usuarioViewModelMock);
            _usuarioService.Setup(s => s.CadastrarUsuario(It.IsAny<UsuarioInputModel>())).Returns(_usuario);
            IActionResult resultado = _usuarioController.CadastrarUsuario(_usuarioInputMock).Result;
            Assert.IsType<CreatedResult>(resultado);
            CreatedResult createdResult = (CreatedResult)resultado;
            Assert.Equal(201, createdResult.StatusCode);
        }
        [Fact(DisplayName = "CadastrarUsuario - Quando a api CEP retornar logradouro vazio - Deve lançar exception")]
        public async Task CadastrarUsuarioExceptionLogradouro()
        {
            _usuarioService.Setup(s => s.CadastrarUsuario(It.IsAny<UsuarioInputModel>())).Throws(new Exception("Não foi possível identificar o Logradouro. Informe, por favor."));
            var resultado = await _usuarioController.CadastrarUsuario(_usuarioInputMock);
            Assert.IsType<UnprocessableEntityObjectResult>(resultado);
        }

        [Fact(DisplayName = "ListarUsuario - Quando a função for chamada - Deve retornar uma lista de usuários simplificado")]
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
            _usuarioService.Setup(s => s.ListarUsuarios()).Returns(_usuario);

            IActionResult resultado = _usuarioController.ListarUsuarios().Result;

            Assert.IsType<OkObjectResult>(resultado);
            OkObjectResult createdResult = (OkObjectResult)resultado;
            Assert.Equal(200, createdResult.StatusCode);
        }

        [Fact(DisplayName = "ListarUsuario - Quando a função for chamada e dá erro sistemico - Deve retornar uma exception")]
        public async Task ListarUsuariosFalha()
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

            _usuarioService.Setup(s => s.ListarUsuarios()).Throws(new Exception("Erro 500"));

            IActionResult resultado = await _usuarioController.ListarUsuarios();

            Assert.IsType<StatusCodeResult>(resultado);
            var statusCodeResult = (StatusCodeResult)resultado;
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact(DisplayName = "DetalharUsuario - Quando a função for chamada - Deve retornar o usuário detalhado")]
        public async Task DetalharUsuario()
        {
            Task<Usuario> _usuario = Task.Run(() => _usuarioResultMock);
            _usuarioService.Setup(s => s.DetalharUsuario(_usuarioResultMock.Id)).Returns(_usuario);
            IActionResult resultado = _usuarioController.DetalharUsuario(It.IsAny<Guid>()).Result;
            Assert.IsType<OkObjectResult>(resultado);
            OkObjectResult OkResult = (OkObjectResult)resultado;
            Assert.Equal(200, OkResult.StatusCode);
        }

        [Fact(DisplayName = "DetalharUsuario - Quando o usuário não for encontrado - Deve retornar exceptions")]
        public async Task DetalharUsuarioFalha()
        {
            _usuarioService.Setup(s => s.DetalharUsuario(_usuarioResultMock.Id)).Throws(new Exception("Usuário não encontrado."));
            resultado = await _usuarioController.DetalharUsuario(_usuarioResultMock.Id);
            Assert.IsType<NotFoundObjectResult>(resultado);
        }

        [Fact(DisplayName = "ExcluirUsuario - Quando a função for chamada - Deve remover o usuário especificado")]
        public async Task ExcluirUsuario()
        {
            Guid UserId = Guid.NewGuid();

            Task<bool> _usuario = Task.Run(() => true);

            _usuarioService.Setup(s => s.ExcluirUsuario(UserId)).Returns(_usuario);

            resultado = await _usuarioController.ExcluirUsuario(It.IsAny<Guid>());

            Assert.IsType<NoContentResult>(resultado);
        }

        [Fact(DisplayName = "ExcluirUsuario - Quando usuário não for encontrado - Deve retornar exceptions")]
        public async Task ExcluirUsuarioFalha()
        {
            Guid UserId = Guid.NewGuid();

            _usuarioService.Setup(s => s.ExcluirUsuario(UserId)).Throws(new Exception("Cliente não encontrado para exclusão."));

            resultado = await _usuarioController.ExcluirUsuario(UserId);

            Assert.IsType<NotFoundObjectResult>(resultado);
        }

        [Fact(DisplayName = "AtualizarUsuario - Quando a função for chamada - Deve atualizar o usuário informado")]
        public async Task AtualizarUsuario()
        {
            var patchUsuarioDto = new PatchUsuarioDto
            {
                Email = "joao@example.com",
            };

            Task<Usuario> _usuario = Task.Run(() => _usuarioResultMock);

            _usuarioService.Setup(s => s.AtualizarUsuario(_usuarioResultMock.Id, patchUsuarioDto)).Returns(_usuario);

            resultado = await _usuarioController.AtualizarUsuario(_usuarioResultMock.Id, patchUsuarioDto);

            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "AtualizarUsuario -  Quando usuário não for encontrado - Deve retornar exceptions")]
        public async Task AtualizarUsuarioFalha()
        {
            Guid UserId = Guid.NewGuid();
            var patchUsuarioDto = new PatchUsuarioDto
            {
                Email = "joao@example.com",
            };
            _usuarioService.Setup(s => s.AtualizarUsuario(_usuarioResultMock.Id, patchUsuarioDto)).Throws(new Exception("Usuario não identificado."));
            resultado = await _usuarioController.AtualizarUsuario(_usuarioResultMock.Id, patchUsuarioDto);
            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact(DisplayName = "TrocarSenha - Quando a função for chamada - Deve trocar a senha do usuário informado")]
        public async Task TrocarSenha()
        {
            Guid UserId = Guid.NewGuid();
            var trocarSenha = new TrocarSenhaDto
            {
                SenhaAtual = "senha_atual",
                SenhaNova = "senha_nova"
            };

            Task<string> _usuario = Task.Run(() => "Senha atualizada com sucesso.");
            _usuarioService.Setup(s => s.AlterarSenha(UserId,trocarSenha)).Returns(_usuario);

            IActionResult resultado = await _usuarioController.TrocarSenha(UserId, trocarSenha);

            Assert.IsType<NoContentResult>(resultado);
        }

        [Fact(DisplayName = "TrocarSenha - Quando a senha atual não bater - Deve cair em fluxo de erro")]
        public async Task TrocarSenhaFalha()
        {
            Guid UserId = Guid.NewGuid();
            var trocarSenha = new TrocarSenhaDto
            {
                SenhaAtual = "senha_atual_errada",
                SenhaNova = "senha_nova"
            };

            _usuarioService.Setup(s => s.AlterarSenha(UserId, trocarSenha)).Throws(new Exception("Senha atual incorreta."));

            IActionResult resultado = await _usuarioController.TrocarSenha(UserId, trocarSenha);

            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact(DisplayName = "EsquecerSenha - Quando a função for chamada - Deve criar um hash de segurança no usuário")]
        public async Task EsquecerSenha()
        {
            Guid UserId = Guid.NewGuid();
            var hashDto = new HashDto
            {
                CodigoSeguranca = Guid.NewGuid(),
                DataHoraCodigoSeguranca = DateTime.Now
            };

            Task<HashDto> _hashDto = Task.Run(() => hashDto);
            _usuarioService.Setup(s => s.EsquecerSenha(UserId)).Returns(_hashDto);

            IActionResult resultado = await _usuarioController.EsquecerSenha(UserId);

            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "EsquecerSenha - Quando o usuário for informado incorretamente - Deve cair em fluxo de erro")]
        public async Task EsquecerSenhaFalha()
        {
            Guid UserId = Guid.NewGuid();
            _usuarioService.Setup(s => s.EsquecerSenha(UserId)).Throws(new Exception("Usuario Não Existente/Usuário incorreto"));

            IActionResult resultado = await _usuarioController.EsquecerSenha(UserId);

            Assert.IsType<BadRequestObjectResult>(resultado);
        }

        [Fact(DisplayName = "AlterarSenhaViaHash - Quando a função for chamada - Deve alterar a senha do usuário")]
        public async Task AlterarSenhaViaHash()
        {
            Guid UserId = Guid.NewGuid();
            var hashDto = new EsquecerSenhaDto
            {
                HashDeSeguranca = "hash_de_seguranca",
                NovaSenha = "nova_senha"
            };

            Task<string> _hashDto = Task.Run(() => "Senha atualizada com sucesso.");
            _usuarioService.Setup(s => s.AlterarSenhaViaHash(UserId,hashDto)).Returns(_hashDto);

            IActionResult resultado = await _usuarioController.AlterarSenhaViaHash(UserId, hashDto);

            Assert.IsType<OkObjectResult>(resultado);
        }

        [Fact(DisplayName = "AlterarSenhaViaHash - Quando a hash de segurança for incorreta - Deve cair em fluxo de erro")]
        public async Task AlterarSenhaViaHashFalha()
        {
            Guid UserId = Guid.NewGuid();
            var hashDto = new EsquecerSenhaDto
            {
                HashDeSeguranca = "hash_de_seguranca",
                NovaSenha = "nova_senha"
            };

            _usuarioService.Setup(s => s.AlterarSenhaViaHash(UserId, hashDto)).Throws(new Exception("Código de Segurança Inválido!"));

            IActionResult resultado = await _usuarioController.AlterarSenhaViaHash(UserId, hashDto);

            Assert.IsType<BadRequestObjectResult>(resultado);
        }
    }
}