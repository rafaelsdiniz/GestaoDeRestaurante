using GestaoDeRestaurante.Enums;

namespace GestaoDeRestaurante.DTOs.SugestaoChefe
{
    public class SugestaoChefeResponseDTO
    {
        public int Id { get; set; }
        public DateTime DataSugestao { get; set; }
        public Periodo Periodo { get; set; }

        public string NomeItem { get; set; }
    }
}