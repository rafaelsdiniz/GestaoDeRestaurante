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
        [Range(0.01, 99999.99, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal PrecoBase { get; set; }

        [Required]
        public Periodo Periodo { get; set; }

        [Required]
        public Categoria Categoria { get; set; }

        public string? ImagemBase64 { get; set; }

        public List<int>? IngredientesIds { get; set; }
    }
}