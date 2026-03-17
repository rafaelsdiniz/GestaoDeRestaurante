using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.Models
{
    public class Ingrediente : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        public List<ItemIngrediente> ItensIngredientes { get; set; } = new();
    }
}
