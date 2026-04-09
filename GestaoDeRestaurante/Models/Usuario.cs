using System.ComponentModel.DataAnnotations;
using GestaoDeRestaurante.Enums;

namespace GestaoDeRestaurante.Models
{
    public class Usuario : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Senha {  get; set; } = string.Empty;

        public TipoUsuario TipoUsuario { get; set; } = TipoUsuario.Cliente;

        public List<Endereco> Enderecos { get; set; } = new();
        public List<Pedido> Pedidos { get; set; } = new();
        public List<Reserva> Reservas { get; set; } = new();

    }
}
