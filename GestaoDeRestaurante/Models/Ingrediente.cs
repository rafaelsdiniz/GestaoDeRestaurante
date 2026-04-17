using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.Models
{
    public class Ingrediente : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(300)]
        public string? Descricao { get; set; }

        public List<ItemIngrediente> ItensIngredientes { get; set; } = new();
    }
}
