using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.DTOs.Endereco
{
    public class EnderecoRequestDTO
    {
        [Required]
        [StringLength(120)]
        public string Rua { get; set; }

        [Required]
        [StringLength(20)]
        public string Numero { get; set; }

        [StringLength(80)]
        public string? Complemento { get; set; }

        [Required]
        [StringLength(80)]
        public string Bairro { get; set; }

        [Required]
        [StringLength(80)]
        public string Cidade { get; set; }

        [Required]
        [StringLength(2)]
        public string Estado { get; set; }

        [Required]
        [StringLength(10)]
        public string Cep { get; set; }
    }
}