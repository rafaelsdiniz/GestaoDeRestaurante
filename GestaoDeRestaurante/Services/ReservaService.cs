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

            // Busca configuração do restaurante
            var config = await _context.ConfiguracoesRestaurante.FirstOrDefaultAsync();
            var reservaInicio = config != null ? TimeSpan.Parse(config.ReservaInicio) : new TimeSpan(11, 0, 0);
            var reservaFim = config != null ? TimeSpan.Parse(config.ReservaFim) : new TimeSpan(14, 0, 0);
            var antecedenciaMinima = config?.AntecedenciaMinimaDias ?? 1;

            var horaReserva = dto.DataHoraReserva.TimeOfDay;
            if (horaReserva < reservaInicio || horaReserva >= reservaFim)
                throw new Exception($"Reservas só podem ser feitas entre {reservaInicio:hh\\:mm} e {reservaFim:hh\\:mm}.");

            // Antecedência mínima
            if (dto.DataHoraReserva.Date < DateTime.Today.AddDays(antecedenciaMinima))
                throw new Exception($"A reserva deve ser feita com pelo menos {antecedenciaMinima} dia(s) de antecedência.");

            if (dto.QuantidadePessoas > mesa.Capacidade)
                throw new Exception($"Capacidade da mesa excedida. Máximo: {mesa.Capacidade} pessoas.");

            // Verifica se a mesa já possui reserva ativa no mesmo dia (período de almoço)
            var mesaOcupada = await _context.Reservas.AnyAsync(r =>
                r.MesaId == dto.MesaId &&
                r.DataHoraReserva.Date == dto.DataHoraReserva.Date &&
                r.StatusReserva != StatusReserva.Cancelada
            );

            if (mesaOcupada)
                throw new Exception("Mesa já reservada para este dia.");

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

            return MapToResponse(reserva, usuario.Nome, mesa.Numero);
        }

        public async Task<List<ReservaResponseDTO>> ListarReservasPorUsuario(int usuarioId)
        {
            var reservas = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Mesa)
                .Where(r => r.UsuarioID == usuarioId)
                .OrderByDescending(r => r.DataHoraReserva)
                .ToListAsync();

            return reservas.Select(r => MapToResponse(r, r.Usuario!.Nome, r.Mesa!.Numero)).ToList();
        }

        public async Task<ReservaResponseDTO> BuscarReservaPorId(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Mesa)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                throw new Exception("Reserva não encontrada.");

            return MapToResponse(reserva, reserva.Usuario!.Nome, reserva.Mesa!.Numero);
        }

        public async Task<bool> CancelarReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
                throw new Exception("Reserva não encontrada.");

            if (reserva.StatusReserva == StatusReserva.Cancelada)
                throw new Exception("Esta reserva já está cancelada.");

            if (reserva.StatusReserva == StatusReserva.Finalizada)
                throw new Exception("Não é possível cancelar uma reserva já finalizada.");

            reserva.StatusReserva = StatusReserva.Cancelada;
            await _context.SaveChangesAsync();

            return true;
        }

        private static ReservaResponseDTO MapToResponse(Reserva reserva, string nomeUsuario, int numeroMesa)
        {
            return new ReservaResponseDTO
            {
                Id = reserva.Id,
                DataHoraReserva = reserva.DataHoraReserva,
                QuantidadePessoas = reserva.QuantidadePessoas,
                StatusReserva = reserva.StatusReserva,
                CodigoConfirmacao = reserva.CodigoConfirmacao,
                NomeUsuario = nomeUsuario,
                NumeroMesa = numeroMesa
            };
        }
    }
}
