using GestaoDeRestaurante.Data;
using GestaoDeRestaurante.DTOs.Ingrediente;
using GestaoDeRestaurante.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeRestaurante.Services
{
    public class IngredienteService
    {
        private readonly AppDbContext _context;

        public IngredienteService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Ingrediente> CriarIngrediente(IngredienteRequestDTO dto)
        {
            var nome = NormalizarNome(dto.Nome);
            var descricao = NormalizarDescricao(dto.Descricao);

            var existe = await _context.Ingredientes.AnyAsync(i => i.Nome == nome);
            if (existe)
                throw new Exception("Já existe um ingrediente com esse nome.");

            var ingrediente = new Ingrediente
            {
                Nome = nome,
                Descricao = descricao
            };

            _context.Ingredientes.Add(ingrediente);
            await _context.SaveChangesAsync();

            return ingrediente;
        }

        public async Task<List<Ingrediente>> ListarIngredientes()
        {
            return await _context.Ingredientes.OrderBy(i => i.Nome).ToListAsync();
        }

        public async Task<Ingrediente> BuscarIngredientePorId(int id)
        {
            var ingrediente = await _context.Ingredientes.FindAsync(id);
            if (ingrediente == null)
                throw new Exception("Ingrediente não encontrado.");

            return ingrediente;
        }

        public async Task<Ingrediente> AtualizarIngrediente(int id, IngredienteRequestDTO dto)
        {
            var ingrediente = await _context.Ingredientes.FindAsync(id);
            if (ingrediente == null)
                throw new Exception("Ingrediente não encontrado.");

            var nome = NormalizarNome(dto.Nome);
            var descricao = NormalizarDescricao(dto.Descricao);

            var existe = await _context.Ingredientes.AnyAsync(i => i.Id != id && i.Nome == nome);
            if (existe)
                throw new Exception("Já existe um ingrediente com esse nome.");

            ingrediente.Nome = nome;
            ingrediente.Descricao = descricao;
            await _context.SaveChangesAsync();

            return ingrediente;
        }

        public async Task<bool> DeletarIngrediente(int id)
        {
            var ingrediente = await _context.Ingredientes.FindAsync(id);
            if (ingrediente == null)
                throw new Exception("Ingrediente não encontrado.");

            _context.Ingredientes.Remove(ingrediente);
            await _context.SaveChangesAsync();
            return true;
        }

        private static string NormalizarNome(string? nome)
        {
            var nomeNormalizado = nome?.Trim();
            if (string.IsNullOrWhiteSpace(nomeNormalizado))
                throw new Exception("O nome do ingrediente é obrigatório.");

            return nomeNormalizado;
        }

        private static string? NormalizarDescricao(string? descricao)
        {
            var descricaoNormalizada = descricao?.Trim();
            return string.IsNullOrWhiteSpace(descricaoNormalizada) ? null : descricaoNormalizada;
        }
    }
}
