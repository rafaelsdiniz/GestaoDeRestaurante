using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.DTOs.Mesa
{
    public class MesaRequestDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "O número da mesa deve ser maior que zero.")]
        public int Numero { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "A capacidade deve ser entre 1 e 100 pessoas.")]
        public int Capacidade { get; set; }
    }
}
