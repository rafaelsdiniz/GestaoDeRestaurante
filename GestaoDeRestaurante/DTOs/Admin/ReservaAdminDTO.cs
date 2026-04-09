namespace GestaoDeRestaurante.DTOs.Admin
{
    public class ReservaAdminDTO
    {
        public int Id { get; set; }
        public DateTime DataHoraReserva { get; set; }
        public int QuantidadePessoas { get; set; }
        public string StatusReserva { get; set; } = string.Empty;
        public string CodigoConfirmacao { get; set; } = string.Empty;
        public string UsuarioNome { get; set; } = string.Empty;
        public string UsuarioEmail { get; set; } = string.Empty;
        public int MesaNumero { get; set; }
        public int MesaCapacidade { get; set; }
    }
}