using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.Models
{
    public class Mesa : BaseEntity
    {
        [Required]
        public int Numero {  get; set; }

        [Required]
        public int Capacidade { get; set; }

        public List<Reserva> Reservas { get; set; } = new();
    }
}
