using GestaoDeRestaurante.DTOs.Mesa;
using GestaoDeRestaurante.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeRestaurante.Controllers
{
    [ApiController]
    [Route("api/mesa")]
    public class MesaController : ControllerBase
    {
        private readonly MesaService _service;

        public MesaController(MesaService service)
        {
            _service = service;
        }

        [HttpGet("disponiveis")]
        [AllowAnonymous]
        public async Task<IActionResult> ListarMesasDisponiveis()
        {
            try
            {
                return Ok(await _service.ListarMesasDisponiveis());
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CriarMesa(MesaRequestDTO dto)
        {
            try
            {
                return Ok(await _service.CriarMesa(dto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ListarMesas()
        {
            try
            {
                return Ok(await _service.ListarMesas());
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> BuscarMesaPorId(int id)
        {
            try
            {
                return Ok(await _service.BuscarMesaPorId(id));
            }
            catch (Exception ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AtualizarMesa(int id, MesaRequestDTO dto)
        {
            try
            {
                return Ok(await _service.AtualizarMesa(id, dto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeletarMesa(int id)
        {
            try
            {
                await _service.DeletarMesa(id);
                return Ok(new { mensagem = "Mesa removida com sucesso." });
            }
            catch (Exception ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
    }
}
