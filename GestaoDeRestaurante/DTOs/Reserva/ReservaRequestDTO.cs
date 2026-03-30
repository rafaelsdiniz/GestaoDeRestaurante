using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.DTOs.Reserva
{
    public class ReservaRequestDTO
    {
        [Required]
        public DateTime DataHoraReserva { get; set; }

        [Required]
        public int QuantidadePessoas { get; set; }

        [Required]
        public int MesaId { get; set; }
    }
}