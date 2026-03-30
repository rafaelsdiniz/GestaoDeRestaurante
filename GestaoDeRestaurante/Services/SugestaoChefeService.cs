using GestaoDeRestaurante.Data;
using GestaoDeRestaurante.DTOs.SugestaoChefe;
using GestaoDeRestaurante.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeRestaurante.Services
{
    public class SugestaoChefeService
    {
        private readonly AppDbContext _context;

        public SugestaoChefeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SugestaoChefeResponseDTO> CriarSugestaoDoChefe(SugestaoChefeRequestDTO dto)
        {
            var existe = await _context.SugestoesChefe.AnyAsync(s =>
                s.DataSugestao.Date == dto.DataSugestao.Date &&
                s.Periodo == dto.Periodo
            );

            if (existe)
                throw new Exception("Já existe uma sugestão para este período neste dia.");

            var item = await _context.ItensCardapio
                .FirstOrDefaultAsync(i => i.Id == dto.ItemCardapioId);

            if (item == null)
                throw new Exception("Item do cardápio não encontrado.");

            if (item.Periodo != dto.Periodo)
                throw new Exception("O item não pertence ao período informado.");

            var sugestao = new SugestaoChefe
            {
                DataSugestao = dto.DataSugestao,
                Periodo = dto.Periodo,
                ItemCardapioId = dto.ItemCardapioId
            };

            _context.SugestoesChefe.Add(sugestao);
            await _context.SaveChangesAsync();

            return new SugestaoChefeResponseDTO
            {
                Id = sugestao.Id,
                DataSugestao = sugestao.DataSugestao,
                Periodo = sugestao.Periodo,
                NomeItem = item.Nome
            };
        }

        public async Task<List<SugestaoChefeResponseDTO>> ListarSugestoesDoChefe()
        {
            var sugestoes = await _context.SugestoesChefe
                .Include(s => s.ItemCardapio)
                .ToListAsync();

            return sugestoes.Select(s => new SugestaoChefeResponseDTO
            {
                Id = s.Id,
                DataSugestao = s.DataSugestao,
                Periodo = s.Periodo,
                NomeItem = s.ItemCardapio!.Nome
            }).ToList();
        }

        public async Task<SugestaoChefeResponseDTO> BuscarSugestaoDoChefePorId(int id)
        {
            var sugestao = await _context.SugestoesChefe
                .Include(s => s.ItemCardapio)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (sugestao == null)
                throw new Exception("Sugestão não encontrada.");

            return new SugestaoChefeResponseDTO
            {
                Id = sugestao.Id,
                DataSugestao = sugestao.DataSugestao,
                Periodo = sugestao.Periodo,
                NomeItem = sugestao.ItemCardapio!.Nome
            };
        }

        public async Task<bool> DeletarSugestaoDoChefe(int id)
        {
            var sugestao = await _context.SugestoesChefe.FindAsync(id);

            if (sugestao == null)
                throw new Exception("Sugestão não encontrada.");

            _context.SugestoesChefe.Remove(sugestao);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}