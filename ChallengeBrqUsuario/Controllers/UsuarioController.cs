using Application.Interfaces;
using Domain.Dto;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("/challengebrq/v1/")]
    public class UsuarioController: ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService UsuarioService)
        {
            _usuarioService = UsuarioService;
        }

        [HttpPost("usuarios")]
        public async Task<IActionResult> CadastrarUsuario([FromBody] UsuarioDto usuarioDto)
        {
            try
            {
                var cadastrarUsuario = await _usuarioService.CadastrarUsuario(usuarioDto);
                return Created($"/{cadastrarUsuario.Id}", cadastrarUsuario);

            } catch (Exception ex)
            {
                return ex.InnerException.Message.StartsWith("Duplicate")
                    ? BadRequest("Algum item único está sendo duplicado..")
                    : UnprocessableEntity("Não foi possível processar o Body.");
            }
        }

        [HttpGet("usuarios")]
        public async Task<IActionResult> ListarUsuarios()
        {
                var listarUsuarios = await _usuarioService.ListarUsuarios();
                return Ok(listarUsuarios);
        }
        [HttpGet("usuarios/{idusuario}")]
        public async Task<IActionResult> DetalharUsuario(Guid idusuario)
        {
                var detalharUsuario = await _usuarioService.DetalharUsuario(idusuario);

                if (detalharUsuario == null)
            {
                return NotFound("Usuario não encontrado!");
            }
                return Ok(detalharUsuario);
        }

        [HttpDelete("usuarios/{idusuario}")]
        public async Task<IActionResult> ExcluirUsuario(Guid idUsuario)
        {
            try
            {
                var excluirUsuario = await _usuarioService.ExcluirUsuario(idUsuario);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound($"{ex.Message}");
            }        
        }

        [HttpPatch("usuarios/{idusuario}")]
        public async Task<IActionResult> AtualizarUsuario(Guid idUsuario, [FromBody] FromBodyPutUsuarioDto fromBodyPutUsuarioDto)
        {
            try
            {
                var atualizarUsuario = await _usuarioService.AtualizarUsuario(idUsuario, fromBodyPutUsuarioDto);
                return Ok(atualizarUsuario);
            } catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPut("usuarios/{idusuario}/senhas")]
        public async Task<IActionResult> TrocarSenha(Guid idUsuario, [FromBody] TrocarSenhaDto trocarSenhaDto)
        {
            try
            {
                var trocarSenha = await _usuarioService.AlterarSenha(idUsuario, trocarSenhaDto);
                return NoContent();
            } catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}

