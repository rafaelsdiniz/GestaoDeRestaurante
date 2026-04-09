using GestaoDeRestaurante.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoDeRestaurante.Models
{
    public class Pedido : BaseEntity
    {
        public DateTime DataHora { get; set; }
        public Periodo Periodo { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Desconto { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TaxaEntrega { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        public StatusPedido Status { get; set; } = StatusPedido.Recebido;

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int AtendimentoId { get; set; }
        public Atendimento? Atendimento { get; set; }

        public List<ItemPedido> ItensPedidos { get; set; } = new();
    }
}
