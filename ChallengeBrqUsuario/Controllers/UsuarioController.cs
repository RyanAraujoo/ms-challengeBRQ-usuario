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
                if (ex.InnerException.Message.StartsWith("Duplicate"))
                {
                    return BadRequest("Algum item único está sendo duplicado..");
                }
                return UnprocessableEntity("Não foi possível processar o Body.");
            }       
        }
    }
}

