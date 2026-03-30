using GestaoDeRestaurante.Enums;
using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.DTOs.SugestaoChefe
{
    public class SugestaoChefeRequestDTO
    {
        [Required]
        public DateTime DataSugestao { get; set; }

        [Required]
        public Periodo Periodo { get; set; }

        [Required]
        public int ItemCardapioId { get; set; }
    }
}