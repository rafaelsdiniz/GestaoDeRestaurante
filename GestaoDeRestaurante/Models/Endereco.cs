using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.Models
{
    public class Endereco : BaseEntity
    {
        [Required]
        [StringLength(120)]
        public string Rua { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Numero { get; set; } = string.Empty;

        [StringLength(80)] 
        public string? Complemento {  get; set; } = string.Empty;

        [Required]
        [StringLength(80)]
        public string Bairro { get; set; } = string.Empty;

        [Required]
        [StringLength(80)]
        public string Cidade { get; set; } = string.Empty;

        [Required]
        [StringLength(2)]
        public string Estado { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Cep { get; set; } = string.Empty;

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

    }   
}
