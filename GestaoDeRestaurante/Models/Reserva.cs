using GestaoDeRestaurante.Enums;
using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.Models
{
    public class Reserva : BaseEntity
    {
        [Required]
        public DateTime DataHoraReserva { get; set; }

        public int QuantidadePessoas { get; set; }
        public StatusReserva StatusReserva { get; set; }

        [MaxLength(8)]
        public string? CodigoConfirmacao { get; set; }

        public int UsuarioID { get; set; }
        public Usuario? Usuario { get; set; }
        public int MesaId { get; set; }
        public Mesa? Mesa { get; set; }
    }
}
