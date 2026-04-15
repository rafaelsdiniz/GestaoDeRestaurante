using GestaoDeRestaurante.DTOs.Atendimento;
using GestaoDeRestaurante.Enums;
using GestaoDeRestaurante.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class AtendimentoController : ControllerBase
    {
        private readonly AtendimentoService _service;

        public AtendimentoController(AtendimentoService service)
        {
            _service = service;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CriarAtendimento(AtendimentoRequestDTO dto)
        {
            try
            {
                // Delivery exige usuário logado
                if (dto.TipoAtendimento != TipoAtendimento.AtendimentoPresencial
                    && !User.Identity.IsAuthenticated)
                {
                    return Unauthorized(new { mensagem = "Para pedidos delivery é necessário estar logado." });
                }

                var resultado = await _service.CriarAtendimento(dto);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarAtendimentos()
        {
            try
            {
                return Ok(await _service.ListarAtendimentos());
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarAtendimentoPorId(int id)
        {
            try
            {
                return Ok(await _service.BuscarAtendimentoPorId(id));
            }
            catch (Exception ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarAtendimento(int id, AtendimentoRequestDTO dto)
        {
            try
            {
                return Ok(await _service.AtualizarAtendimento(id, dto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarAtendimento(int id)
        {
            try
            {
                await _service.DeletarAtendimento(id);
                return Ok(new { mensagem = "Atendimento removido com sucesso." });
            }
            catch (Exception ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
    }
}
