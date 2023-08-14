using Domain.Dto;
using Domain.Entity;
using Infrastructure.DataBase;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Repository
{
    public class UsuarioRepositoryTest
    {
        private readonly ApiDbContext _apiDbContext;
        private DbContextOptions<ApiDbContext> _options;
        private Guid enderecoId;
        private Usuario _usuarioMock;
        private Guid idGuidUsuario;

        public UsuarioRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<ApiDbContext>()
           .UseInMemoryDatabase(databaseName: "ChallengeDbUsuarios")
           .Options;
            idGuidUsuario = Guid.NewGuid();
            _usuarioMock = new Usuario
            {
                Id = idGuidUsuario,
                EnderecoId = enderecoId,
                DataDeNascimento = new DateTime(1990, 1, 1),
                Cpf = "123.456.789-00",
                Email = "exemplo@gmail.com",
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
        }

        [Fact(DisplayName = "CadastrarUsuario - Quando a função for chamada - Deve salvar o usuário no banco de dados")]
        public async Task CadastrarUsuario()
        {
            using (var context = new ApiDbContext(_options))
            {
                UsuarioRepository _usuarioRepository = new UsuarioRepository(context);
                bool _usuarioSaved = await _usuarioRepository.CadastrarUsuario(_usuarioMock);
                Assert.Equal(true, _usuarioSaved);
            }
        }

        [Fact(DisplayName = "DetalharUsuario - Quando a função for chamada - Deve retornar o usuário detalhado no banco de dados")]
        public async Task DetalharUsuario()
        {
            using (var context = new ApiDbContext(_options))
            {
                UsuarioRepository _usuarioRepository = new UsuarioRepository(context);
                context.Usuarios.Add(_usuarioMock);
                context.SaveChanges();
                Task<Usuario> _usuarioDetalhado = _usuarioRepository.DetalharUsuario(idGuidUsuario);

                Assert.Equal("123.456.789-00", _usuarioDetalhado.Result.Cpf);
            }
        }

        [Fact(DisplayName = "ExcluirUsuario - Quando a função for chamada - Deve remover o usuário especificado no banco de dados")]

        public async Task ExcluirUsuario()
        {
            using (var context = new ApiDbContext(_options))
            {
                UsuarioRepository _usuarioRepository = new UsuarioRepository(context);
                context.Usuarios.Add(_usuarioMock);
                context.SaveChanges();
                Task<bool> _usuarioremovido = _usuarioRepository.ExcluirUsuario(idGuidUsuario);
                Task<Usuario> _buscarUsuario = _usuarioRepository.buscarUsuario(idGuidUsuario);
                Assert.Equal(true, _usuarioremovido.Result);
                Assert.Null(_buscarUsuario.Result);
            }
        }

        [Fact(DisplayName = "BuscarUsuario - Quando a função for chamada - Deve retornar o usuário existente")]
        public async Task BuscarUsuario()
        {
            using (var context = new ApiDbContext(_options))
            {
                UsuarioRepository _usuarioRepository = new UsuarioRepository(context);
                context.Usuarios.Add(_usuarioMock);
                context.SaveChanges();
                Task<Usuario> _buscarUsuario = _usuarioRepository.buscarUsuario(idGuidUsuario);

                Assert.Equal(_buscarUsuario.Result, _usuarioMock);
            }
        }

        [Fact(DisplayName = "BuscarEnderecoUsuario - Quando a função for chamada - Deve retornar o endereco do usuário existente")]
        public async Task BuscarEndereco()
        {
            using (var context = new ApiDbContext(_options))
            {
                UsuarioRepository _usuarioRepository = new UsuarioRepository(context);
                context.Usuarios.Add(_usuarioMock);
                context.SaveChanges();
                Task<Endereco> _buscarEndereco = _usuarioRepository.buscarEnderecoUsuario(_usuarioMock);

                Assert.IsType<Endereco>(_buscarEndereco.Result);
                Assert.Equal("45140000", _buscarEndereco.Result.Cep);
            }
        }

    }
}
