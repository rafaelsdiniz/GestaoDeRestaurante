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
            => Ok(await _service.CriarSugestaoDoChefe(dto));

        [HttpGet]
        public async Task<IActionResult> ListarSugestoesDoChefe()
            => Ok(await _service.ListarSugestoesDoChefe());

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarSugestaoDoChefePorId(int id)
            => Ok(await _service.BuscarSugestaoDoChefePorId(id));

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AtualizarSugestaoDoChefe(int id, SugestaoChefeRequestDTO dto)
            => Ok(await _service.AtualizarSugestaoDoChefe(id, dto));

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeletarSugestaoDoChefe(int id)
            => Ok(await _service.DeletarSugestaoDoChefe(id));
    }
}