namespace GestaoDeRestaurante.DTOs.Configuracao
{
    public class HorariosPublicosDTO
    {
        public string AlmocoInicio { get; set; } = null!;
        public string AlmocoFim { get; set; } = null!;
        public string JantarInicio { get; set; } = null!;
        public string JantarFim { get; set; } = null!;
        public string ReservaInicio { get; set; } = null!;
        public string ReservaFim { get; set; } = null!;
        public int AntecedenciaMinimaDias { get; set; }
    }
}
