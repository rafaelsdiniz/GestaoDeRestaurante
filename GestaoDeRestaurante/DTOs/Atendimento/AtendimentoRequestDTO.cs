using GestaoDeRestaurante.Enums;
using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.DTOs.Atendimento
{
    public class AtendimentoRequestDTO
    {
        [Required]
        public TipoAtendimento TipoAtendimento { get; set; }

        public string? ObservacaoEntrega { get; set; } // delivery proprio

        public string? NomeAplicativo { get; set; } // delivery aplicativo
    }
}