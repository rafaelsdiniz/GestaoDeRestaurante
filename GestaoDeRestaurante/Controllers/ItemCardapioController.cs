using GestaoDeRestaurante.DTOs.ItemCardapio;
using GestaoDeRestaurante.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemCardapioController : ControllerBase
    {
        private readonly ItemCardapioService _service;

        public ItemCardapioController(ItemCardapioService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CriarItemCardapio(ItemCardapioRequestDTO dto)
        {
            try
            {
                return Ok(await _service.CriarItemCardapio(dto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarItensCardapio()
        {
            try
            {
                return Ok(await _service.ListarItensCardapio());
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarItemCardapioPorId(int id)
        {
            try
            {
                return Ok(await _service.BuscarItemCardapioPorId(id));
            }
            catch (Exception ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AtualizarItemCardapio(int id, ItemCardapioRequestDTO dto)
        {
            try
            {
                return Ok(await _service.AtualizarItemCardapio(id, dto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeletarItemCardapio(int id)
        {
            try
            {
                await _service.DeletarItemCardapio(id);
                return Ok(new { mensagem = "Item do cardápio removido com sucesso." });
            }
            catch (Exception ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
    }
}
