using GestaoDeRestaurante.Data;
using GestaoDeRestaurante.DTOs.Reserva;
using GestaoDeRestaurante.Enums;
using GestaoDeRestaurante.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeRestaurante.Services
{
    public class ReservaService
    {
        private readonly AppDbContext _context;

        public ReservaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ReservaResponseDTO> CriarReserva(int usuarioId, ReservaRequestDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
                throw new Exception("Usuário não encontrado.");

            var mesa = await _context.Mesas.FindAsync(dto.MesaId);
            if (mesa == null)
                throw new Exception("Mesa não encontrada.");

            // Reservas só são para jantar (19h–22h)
            var hora = dto.DataHoraReserva.Hour;
            if (hora < 19 || hora >= 22)
                throw new Exception("Reservas só podem ser feitas entre 19h e 22h (jantar).");

            // Antecedência mínima de 1 dia
            if (dto.DataHoraReserva.Date <= DateTime.Today)
                throw new Exception("A reserva deve ser feita com pelo menos 1 dia de antecedência.");

            if (dto.QuantidadePessoas > mesa.Capacidade)
                throw new Exception($"Capacidade da mesa excedida. Máximo: {mesa.Capacidade} pessoas.");

            var mesaOcupada = await _context.Reservas.AnyAsync(r =>
                r.MesaId == dto.MesaId &&
                r.DataHoraReserva == dto.DataHoraReserva &&
                r.StatusReserva != StatusReserva.Cancelada
            );

            if (mesaOcupada)
                throw new Exception("Mesa já reservada para este horário.");

            var codigoConfirmacao = Guid.NewGuid().ToString("N")[..8].ToUpper();

            var reserva = new Reserva
            {
                DataHoraReserva = dto.DataHoraReserva,
                QuantidadePessoas = dto.QuantidadePessoas,
                StatusReserva = StatusReserva.Confirmada,
                CodigoConfirmacao = codigoConfirmacao,
                UsuarioID = usuarioId,
                MesaId = dto.MesaId
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return await MapToResponse(reserva.Id);
        }

        public async Task<List<ReservaResponseDTO>> ListarReserva()
        {
            var reservas = await _context.Reservas.ToListAsync();
            var lista = new List<ReservaResponseDTO>();

            foreach (var r in reservas)
                lista.Add(await MapToResponse(r.Id));

            return lista;
        }

        public async Task<ReservaResponseDTO> BuscarReservaPorId(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
                throw new Exception("Reserva não encontrada.");

            return await MapToResponse(id);
        }

        public async Task<bool> CancelarReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
                throw new Exception("Reserva não encontrada.");

            reserva.StatusReserva = StatusReserva.Cancelada;
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<ReservaResponseDTO> MapToResponse(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Mesa)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                throw new Exception("Reserva não encontrada.");

            return new ReservaResponseDTO
            {
                Id = reserva.Id,
                DataHoraReserva = reserva.DataHoraReserva,
                QuantidadePessoas = reserva.QuantidadePessoas,
                StatusReserva = reserva.StatusReserva,
                CodigoConfirmacao = reserva.CodigoConfirmacao,
                NomeUsuario = reserva.Usuario!.Nome,
                NumeroMesa = reserva.Mesa!.Numero
            };
        }
    }
}
