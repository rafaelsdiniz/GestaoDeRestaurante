using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.DTOs.Admin
{
    public class AlterarStatusPedidoDTO
    {
        [Required]
        public string NovoStatus { get; set; } = string.Empty;
    }
}
