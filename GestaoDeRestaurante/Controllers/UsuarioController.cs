using GestaoDeRestaurante.DTOs.Usuario;
using GestaoDeRestaurante.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario(UsuarioRequestDTO dto)
            => Ok(await _service.CriarUsuario(dto));

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ListarUsuarios()
            => Ok(await _service.ListarUsuarios());

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> BuscarUsuarioPorId(int id)
            => Ok(await _service.BuscarUsuarioPorId(id));

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> AtualizarUsuario(int id, UsuarioRequestDTO dto)
            => Ok(await _service.AtualizarUsuario(id, dto));
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeletarUsuario(int id)
            => Ok(await _service.DeletarUsuario(id));
    }
}