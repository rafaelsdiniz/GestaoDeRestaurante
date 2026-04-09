using GestaoDeRestaurante.Data;
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

        public async Task<Ingrediente> CriarIngrediente(string nome)
        {
            var existe = await _context.Ingredientes.AnyAsync(i => i.Nome == nome);
            if (existe)
                throw new Exception("Já existe um ingrediente com esse nome.");

            var ingrediente = new Ingrediente { Nome = nome };
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

        public async Task<Ingrediente> AtualizarIngrediente(int id, string nome)
        {
            var ingrediente = await _context.Ingredientes.FindAsync(id);
            if (ingrediente == null)
                throw new Exception("Ingrediente não encontrado.");

            ingrediente.Nome = nome;
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
    }
}
