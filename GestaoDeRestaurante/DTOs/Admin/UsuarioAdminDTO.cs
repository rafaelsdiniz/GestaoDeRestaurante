namespace GestaoDeRestaurante.DTOs.Admin
{
    public class UsuarioAdminDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
        public int TotalPedidos { get; set; }
        public int TotalReservas { get; set; }
        public DateTime? UltimoPedido { get; set; }
        public DateTime? UltimaReserva { get; set; }
    }
}