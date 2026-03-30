using GestaoDeRestaurante.DTOs.ItemCardapio;
using GestaoDeRestaurante.Services;
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
        public async Task<IActionResult> CriarItemCardapio(ItemCardapioRequestDTO dto)
            => Ok(await _service.CriarItemCardapio(dto));

        [HttpGet]
        public async Task<IActionResult> ListarItensCardapio()
            => Ok(await _service.ListarItensCardapio());

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarItemCardapioPorId(int id)
            => Ok(await _service.BuscarItemCardapioPorId(id));

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarItemCardapio(int id, ItemCardapioRequestDTO dto)
            => Ok(await _service.AtualizarItemCardapio(id, dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarItemCardapio(int id)
            => Ok(await _service.DeletarItemCardapio(id));
    }
}