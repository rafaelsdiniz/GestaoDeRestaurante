using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.Models
{
    public class ConfiguracaoRestaurante : BaseEntity
    {
        [MaxLength(5)]
        public string AlmocoInicio { get; set; } = "11:00";
        [MaxLength(5)]
        public string AlmocoFim { get; set; } = "14:00";
        [MaxLength(5)]
        public string JantarInicio { get; set; } = "18:00";
        [MaxLength(5)]
        public string JantarFim { get; set; } = "22:00";
        [MaxLength(5)]
        public string ReservaInicio { get; set; } = "11:00";
        [MaxLength(5)]
        public string ReservaFim { get; set; } = "14:00";
        public int AntecedenciaMinimaDias { get; set; } = 1;
    }
}
