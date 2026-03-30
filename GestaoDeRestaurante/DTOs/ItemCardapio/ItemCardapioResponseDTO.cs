using GestaoDeRestaurante.Enums;

namespace GestaoDeRestaurante.DTOs.ItemCardapio
{
    public class ItemCardapioResponseDTO
    {
        public int Id { get; set; }

        public string Nome { get; set; }
        public string Descricao { get; set; }

        public decimal PrecoBase { get; set; }

        public Periodo Periodo { get; set; }

        public bool EhSugestaoDoChefe { get; set; }

        public List<string> Ingredientes { get; set; }
    }
}