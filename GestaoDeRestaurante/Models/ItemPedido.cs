using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoDeRestaurante.Models
{
    public class ItemPedido : BaseEntity
    {
        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }

        public int ItemCardapioId { get; set; }
        public ItemCardapio? ItemCardapio { get; set; }

        public int Quantidade { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecoUnitario { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }

    }
}
