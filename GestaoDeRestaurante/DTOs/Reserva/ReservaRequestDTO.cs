using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.DTOs.Reserva
{
    public class ReservaRequestDTO
    {
        [Required]
        public DateTime DataHoraReserva { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "A quantidade de pessoas deve ser entre 1 e 100.")]
        public int QuantidadePessoas { get; set; }

        [Required]
        public int MesaId { get; set; }
    }
}
