using GestaoDeRestaurante.DTOs.Usuario;
using GestaoDeRestaurante.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestaoDeRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CriarUsuario(UsuarioRequestDTO dto)
        {
            try
            {
                return Ok(await _service.CriarUsuario(dto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ListarUsuarios()
        {
            try
            {
                return Ok(await _service.ListarUsuarios());
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> BuscarUsuarioPorId(int id)
        {
            try
            {
                if (!UsuarioAutorizado(id))
                    return Forbid();

                return Ok(await _service.BuscarUsuarioPorId(id));
            }
            catch (Exception ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> AtualizarUsuario(int id, UsuarioRequestDTO dto)
        {
            try
            {
                if (!UsuarioAutorizado(id))
                    return Forbid();

                return Ok(await _service.AtualizarUsuario(id, dto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeletarUsuario(int id)
        {
            try
            {
                await _service.DeletarUsuario(id);
                return Ok(new { mensagem = "Usuário removido com sucesso." });
            }
            catch (Exception ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        private bool UsuarioAutorizado(int id)
        {
            if (User.IsInRole("Administrador"))
                return true;

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userIdClaim != null && int.Parse(userIdClaim) == id;
        }
    }
}
