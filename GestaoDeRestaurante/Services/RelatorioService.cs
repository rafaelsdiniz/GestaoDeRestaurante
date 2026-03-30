using GestaoDeRestaurante.Data;
using GestaoDeRestaurante.DTOs.Relatorio;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeRestaurante.Services
{
    public class RelatorioService
    {
        private readonly AppDbContext _context;

        public RelatorioService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Relatório 1: Faturamento total por tipo de atendimento em determinado período.
        /// </summary>
        public async Task<List<FaturamentoPorTipoDTO>> FaturamentoPorTipoAtendimento(DateTime dataInicio, DateTime dataFim)
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Atendimento)
                .Where(p => p.DataHora >= dataInicio && p.DataHora <= dataFim.AddDays(1).AddSeconds(-1))
                .ToListAsync();

            var resultado = pedidos
                .GroupBy(p => p.Atendimento!.TipoAtendimento)
                .Select(g => new FaturamentoPorTipoDTO
                {
                    TipoAtendimento = g.Key.ToString(),
                    QuantidadePedidos = g.Count(),
                    TotalFaturado = g.Sum(p => p.Total)
                })
                .OrderByDescending(r => r.TotalFaturado)
                .ToList();

            return resultado;
        }

        /// <summary>
        /// Relatório 2: Itens mais vendidos, indicando se são ou já foram Sugestão do Chefe.
        /// </summary>
        public async Task<List<ItemMaisVendidoDTO>> ItensMaisVendidos()
        {
            var itensPedidos = await _context.ItensPedidos
                .Include(ip => ip.ItemCardapio)
                .ToListAsync();

            // IDs que já foram Sugestão do Chefe em algum momento
            var idsSugestao = await _context.SugestoesChefe
                .Select(s => s.ItemCardapioId)
                .Distinct()
                .ToListAsync();

            var resultado = itensPedidos
                .GroupBy(ip => ip.ItemCardapioId)
                .Select(g => new ItemMaisVendidoDTO
                {
                    ItemId = g.Key,
                    NomeItem = g.First().ItemCardapio!.Nome,
                    Periodo = g.First().ItemCardapio!.Periodo.ToString(),
                    QuantidadeVendida = g.Sum(ip => ip.Quantidade),
                    TotalGerado = g.Sum(ip => ip.Subtotal),
                    EhSugestaoChefe = idsSugestao.Contains(g.Key)
                })
                .OrderByDescending(i => i.QuantidadeVendida)
                .ToList();

            return resultado;
        }
    }
}
