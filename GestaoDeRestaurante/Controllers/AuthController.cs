using GestaoDeRestaurante.DTOs.Auth;
using GestaoDeRestaurante.DTOs.Usuario;
using GestaoDeRestaurante.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly UsuarioService _usuarioService;

        public AuthController(AuthService authService, UsuarioService usuarioService)
        {
            _authService = authService;
            _usuarioService = usuarioService;
        }

        /// <summary>Realiza login e retorna o token JWT.</summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO dto)
        {
            try
            {
                var response = await _authService.Login(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { mensagem = ex.Message });
            }
        }

        /// <summary>Cadastra um novo usuário.</summary>
        [HttpPost("cadastro")]
        public async Task<IActionResult> Cadastro([FromBody] UsuarioRequestDTO dto)
        {
            try
            {
                var response = await _usuarioService.CriarUsuario(dto);
                return Created($"/api/usuario/{response.Id}", response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
