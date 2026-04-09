using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.DTOs.Admin
{
    public class AlterarStatusReservaDTO
    {
        [Required]
        public string NovoStatus { get; set; } = string.Empty;
    }
}
