namespace GestaoDeRestaurante.DTOs.Admin
{
    public class PedidoAdminDTO
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public string Periodo { get; set; } = string.Empty;
        public decimal Subtotal { get; set; }
        public decimal Desconto { get; set; }
        public decimal TaxaEntrega { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
        public string UsuarioNome { get; set; } = string.Empty;
        public string UsuarioEmail { get; set; } = string.Empty;
        public string TipoAtendimento { get; set; } = string.Empty;
        public List<ItemPedidoAdminDTO> Itens { get; set; } = new();
    }

    public class ItemPedidoAdminDTO
    {
        public string ItemCardapioNome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}