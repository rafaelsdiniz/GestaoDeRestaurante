using GestaoDeRestaurante.Enums;
using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.DTOs.ItemCardapio
{
    public class ItemCardapioRequestDTO
    {
        [Required]
        [StringLength(120)]
        public string Nome { get; set; }

        [Required]
        [StringLength(300)]
        public string Descricao { get; set; }

        [Required]
        public decimal PrecoBase { get; set; }

        [Required]
        public Periodo Periodo { get; set; }

        public List<int>? IngredientesIds { get; set; }
    }
}