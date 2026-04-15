using GestaoDeRestaurante.DTOs.Endereco;
using GestaoDeRestaurante.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestaoDeRestaurante.Controllers
{
    [ApiController]
    [Route("api/usuarios/{usuarioId}/enderecos")]
    [Authorize]
    public class EnderecoController : ControllerBase
    {
        private readonly EnderecoService _service;

        public EnderecoController(EnderecoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CriarEndereco(int usuarioId, EnderecoRequestDTO dto)
        {
            try
            {
                if (!UsuarioAutorizado(usuarioId))
                    return Forbid();

                var resultado = await _service.CriarEndereco(usuarioId, dto);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarEnderecoPorUsuario(int usuarioId)
        {
            try
            {
                if (!UsuarioAutorizado(usuarioId))
                    return Forbid();

                return Ok(await _service.ListarEnderecoPorUsuario(usuarioId));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarEnderecoPorId(int usuarioId, int id)
        {
            try
            {
                if (!UsuarioAutorizado(usuarioId))
                    return Forbid();

                return Ok(await _service.BuscarEnderecoPorId(id));
            }
            catch (Exception ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarEndereco(int usuarioId, int id, EnderecoRequestDTO dto)
        {
            try
            {
                if (!UsuarioAutorizado(usuarioId))
                    return Forbid();

                return Ok(await _service.AtualizarEndereco(id, dto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarEndereco(int usuarioId, int id)
        {
            try
            {
                if (!UsuarioAutorizado(usuarioId))
                    return Forbid();

                await _service.DeletarEndereco(id);
                return Ok(new { mensagem = "Endereço removido com sucesso." });
            }
            catch (Exception ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        private bool UsuarioAutorizado(int usuarioId)
        {
            if (User.IsInRole("Administrador"))
                return true;

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userIdClaim != null && int.Parse(userIdClaim) == usuarioId;
        }
    }
}
