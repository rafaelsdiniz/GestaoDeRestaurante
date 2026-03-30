using GestaoDeRestaurante.Enums;

namespace GestaoDeRestaurante.DTOs.Atendimento
{
    public class AtendimentoResponseDTO
    {
        public int Id { get; set; }
        public TipoAtendimento TipoAtendimento { get; set; }
        public DateTime DataHora { get; set; }
        public decimal TaxaEntrega { get; set; }
    }
}