using GestaoDeRestaurante.DTOs.SugestaoChefe;
using GestaoDeRestaurante.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SugestaoChefeController : ControllerBase
    {
        private readonly SugestaoChefeService _service;

        public SugestaoChefeController(SugestaoChefeService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CriarSugestaoDoChefe(SugestaoChefeRequestDTO dto)
        {
            try
            {
                return Ok(await _service.CriarSugestaoDoChefe(dto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarSugestoesDoChefe()
        {
            try
            {
                return Ok(await _service.ListarSugestoesDoChefe());
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarSugestaoDoChefePorId(int id)
        {
            try
            {
                return Ok(await _service.BuscarSugestaoDoChefePorId(id));
            }
            catch (Exception ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AtualizarSugestaoDoChefe(int id, SugestaoChefeRequestDTO dto)
        {
            try
            {
                return Ok(await _service.AtualizarSugestaoDoChefe(id, dto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeletarSugestaoDoChefe(int id)
        {
            try
            {
                await _service.DeletarSugestaoDoChefe(id);
                return Ok(new { mensagem = "Sugestão do chefe removida com sucesso." });
            }
            catch (Exception ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
    }
}
