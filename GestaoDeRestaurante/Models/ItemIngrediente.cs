namespace GestaoDeRestaurante.Models
{
    public class ItemIngrediente : BaseEntity
    {
        public int ItemCardapioId { get; set; }
        public ItemCardapio? ItemCardapio { get; set; }

        public int IngredienteId { get; set; }
        public Ingrediente? Ingrediente { get; set; }
    }
}
