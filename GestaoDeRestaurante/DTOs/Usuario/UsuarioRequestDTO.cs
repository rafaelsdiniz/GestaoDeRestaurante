using System.ComponentModel.DataAnnotations;

namespace GestaoDeRestaurante.DTOs.Usuario
{
    public class UsuarioRequestDTO
    {
        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(120)]
        public string Email { get; set; }

        [Required]  
        [StringLength(200)]
        public string Senha { get; set; }
    }
}
