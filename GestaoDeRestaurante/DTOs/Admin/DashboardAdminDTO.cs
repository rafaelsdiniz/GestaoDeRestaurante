namespace GestaoDeRestaurante.DTOs.Admin
{
    public class DashboardAdminDTO
    {
        public int TotalUsuarios { get; set; }
        public int TotalAdministradores { get; set; }
        public int TotalClientes { get; set; }
        public int TotalPedidos { get; set; }
        public int PedidosHoje { get; set; }
        public int PedidosMes { get; set; }
        public int TotalReservas { get; set; }
        public int ReservasHoje { get; set; }
        public int ReservasMes { get; set; }
        public int TotalAtendimentos { get; set; }
        public int AtendimentosPresencial { get; set; }
        public int AtendimentosDelivery { get; set; }
        public int AtendimentosApp { get; set; }
        public int TotalItensCardapio { get; set; }
        public int ItensAlmoco { get; set; }
        public int ItensJantar { get; set; }
        public decimal FaturamentoTotal { get; set; }
        public decimal FaturamentoMes { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}