using GestaoDeRestaurante.DTOs.Reserva;
using GestaoDeRestaurante.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            => Ok(await _service.CriarReserva(usuarioId, dto));

        [HttpGet]
        public async Task<IActionResult> ListarReserva()
            => Ok(await _service.ListarReserva());

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarReservaPorId(int id)
            => Ok(await _service.BuscarReservaPorId(id));

        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> CancelarReserva(int id)
            => Ok(await _service.CancelarReserva(id));
    }
}