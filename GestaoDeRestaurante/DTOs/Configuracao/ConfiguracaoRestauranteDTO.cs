using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.DTOs.Configuracao
{
    public class ConfiguracaoRestauranteDTO
    {
        [Required]
        [RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d$", ErrorMessage = "Formato inválido. Use HH:mm (ex: 11:00).")]
        public string AlmocoInicio { get; set; } = "11:00";

        [Required]
        [RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d$", ErrorMessage = "Formato inválido. Use HH:mm (ex: 14:00).")]
        public string AlmocoFim { get; set; } = "14:00";

        [Required]
        [RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d$", ErrorMessage = "Formato inválido. Use HH:mm (ex: 18:00).")]
        public string JantarInicio { get; set; } = "18:00";

        [Required]
        [RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d$", ErrorMessage = "Formato inválido. Use HH:mm (ex: 22:00).")]
        public string JantarFim { get; set; } = "22:00";

        [Required]
        [RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d$", ErrorMessage = "Formato inválido. Use HH:mm (ex: 11:00).")]
        public string ReservaInicio { get; set; } = "11:00";

        [Required]
        [RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d$", ErrorMessage = "Formato inválido. Use HH:mm (ex: 14:00).")]
        public string ReservaFim { get; set; } = "14:00";

        [Required]
        [Range(0, 30, ErrorMessage = "Antecedência mínima deve ser entre 0 e 30 dias.")]
        public int AntecedenciaMinimaDias { get; set; } = 1;
    }
}
