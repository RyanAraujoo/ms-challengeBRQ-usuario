using Domain.Entity;
using Infrastructure.DataBase;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Xunit;

namespace Tests.Repository
{
    public class UsuarioRepositoryTest
    {
        private readonly ApiDbContext _apiDbContext;
        private DbContextOptions<ApiDbContext> _options;
        private readonly Usuario _usuarioMock;

        public UsuarioRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<ApiDbContext>()
           .UseInMemoryDatabase(databaseName: "ChallengeDbUsuarios")
           .Options;
            _usuarioMock = new Usuario
            {
                Cpf = "123.456.789-00",
                Email = "exemplo@gmail.com",
                NomeCompleto = "Fulano de Tal",
                Senha = "senha123",
                Apelido = "fulaninho",
                Telefone = "7799999999",
                Sexo = 1
            };
            _usuarioMock.AtrelarEnderecoAoUsuario("45140000","Bairro Exemplo","Logradouro Exemplo","Itambé","BA","","12");
            _usuarioMock.DefinirDataDeNascimento("1990-01-01");
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
                Task<Usuario> _usuarioDetalhado = _usuarioRepository.DetalharUsuario(_usuarioMock.Id);

                Assert.Equal(_usuarioMock.Cpf, _usuarioDetalhado.Result.Cpf);
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
                Task<bool> _usuarioremovido = _usuarioRepository.ExcluirUsuario(_usuarioMock.Id);
                Task<Usuario> _buscarUsuario = _usuarioRepository.buscarUsuario(_usuarioMock.Id);
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
                Task<Usuario> _buscarUsuario = _usuarioRepository.buscarUsuario(_usuarioMock.Id);

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
