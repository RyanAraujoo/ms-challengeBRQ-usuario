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
            var cadastrarUsuario = await _usuarioService.CadastrarUsuario(usuarioDto);

            if (cadastrarUsuario is null)
            {
                return UnprocessableEntity("");
            }
            // return Created("/challengebrq/v1/usuarios", usuarioDto);
               return Created($"/{cadastrarUsuario.Id}", cadastrarUsuario);
        }
    }
}

