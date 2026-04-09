using GestaoDeRestaurante.Data;
using GestaoDeRestaurante.DTOs.ItemCardapio;
using GestaoDeRestaurante.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeRestaurante.Services
{
    public class ItemCardapioService
    {
        private readonly AppDbContext _context;
        private const int LimiteItensPorPeriodo = 20;

        public ItemCardapioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ItemCardapioResponseDTO> CriarItemCardapio(ItemCardapioRequestDTO dto)
        {
            // Limite de 20 itens por período
            var quantidadeAtual = await _context.ItensCardapio
                .CountAsync(i => i.Periodo == dto.Periodo);

            if (quantidadeAtual >= LimiteItensPorPeriodo)
                throw new Exception($"O cardápio de {dto.Periodo} já atingiu o limite de {LimiteItensPorPeriodo} itens.");

            var item = new ItemCardapio
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                PrecoBase = dto.PrecoBase,
                Periodo = dto.Periodo,
                ImagemBase64 = dto.ImagemBase64
            };

            _context.ItensCardapio.Add(item);
            await _context.SaveChangesAsync();

            if (dto.IngredientesIds != null && dto.IngredientesIds.Any())
            {
                var ingredientes = await _context.Ingredientes
                    .Where(i => dto.IngredientesIds.Contains(i.Id))
                    .ToListAsync();

                foreach (var ingrediente in ingredientes)
                {
                    _context.ItemIngredientes.Add(new ItemIngrediente
                    {
                        ItemCardapioId = item.Id,
                        IngredienteId = ingrediente.Id
                    });
                }

                await _context.SaveChangesAsync();
            }

            return await MapToResponse(item.Id);
        }

        public async Task<List<ItemCardapioResponseDTO>> ListarItensCardapio()
        {
            var itens = await _context.ItensCardapio.ToListAsync();

            var lista = new List<ItemCardapioResponseDTO>();
            foreach (var item in itens)
                lista.Add(await MapToResponse(item.Id));

            return lista;
        }

        public async Task<ItemCardapioResponseDTO> BuscarItemCardapioPorId(int id)
        {
            var item = await _context.ItensCardapio.FindAsync(id);

            if (item == null)
                throw new Exception("Item não encontrado.");

            return await MapToResponse(id);
        }

        public async Task<ItemCardapioResponseDTO> AtualizarItemCardapio(int id, ItemCardapioRequestDTO dto)
        {
            var item = await _context.ItensCardapio.FindAsync(id);

            if (item == null)
                throw new Exception("Item não encontrado.");

            // Se o período mudou, verificar o limite do novo período
            if (item.Periodo != dto.Periodo)
            {
                var quantidadeAtual = await _context.ItensCardapio
                    .CountAsync(i => i.Periodo == dto.Periodo);

                if (quantidadeAtual >= LimiteItensPorPeriodo)
                    throw new Exception($"O cardápio de {dto.Periodo} já atingiu o limite de {LimiteItensPorPeriodo} itens.");
            }

            item.Nome = dto.Nome;
            item.Descricao = dto.Descricao;
            item.PrecoBase = dto.PrecoBase;
            item.Periodo = dto.Periodo;
            item.ImagemBase64 = dto.ImagemBase64;

            var antigos = _context.ItemIngredientes.Where(ii => ii.ItemCardapioId == id);
            _context.ItemIngredientes.RemoveRange(antigos);

            if (dto.IngredientesIds != null && dto.IngredientesIds.Any())
            {
                foreach (var ingredienteId in dto.IngredientesIds)
                {
                    _context.ItemIngredientes.Add(new ItemIngrediente
                    {
                        ItemCardapioId = id,
                        IngredienteId = ingredienteId
                    });
                }
            }

            await _context.SaveChangesAsync();

            return await MapToResponse(id);
        }

        public async Task<bool> DeletarItemCardapio(int id)
        {
            var item = await _context.ItensCardapio.FindAsync(id);

            if (item == null)
                throw new Exception("Item não encontrado.");

            _context.ItensCardapio.Remove(item);
            await _context.SaveChangesAsync();

            return true;
        }

        private async Task<ItemCardapioResponseDTO> MapToResponse(int itemId)
        {
            var item = await _context.ItensCardapio
                .Include(i => i.ItensIngredientes)
                    .ThenInclude(ii => ii.Ingrediente)
                .FirstOrDefaultAsync(i => i.Id == itemId);

            if (item == null)
                throw new Exception("Item não encontrado.");

            var ehSugestao = await _context.SugestoesChefe.AnyAsync(s =>
                s.ItemCardapioId == item.Id &&
                s.DataSugestao.Date == DateTime.Now.Date &&
                s.Periodo == item.Periodo
            );

            return new ItemCardapioResponseDTO
            {
                Id = item.Id,
                Nome = item.Nome,
                Descricao = item.Descricao,
                PrecoBase = item.PrecoBase,
                Periodo = item.Periodo,
                ImagemBase64 = item.ImagemBase64,
                EhSugestaoDoChefe = ehSugestao,
                Ingredientes = item.ItensIngredientes
                    .Select(ii => ii.Ingrediente!.Nome)
                    .ToList()
            };
        }
    }
}
