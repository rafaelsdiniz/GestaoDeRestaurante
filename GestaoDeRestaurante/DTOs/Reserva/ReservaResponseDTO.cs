using GestaoDeRestaurante.Enums;

namespace GestaoDeRestaurante.DTOs.Reserva
{
    public class ReservaResponseDTO
    {
        public int Id { get; set; }
        public DateTime DataHoraReserva { get; set; }
        public int QuantidadePessoas { get; set; }
        public StatusReserva StatusReserva { get; set; }

        public string NomeUsuario { get; set; }
        public int NumeroMesa { get; set; }
    }
}