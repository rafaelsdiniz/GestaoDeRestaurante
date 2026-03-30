namespace GestaoDeRestaurante.DTOs.Relatorio
{
    public class FaturamentoPorTipoDTO
    {
        public string TipoAtendimento { get; set; } = string.Empty;
        public int QuantidadePedidos { get; set; }
        public decimal TotalFaturado { get; set; }
    }
}
