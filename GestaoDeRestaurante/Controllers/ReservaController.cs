using GestaoDeRestaurante.DTOs.Reserva;
using GestaoDeRestaurante.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestaoDeRestaurante.Controllers
{
    [ApiController]
    [Route("api/usuarios/{usuarioId}/reservas")]
    [Authorize]
    public class ReservaController : ControllerBase
    {
        private readonly ReservaService _service;

        public ReservaController(ReservaService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CriarReserva(int usuarioId, ReservaRequestDTO dto)
        {
            try
            {
                if (!UsuarioAutorizado(usuarioId))
                    return Forbid();

                return Ok(await _service.CriarReserva(usuarioId, dto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarReservas(int usuarioId)
        {
            try
            {
                if (!UsuarioAutorizado(usuarioId))
                    return Forbid();

                return Ok(await _service.ListarReservasPorUsuario(usuarioId));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarReservaPorId(int usuarioId, int id)
        {
            try
            {
                if (!UsuarioAutorizado(usuarioId))
                    return Forbid();

                return Ok(await _service.BuscarReservaPorId(id));
            }
            catch (Exception ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> CancelarReserva(int usuarioId, int id)
        {
            try
            {
                if (!UsuarioAutorizado(usuarioId))
                    return Forbid();

                await _service.CancelarReserva(id);
                return Ok(new { mensagem = "Reserva cancelada com sucesso." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        private bool UsuarioAutorizado(int usuarioId)
        {
            if (User.IsInRole("Administrador"))
                return true;

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userIdClaim != null && int.Parse(userIdClaim) == usuarioId;
        }
    }
}
