using GestaoDeRestaurante.DTOs.Endereco;
using GestaoDeRestaurante.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            => Ok(await _service.CriarEndereco(usuarioId, dto));

        [HttpGet]
        public async Task<IActionResult> ListarEnderecoPorUsuario(int usuarioId)
            => Ok(await _service.ListarEnderecoPorUsuario(usuarioId));

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarEnderecoPorId(int id)
            => Ok(await _service.BuscarEnderecoPorId(id));

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarEndereco(int id, EnderecoRequestDTO dto)
            => Ok(await _service.AtualizarEndereco(id, dto));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarEndereco(int id)
            => Ok(await _service.DeletarEndereco(id));
    }
}