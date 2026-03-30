using GestaoDeRestaurante.DTOs.Atendimento;
using GestaoDeRestaurante.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtendimentoController : ControllerBase
    {
        private readonly AtendimentoService _service;

        public AtendimentoController(AtendimentoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CriarAtendimento(AtendimentoRequestDTO dto)
            => Ok(await _service.CriarAtendimento(dto));

        [HttpGet]
        public async Task<IActionResult> ListarAtendimentos()
            => Ok(await _service.ListarAtendimentos());

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarAtendimentoPorId(int id)
            => Ok(await _service.BuscarAtendimentoPorId(id));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarAtendimento(int id)
            => Ok(await _service.DeletarAtendimento(id));
    }
}