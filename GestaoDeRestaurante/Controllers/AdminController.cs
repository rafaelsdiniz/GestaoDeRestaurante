using GestaoDeRestaurante.Data;
using GestaoDeRestaurante.DTOs.Admin;
using GestaoDeRestaurante.Enums;
using GestaoDeRestaurante.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeRestaurante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrador")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista todos os usuários com estatísticas
        /// </summary>
        [HttpGet("usuarios")]
        public async Task<ActionResult<IEnumerable<UsuarioAdminDTO>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Pedidos)
                .Include(u => u.Reservas)
                .Select(u => new UsuarioAdminDTO
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    TipoUsuario = u.TipoUsuario.ToString(),
                    TotalPedidos = u.Pedidos.Count,
                    TotalReservas = u.Reservas.Count,
                    UltimoPedido = u.Pedidos.OrderByDescending(p => p.DataHora).FirstOrDefault() != null ? u.Pedidos.OrderByDescending(p => p.DataHora).FirstOrDefault().DataHora : (DateTime?)null,
                    UltimaReserva = u.Reservas.OrderByDescending(r => r.DataHoraReserva).FirstOrDefault() != null ? u.Reservas.OrderByDescending(r => r.DataHoraReserva).FirstOrDefault().DataHoraReserva : (DateTime?)null
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        /// <summary>
        /// Altera o tipo de um usuário (Cliente ↔ Administrador)
        /// </summary>
        [HttpPut("usuarios/{id}/tipo")]
        public async Task<IActionResult> AlterarTipoUsuario(int id, [FromBody] AlterarTipoUsuarioDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound(new { mensagem = "Usuário não encontrado." });

            if (!Enum.TryParse<TipoUsuario>(dto.NovoTipo, out var novoTipo))
                return BadRequest(new { mensagem = "Tipo de usuário inválido. Use 'Cliente' ou 'Administrador'." });

            usuario.TipoUsuario = novoTipo;
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = $"Tipo do usuário {usuario.Nome} alterado para {novoTipo}." });
        }

        /// <summary>
        /// Painel administrativo com estatísticas gerais
        /// </summary>
        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardAdminDTO>> GetDashboard()
        {
            var hoje = DateTime.Today;
            var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);

            var totalUsuarios = await _context.Usuarios.CountAsync();
            var totalAdministradores = await _context.Usuarios.CountAsync(u => u.TipoUsuario == TipoUsuario.Administrador);
            var totalClientes = totalUsuarios - totalAdministradores;

            var totalPedidos = await _context.Pedidos.CountAsync();
            var pedidosHoje = await _context.Pedidos.CountAsync(p => p.DataHora.Date == hoje);
            var pedidosMes = await _context.Pedidos.CountAsync(p => p.DataHora >= inicioMes && p.DataHora <= fimMes);

            var totalReservas = await _context.Reservas.CountAsync();
            var reservasHoje = await _context.Reservas.CountAsync(r => r.DataHoraReserva.Date == hoje);
            var reservasMes = await _context.Reservas.CountAsync(r => r.DataHoraReserva >= inicioMes && r.DataHoraReserva <= fimMes);

            var totalAtendimentos = await _context.Atendimentos.CountAsync();
            var atendimentosPresencial = await _context.Atendimentos.CountAsync(a => a.TipoAtendimento == TipoAtendimento.AtendimentoPresencial);
            var atendimentosDelivery = await _context.Atendimentos.CountAsync(a => a.TipoAtendimento == TipoAtendimento.DeliveryProprio);
            var atendimentosApp = await _context.Atendimentos.CountAsync(a => a.TipoAtendimento == TipoAtendimento.DeliveryAplicativo);

            var totalItensCardapio = await _context.ItensCardapio.CountAsync();
            var itensAlmoco = await _context.ItensCardapio.CountAsync(i => i.Periodo == Periodo.Almoco);
            var itensJantar = await _context.ItensCardapio.CountAsync(i => i.Periodo == Periodo.Jantar);

            var faturamentoTotal = await _context.Pedidos.SumAsync(p => p.Total);
            var faturamentoMes = await _context.Pedidos
                .Where(p => p.DataHora >= inicioMes && p.DataHora <= fimMes)
                .SumAsync(p => p.Total);

            var dashboard = new DashboardAdminDTO
            {
                TotalUsuarios = totalUsuarios,
                TotalAdministradores = totalAdministradores,
                TotalClientes = totalClientes,
                TotalPedidos = totalPedidos,
                PedidosHoje = pedidosHoje,
                PedidosMes = pedidosMes,
                TotalReservas = totalReservas,
                ReservasHoje = reservasHoje,
                ReservasMes = reservasMes,
                TotalAtendimentos = totalAtendimentos,
                AtendimentosPresencial = atendimentosPresencial,
                AtendimentosDelivery = atendimentosDelivery,
                AtendimentosApp = atendimentosApp,
                TotalItensCardapio = totalItensCardapio,
                ItensAlmoco = itensAlmoco,
                ItensJantar = itensJantar,
                FaturamentoTotal = faturamentoTotal,
                FaturamentoMes = faturamentoMes,
                DataAtualizacao = DateTime.Now
            };

            return Ok(dashboard);
        }

        /// <summary>
        /// Lista os últimos pedidos (últimos 10)
        /// </summary>
        [HttpGet("pedidos/recentes")]
        public async Task<ActionResult<IEnumerable<PedidoAdminDTO>>> GetPedidosRecentes()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Atendimento)
                .Include(p => p.ItensPedidos)
                    .ThenInclude(ip => ip.ItemCardapio)
                .OrderByDescending(p => p.DataHora)
                .Take(10)
                .Select(p => new PedidoAdminDTO
                {
                    Id = p.Id,
                    DataHora = p.DataHora,
                    Periodo = p.Periodo.ToString(),
                    Subtotal = p.Subtotal,
                    Desconto = p.Desconto,
                    TaxaEntrega = p.TaxaEntrega,
                    Total = p.Total,
                    Status = p.Status.ToString(),
                    UsuarioNome = p.Usuario.Nome,
                    UsuarioEmail = p.Usuario.Email,
                    TipoAtendimento = p.Atendimento.TipoAtendimento.ToString(),
                    Itens = p.ItensPedidos.Select(ip => new ItemPedidoAdminDTO
                    {
                        ItemCardapioNome = ip.ItemCardapio.Nome,
                        Quantidade = ip.Quantidade,
                        PrecoUnitario = ip.PrecoUnitario,
                        Subtotal = ip.Subtotal
                    }).ToList()
                })
                .ToListAsync();

            return Ok(pedidos);
        }

        /// <summary>
        /// Lista TODOS os pedidos com filtros opcionais
        /// </summary>
        [HttpGet("pedidos")]
        public async Task<ActionResult<IEnumerable<PedidoAdminDTO>>> GetTodosPedidos(
            [FromQuery] string? status,
            [FromQuery] string? periodo,
            [FromQuery] DateTime? dataInicio,
            [FromQuery] DateTime? dataFim)
        {
            var query = _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Atendimento)
                .Include(p => p.ItensPedidos)
                    .ThenInclude(ip => ip.ItemCardapio)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<Enums.StatusPedido>(status, out var statusPedido))
                query = query.Where(p => p.Status == statusPedido);

            if (!string.IsNullOrEmpty(periodo) && Enum.TryParse<Enums.Periodo>(periodo, out var periodoPedido))
                query = query.Where(p => p.Periodo == periodoPedido);

            if (dataInicio.HasValue)
                query = query.Where(p => p.DataHora >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(p => p.DataHora <= dataFim.Value);

            var pedidos = await query
                .OrderByDescending(p => p.DataHora)
                .Select(p => new PedidoAdminDTO
                {
                    Id = p.Id,
                    DataHora = p.DataHora,
                    Periodo = p.Periodo.ToString(),
                    Subtotal = p.Subtotal,
                    Desconto = p.Desconto,
                    TaxaEntrega = p.TaxaEntrega,
                    Total = p.Total,
                    Status = p.Status.ToString(),
                    UsuarioNome = p.Usuario.Nome,
                    UsuarioEmail = p.Usuario.Email,
                    TipoAtendimento = p.Atendimento.TipoAtendimento.ToString(),
                    Itens = p.ItensPedidos.Select(ip => new ItemPedidoAdminDTO
                    {
                        ItemCardapioNome = ip.ItemCardapio.Nome,
                        Quantidade = ip.Quantidade,
                        PrecoUnitario = ip.PrecoUnitario,
                        Subtotal = ip.Subtotal
                    }).ToList()
                })
                .ToListAsync();

            return Ok(pedidos);
        }

        /// <summary>
        /// Altera o status de um pedido
        /// </summary>
        [HttpPut("pedidos/{id}/status")]
        public async Task<IActionResult> AlterarStatusPedido(int id, [FromBody] AlterarStatusPedidoDTO dto)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
                return NotFound(new { mensagem = "Pedido não encontrado." });

            if (!Enum.TryParse<Enums.StatusPedido>(dto.NovoStatus, out var novoStatus))
                return BadRequest(new { mensagem = "Status inválido. Use: Recebido, EmPreparo, Pronto, Entregue ou Cancelado." });

            pedido.Status = novoStatus;
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = $"Status do pedido #{id} alterado para {novoStatus}." });
        }

        /// <summary>
        /// Lista as próximas reservas (próximos 7 dias)
        /// </summary>
        [HttpGet("reservas/proximas")]
        public async Task<ActionResult<IEnumerable<ReservaAdminDTO>>> GetReservasProximas()
        {
            var hoje = DateTime.Today;
            var limite = hoje.AddDays(7);

            var reservas = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Mesa)
                .Where(r => r.DataHoraReserva >= hoje && r.DataHoraReserva <= limite)
                .OrderBy(r => r.DataHoraReserva)
                .Select(r => new ReservaAdminDTO
                {
                    Id = r.Id,
                    DataHoraReserva = r.DataHoraReserva,
                    QuantidadePessoas = r.QuantidadePessoas,
                    StatusReserva = r.StatusReserva.ToString(),
                    CodigoConfirmacao = r.CodigoConfirmacao,
                    UsuarioNome = r.Usuario.Nome,
                    UsuarioEmail = r.Usuario.Email,
                    MesaNumero = r.Mesa.Numero,
                    MesaCapacidade = r.Mesa.Capacidade
                })
                .ToListAsync();

            return Ok(reservas);
        }

        /// <summary>
        /// Lista TODAS as reservas com filtros opcionais
        /// </summary>
        [HttpGet("reservas")]
        public async Task<ActionResult<IEnumerable<ReservaAdminDTO>>> GetTodasReservas(
            [FromQuery] string? status,
            [FromQuery] DateTime? dataInicio,
            [FromQuery] DateTime? dataFim)
        {
            var query = _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Mesa)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<Enums.StatusReserva>(status, out var statusReserva))
                query = query.Where(r => r.StatusReserva == statusReserva);

            if (dataInicio.HasValue)
                query = query.Where(r => r.DataHoraReserva >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(r => r.DataHoraReserva <= dataFim.Value);

            var reservas = await query
                .OrderByDescending(r => r.DataHoraReserva)
                .Select(r => new ReservaAdminDTO
                {
                    Id = r.Id,
                    DataHoraReserva = r.DataHoraReserva,
                    QuantidadePessoas = r.QuantidadePessoas,
                    StatusReserva = r.StatusReserva.ToString(),
                    CodigoConfirmacao = r.CodigoConfirmacao,
                    UsuarioNome = r.Usuario.Nome,
                    UsuarioEmail = r.Usuario.Email,
                    MesaNumero = r.Mesa.Numero,
                    MesaCapacidade = r.Mesa.Capacidade
                })
                .ToListAsync();

            return Ok(reservas);
        }

        /// <summary>
        /// Altera o status de uma reserva (Confirmar, Cancelar, Finalizar)
        /// </summary>
        [HttpPut("reservas/{id}/status")]
        public async Task<IActionResult> AlterarStatusReserva(int id, [FromBody] AlterarStatusReservaDTO dto)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return NotFound(new { mensagem = "Reserva não encontrada." });

            if (!Enum.TryParse<Enums.StatusReserva>(dto.NovoStatus, out var novoStatus))
                return BadRequest(new { mensagem = "Status inválido. Use: Ativa, Confirmada, Cancelada ou Finalizada." });

            reserva.StatusReserva = novoStatus;
            await _context.SaveChangesAsync();

            return Ok(new { mensagem = $"Status da reserva #{id} alterado para {novoStatus}." });
        }
    }
}