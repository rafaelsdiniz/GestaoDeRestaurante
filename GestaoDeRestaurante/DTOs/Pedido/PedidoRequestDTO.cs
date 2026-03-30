using GestaoDeRestaurante.Enums;
using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.DTOs.Pedido
{
    public class PedidoRequestDTO
    {
        [Required]
        public Periodo Periodo { get; set; }

        [Required]
        public int AtendimentoId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "O pedido deve ter pelo menos 1 item.")]
        public List<int> ItensIds { get; set; }
    }
}