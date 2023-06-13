using Application.Interfaces;
using Domain.Dto;
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
                if (ex.InnerException.Message.StartsWith("Duplicate"))
                {
                    return BadRequest("Algum item único está sendo duplicado.");
                }
                  return UnprocessableEntity(ex.ToString());
            }
        }

        [HttpGet("usuarios")]
        public async Task<IActionResult> ListarUsuarios()
        {
            try
            {
                var listarUsuarios = await _usuarioService.ListarUsuarios();
                return Ok(listarUsuarios);
            } catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }            
        }
        [HttpGet("usuarios/{idusuario}")]
        public async Task<IActionResult> DetalharUsuario(Guid idusuario)
        {
            try
            {
                var detalharUsuario = await _usuarioService.DetalharUsuario(idusuario);
                return Ok(detalharUsuario);
            }
            catch (Exception ex)
            {
                return NotFound(ex.ToString());
            }
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
                return NotFound(ex.ToString());
            }        
        }

        [HttpPatch("usuarios/{idusuario}")]
        public async Task<IActionResult> AtualizarUsuario(Guid idUsuario, [FromBody] PatchUsuarioDto usuarioDto)
        {
            try
            {
                var atualizarUsuario = await _usuarioService.AtualizarUsuario(idUsuario, usuarioDto);
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

        [HttpGet("usuarios/{idusuario}/senhas")]
        public async Task<IActionResult> EsquecerSenha(Guid idusuario)
        {
            try
            {
                var hashDeSeguranca = await _usuarioService.EsquecerSenha(idusuario);
                return Ok(hashDeSeguranca);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPost("usuarios/{idusuario}/senhas")]
        public async Task<IActionResult> AlterarSenhaViaHash(Guid idusuario, [FromBody] EsquecerSenhaDto esquecerSenhaDto)
        {
            try
            {
                var stringResposta = await _usuarioService.AlterarSenhaViaHash(idusuario, esquecerSenhaDto);
                return Ok(stringResposta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}

