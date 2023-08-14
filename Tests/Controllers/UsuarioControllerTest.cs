using Application.Interfaces;
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

        public UsuarioControllerTest()
        {
            _usuarioService = new Mock<IUsuarioService>();
            _usuarioController = new UsuarioController(_usuarioService.Object);
            enderecoId = Guid.NewGuid();
        }
        [Fact(DisplayName = "CadastrarUsuario - Quando a função for chamada - Deve criar um Usuário")]

        public async Task CadastrarUsuarioCriado()
        {
            var mockUsuarioResult = new Usuario
            {
                Id = Guid.NewGuid(),
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
            var mockUsuarioDtoParaService = new UsuarioDto
            {
                Cpf = "12345678900",
                Email = "exemplo@email.com",
                DataDeNascimento = "1990-01-01",
                Sexo = "M",
                NomeCompleto = "Fulano de Tal",
                Senha = "senha123",
                Apelido = "fulano",
                Telefone = "1234567890",
                Endereco = new EnderecoDto
                {
                    Complemento = "Complemento Exemplo",
                    Numero = "123",
                    Cep = "45140000",
                    Bairro = "Bairro Exemplo",
                    Logradouro = "Rua Exemplo"
                },
            };

            Task<Usuario> _usuario = Task.Run(() => mockUsuarioResult);
            _usuarioService.Setup(s => s.CadastrarUsuario(It.IsAny<UsuarioDto>())).Returns(_usuario);


            IActionResult resultado = _usuarioController.CadastrarUsuario(mockUsuarioDtoParaService).Result;

            Assert.IsType<CreatedResult>(resultado);
            CreatedResult createdResult = (CreatedResult)resultado;
            Assert.Equal(201, createdResult.StatusCode);
        }
        [Fact(DisplayName = "CadastrarUsuario - Quando a api CEP retornar logradouro vazio - Deve lançar exception")]
        public async Task CadastrarUsuarioExceptionLogradouro()
        {
            var mockUsuarioDtoParaService = new UsuarioDto
            {
                Cpf = "12345678900",
                Email = "exemplo@email.com",
                DataDeNascimento = "1990-01-01",
                Sexo = "M",
                NomeCompleto = "Fulano de Tal",
                Senha = "senha123",
                Apelido = "fulano",
                Telefone = "1234567890",
                Endereco = new EnderecoDto
                {
                    Complemento = "Complemento Exemplo",
                    Numero = "123",
                    Cep = "45140000",
                    Bairro = "Bairro Exemplo",
                    Logradouro = "Rua Exemplo"
                },
            };

            _usuarioService.Setup(s => s.CadastrarUsuario(It.IsAny<UsuarioDto>())).Throws(new Exception("Não foi possível identificar o Logradouro. Informe, por favor."));

            // Chama a ação do controlador e captura o resultado
            var resultado = await _usuarioController.CadastrarUsuario(mockUsuarioDtoParaService);

            // Verifica o tipo do resultado
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
            _usuarioService.Setup(s => s.DetalharUsuario(UserId)).Returns(_usuario);

            IActionResult resultado = _usuarioController.DetalharUsuario(It.IsAny<Guid>()).Result;

            Assert.IsType<OkObjectResult>(resultado);
            OkObjectResult OkResult = (OkObjectResult)resultado;
            Assert.Equal(200, OkResult.StatusCode);
        }

        [Fact(DisplayName = "DetalharUsuario - Quando o usuário não for encontrado - Deve retornar exceptions")]
        public async Task DetalharUsuarioFalha()
        {
            Guid UserId = Guid.NewGuid();

            _usuarioService.Setup(s => s.DetalharUsuario(UserId)).Throws(new Exception("Usuário não encontrado."));

            resultado = await _usuarioController.DetalharUsuario(UserId);

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
            Guid UserId = Guid.NewGuid();
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

            _usuarioService.Setup(s => s.AtualizarUsuario(UserId, patchUsuarioDto)).Returns(_usuario);

            resultado = await _usuarioController.AtualizarUsuario(UserId, patchUsuarioDto);

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

            _usuarioService.Setup(s => s.AtualizarUsuario(UserId, patchUsuarioDto)).Throws(new Exception("Usuario não identificado."));

            resultado = await _usuarioController.AtualizarUsuario(UserId, patchUsuarioDto);

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