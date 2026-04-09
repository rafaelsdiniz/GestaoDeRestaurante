using GestaoDeRestaurante.DTOs.Pedido;
using GestaoDeRestaurante.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeRestaurante.Controllers
{
    [ApiController]
    [Route("api/usuarios/{usuarioId}/pedidos")]
    [Authorize]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _service;

        public PedidoController(PedidoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CriarPedido(int usuarioId, PedidoRequestDTO dto)
            => Ok(await _service.CriarPedido(usuarioId, dto));

        [HttpGet]
        public async Task<IActionResult> ListarPedidos()
            => Ok(await _service.ListarPedidos());

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPedidoPorId(int id)
            => Ok(await _service.BuscarPedidoPorId(id));
    }
}