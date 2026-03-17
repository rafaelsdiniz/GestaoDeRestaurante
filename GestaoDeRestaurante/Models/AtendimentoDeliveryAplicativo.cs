using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.Models
{
    public class AtendimentoDeliveryAplicativo : Atendimento
    {
        [StringLength(80)]
        public string NomeAplicativo { get; set; } = string.Empty;
    }
}
