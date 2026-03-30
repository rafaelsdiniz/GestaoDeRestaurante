using GestaoDeRestaurante.Data;
using GestaoDeRestaurante.DTOs.Pedido;
using GestaoDeRestaurante.Enums;
using GestaoDeRestaurante.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeRestaurante.Services
{
    public class PedidoService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public PedidoService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<PedidoResponseDTO> CriarPedido(int usuarioId, PedidoRequestDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
                throw new Exception("Usuário não encontrado.");

            var atendimento = await _context.Atendimentos.FindAsync(dto.AtendimentoId);
            if (atendimento == null)
                throw new Exception("Atendimento não encontrado.");

            var itens = await _context.ItensCardapio
                .Where(i => dto.ItensIds.Contains(i.Id))
                .ToListAsync();

            if (itens.Count == 0)
                throw new Exception("Nenhum item válido encontrado.");

            // Regra 3 e 4: itens devem pertencer ao mesmo período do pedido
            var itensInvalidos = itens.Where(i => i.Periodo != dto.Periodo).ToList();
            if (itensInvalidos.Any())
            {
                var nomes = string.Join(", ", itensInvalidos.Select(i => i.Nome));
                throw new Exception($"Os seguintes itens não pertencem ao período {dto.Periodo}: {nomes}.");
            }

            // Sugestões do Chefe de hoje para este período (para aplicar desconto)
            var sugestoesHoje = await _context.SugestoesChefe
                .Where(s => s.DataSugestao.Date == DateTime.Now.Date && s.Periodo == dto.Periodo)
                .Select(s => s.ItemCardapioId)
                .ToListAsync();

            decimal subtotal = itens.Sum(i => i.PrecoBase);

            // Desconto de 20% nos itens que são Sugestão do Chefe hoje
            decimal desconto = itens
                .Where(i => sugestoesHoje.Contains(i.Id))
                .Sum(i => i.PrecoBase * 0.20m);

            // Taxa de entrega:
            // - Delivery Próprio: taxa fixa configurável
            // - Delivery Aplicativo: 4% no almoço, 6% no jantar
            // - Presencial: sem taxa
            decimal taxaDeliveryProprio = _configuration.GetValue<decimal>("Configuracoes:TaxaDeliveryProprio", 10m);
            decimal taxaEntrega = atendimento.TipoAtendimento switch
            {
                TipoAtendimento.DeliveryProprio => taxaDeliveryProprio,
                TipoAtendimento.DeliveryAplicativo => dto.Periodo == Periodo.Almoco
                    ? subtotal * 0.04m
                    : subtotal * 0.06m,
                _ => 0m
            };

            decimal total = subtotal - desconto + taxaEntrega;

            var pedido = new Pedido
            {
                DataHora = DateTime.Now,
                Periodo = dto.Periodo,
                Subtotal = subtotal,
                Desconto = desconto,
                TaxaEntrega = taxaEntrega,
                Total = total,
                UsuarioId = usuarioId,
                AtendimentoId = dto.AtendimentoId
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            foreach (var item in itens)
            {
                _context.ItensPedidos.Add(new ItemPedido
                {
                    PedidoId = pedido.Id,
                    ItemCardapioId = item.Id,
                    Quantidade = 1,
                    PrecoUnitario = item.PrecoBase,
                    Subtotal = item.PrecoBase
                });
            }

            atendimento.TaxaEntrega = taxaEntrega;
            await _context.SaveChangesAsync();

            return await MapToResponse(pedido.Id);
        }

        public async Task<List<PedidoResponseDTO>> ListarPedidos()
        {
            var pedidos = await _context.Pedidos.ToListAsync();
            var lista = new List<PedidoResponseDTO>();

            foreach (var p in pedidos)
                lista.Add(await MapToResponse(p.Id));

            return lista;
        }

        public async Task<PedidoResponseDTO> BuscarPedidoPorId(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
                throw new Exception("Pedido não encontrado.");

            return await MapToResponse(id);
        }

        private async Task<PedidoResponseDTO> MapToResponse(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Atendimento)
                .Include(p => p.ItensPedidos)
                    .ThenInclude(ip => ip.ItemCardapio)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null)
                throw new Exception("Pedido não encontrado.");

            return new PedidoResponseDTO
            {
                Id = pedido.Id,
                DataHora = pedido.DataHora,
                Periodo = pedido.Periodo,
                Subtotal = pedido.Subtotal,
                Desconto = pedido.Desconto,
                TaxaEntrega = pedido.TaxaEntrega,
                Total = pedido.Total,
                NomeUsuario = pedido.Usuario!.Nome,
                TipoAtendimento = pedido.Atendimento!.TipoAtendimento.ToString(),
                Itens = pedido.ItensPedidos
                    .Select(ip => ip.ItemCardapio!.Nome)
                    .ToList()
            };
        }
    }
}
