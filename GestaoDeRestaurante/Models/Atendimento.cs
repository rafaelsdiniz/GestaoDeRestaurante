using GestaoDeRestaurante.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoDeRestaurante.Models
{
    public class Atendimento : BaseEntity
    {
        public TipoAtendimento TipoAtendimento { get; set; }
        public DateTime DataHora { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TaxaEntrega { get; set; }
        public Pedido? Pedido { get; set; }
    }
}
