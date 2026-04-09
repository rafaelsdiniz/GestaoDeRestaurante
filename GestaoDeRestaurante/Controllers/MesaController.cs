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
            => Ok(await _service.ListarMesasDisponiveis());

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CriarMesa(MesaRequestDTO dto)
            => Ok(await _service.CriarMesa(dto));

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ListarMesas()
            => Ok(await _service.ListarMesas());

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> BuscarMesaPorId(int id)
            => Ok(await _service.BuscarMesaPorId(id));

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AtualizarMesa(int id, MesaRequestDTO dto)
            => Ok(await _service.AtualizarMesa(id, dto));

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeletarMesa(int id)
            => Ok(await _service.DeletarMesa(id));
    }
}
