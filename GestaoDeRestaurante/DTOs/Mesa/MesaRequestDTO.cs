using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.DTOs.Mesa
{
    public class MesaRequestDTO
    {
        [Required]
        public int Numero { get; set; }

        [Required]
        public int Capacidade { get; set; }
    }
}