using GestaoDeRestaurante.DTOs.Pedido;
using GestaoDeRestaurante.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        {
            try
            {
                var idAutenticado = ObterUsuarioIdDoToken();
                if (idAutenticado == null)
                    return Forbid();

                // Admin pode criar pedido para qualquer usuário; cliente usa seu próprio ID
                var idEfetivo = User.IsInRole("Administrador") ? usuarioId : idAutenticado.Value;

                return Ok(await _service.CriarPedido(idEfetivo, dto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarPedidos(int usuarioId)
        {
            try
            {
                var idAutenticado = ObterUsuarioIdDoToken();
                if (idAutenticado == null)
                    return Forbid();

                var idEfetivo = User.IsInRole("Administrador") ? usuarioId : idAutenticado.Value;

                return Ok(await _service.ListarPedidosPorUsuario(idEfetivo));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPedidoPorId(int usuarioId, int id)
        {
            try
            {
                var idAutenticado = ObterUsuarioIdDoToken();
                if (idAutenticado == null)
                    return Forbid();

                return Ok(await _service.BuscarPedidoPorId(id));
            }
            catch (Exception ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        private int? ObterUsuarioIdDoToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userIdClaim != null ? int.Parse(userIdClaim) : null;
        }

        private bool UsuarioAutorizado(int usuarioId)
        {
            if (User.IsInRole("Administrador"))
                return true;

            var idAutenticado = ObterUsuarioIdDoToken();
            return idAutenticado != null && idAutenticado.Value == usuarioId;
        }
    }
}
