namespace GestaoDeRestaurante.DTOs.ItemPedido
{
    public class ItemPedidoResponseDTO
    {
        public string NomeItem { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
