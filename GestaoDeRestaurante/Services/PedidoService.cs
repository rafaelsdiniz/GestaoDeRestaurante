using GestaoDeRestaurante.Data;
using GestaoDeRestaurante.DTOs.ItemPedido;
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

            // Valida que TODOS os itens solicitados existem
            if (itens.Count != dto.ItensIds.Distinct().Count())
            {
                var idsEncontrados = itens.Select(i => i.Id).ToList();
                var idsNaoEncontrados = dto.ItensIds.Distinct().Where(id => !idsEncontrados.Contains(id)).ToList();
                throw new Exception($"Os seguintes IDs de itens não foram encontrados: {string.Join(", ", idsNaoEncontrados)}.");
            }

            // Regra: itens devem pertencer ao mesmo período do pedido
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

            // Calcula quantidade por item (suporta itens duplicados na lista)
            var itensAgrupados = dto.ItensIds
                .GroupBy(id => id)
                .Select(g => new { ItemId = g.Key, Quantidade = g.Count() })
                .ToList();

            decimal subtotal = 0;
            decimal desconto = 0;
            var itensPedido = new List<ItemPedido>();

            foreach (var grupo in itensAgrupados)
            {
                var item = itens.First(i => i.Id == grupo.ItemId);
                var subtotalItem = item.PrecoBase * grupo.Quantidade;
                subtotal += subtotalItem;

                if (sugestoesHoje.Contains(item.Id))
                    desconto += subtotalItem * 0.20m;

                itensPedido.Add(new ItemPedido
                {
                    ItemCardapioId = item.Id,
                    Quantidade = grupo.Quantidade,
                    PrecoUnitario = item.PrecoBase,
                    Subtotal = subtotalItem
                });
            }

            // Taxa de entrega
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

            foreach (var ip in itensPedido)
            {
                ip.PedidoId = pedido.Id;
                _context.ItensPedidos.Add(ip);
            }

            atendimento.TaxaEntrega = taxaEntrega;
            await _context.SaveChangesAsync();

            return MapToResponse(pedido, usuario.Nome, atendimento.TipoAtendimento.ToString(), itensPedido, itens);
        }

        public async Task<List<PedidoResponseDTO>> ListarPedidosPorUsuario(int usuarioId)
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Usuario)
                .Include(p => p.Atendimento)
                .Include(p => p.ItensPedidos)
                    .ThenInclude(ip => ip.ItemCardapio)
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.DataHora)
                .ToListAsync();

            return pedidos.Select(p => new PedidoResponseDTO
            {
                Id = p.Id,
                DataHora = p.DataHora,
                Periodo = p.Periodo,
                Subtotal = p.Subtotal,
                Desconto = p.Desconto,
                TaxaEntrega = p.TaxaEntrega,
                Total = p.Total,
                Status = p.Status.ToString(),
                NomeUsuario = p.Usuario!.Nome,
                TipoAtendimento = p.Atendimento!.TipoAtendimento.ToString(),
                Itens = p.ItensPedidos.Select(ip => new ItemPedidoResponseDTO
                {
                    NomeItem = ip.ItemCardapio!.Nome,
                    Quantidade = ip.Quantidade,
                    PrecoUnitario = ip.PrecoUnitario,
                    Subtotal = ip.Subtotal
                }).ToList()
            }).ToList();
        }

        public async Task<PedidoResponseDTO> BuscarPedidoPorId(int id)
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
                Status = pedido.Status.ToString(),
                NomeUsuario = pedido.Usuario!.Nome,
                TipoAtendimento = pedido.Atendimento!.TipoAtendimento.ToString(),
                Itens = pedido.ItensPedidos.Select(ip => new ItemPedidoResponseDTO
                {
                    NomeItem = ip.ItemCardapio!.Nome,
                    Quantidade = ip.Quantidade,
                    PrecoUnitario = ip.PrecoUnitario,
                    Subtotal = ip.Subtotal
                }).ToList()
            };
        }

        private static PedidoResponseDTO MapToResponse(Pedido pedido, string nomeUsuario, string tipoAtendimento, List<ItemPedido> itensPedido, List<ItemCardapio> itensCardapio)
        {
            return new PedidoResponseDTO
            {
                Id = pedido.Id,
                DataHora = pedido.DataHora,
                Periodo = pedido.Periodo,
                Subtotal = pedido.Subtotal,
                Desconto = pedido.Desconto,
                TaxaEntrega = pedido.TaxaEntrega,
                Total = pedido.Total,
                Status = pedido.Status.ToString(),
                NomeUsuario = nomeUsuario,
                TipoAtendimento = tipoAtendimento,
                Itens = itensPedido.Select(ip =>
                {
                    var item = itensCardapio.First(i => i.Id == ip.ItemCardapioId);
                    return new ItemPedidoResponseDTO
                    {
                        NomeItem = item.Nome,
                        Quantidade = ip.Quantidade,
                        PrecoUnitario = ip.PrecoUnitario,
                        Subtotal = ip.Subtotal
                    };
                }).ToList()
            };
        }
    }
}
