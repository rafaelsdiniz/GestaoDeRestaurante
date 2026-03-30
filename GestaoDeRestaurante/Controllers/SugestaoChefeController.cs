using GestaoDeRestaurante.DTOs.SugestaoChefe;
using GestaoDeRestaurante.Services;
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
        public async Task<IActionResult> CriarSugestaoDoChefe(SugestaoChefeRequestDTO dto)
            => Ok(await _service.CriarSugestaoDoChefe(dto));

        [HttpGet]
        public async Task<IActionResult> ListarSugestoesDoChefe()
            => Ok(await _service.ListarSugestoesDoChefe());

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarSugestaoDoChefePorId(int id)
            => Ok(await _service.BuscarSugestaoDoChefePorId(id));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarSugestaoDoChefe(int id)
            => Ok(await _service.DeletarSugestaoDoChefe(id));
    }
}