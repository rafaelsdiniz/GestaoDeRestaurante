using GestaoDeRestaurante.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoDeRestaurante.Models
{
    public class ItemCardapio : BaseEntity
    {
        [Required]
        [StringLength(120)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string Descricao {  get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecoBase { get; set; }

        [Required]
        public Periodo Periodo { get; set; }

        public List<ItemIngrediente> ItensIngredientes { get; set; } = new();
        public List<ItemPedido> ItensPedidos { get; set; } = new();
        public List<SugestaoChefe> SugestoesChefe { get; set; } = new();
    }
}
