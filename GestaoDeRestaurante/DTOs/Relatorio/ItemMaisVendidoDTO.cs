namespace GestaoDeRestaurante.DTOs.Relatorio
{
    public class ItemMaisVendidoDTO
    {
        public int ItemId { get; set; }
        public string NomeItem { get; set; } = string.Empty;
        public string Periodo { get; set; } = string.Empty;
        public int QuantidadeVendida { get; set; }
        public decimal TotalGerado { get; set; }
        public bool EhSugestaoChefe { get; set; }
    }
}
