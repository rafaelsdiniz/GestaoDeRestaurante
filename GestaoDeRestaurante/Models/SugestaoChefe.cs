using GestaoDeRestaurante.Enums;

namespace GestaoDeRestaurante.Models
{
    public class SugestaoChefe : BaseEntity
    {
        public DateTime DataSugestao { get; set; }

        public Periodo Periodo { get; set; }

        public int ItemCardapioId { get; set; }
        public ItemCardapio? ItemCardapio { get; set; }
    }
}
