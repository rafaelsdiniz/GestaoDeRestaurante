using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.DTOs.Ingrediente
{
    public class IngredienteRequestDTO
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Descricao { get; set; }
    }
}