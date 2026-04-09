using GestaoDeRestaurante.DTOs.ItemPedido;
using GestaoDeRestaurante.Enums;

namespace GestaoDeRestaurante.DTOs.Pedido
{
    public class PedidoResponseDTO
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public Periodo Periodo { get; set; }

        public decimal Subtotal { get; set; }
        public decimal Desconto { get; set; }
        public decimal TaxaEntrega { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;

        public string NomeUsuario { get; set; }
        public string TipoAtendimento { get; set; }

        public List<ItemPedidoResponseDTO> Itens { get; set; }
    }
}