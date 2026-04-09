using GestaoDeRestaurante.Data;
using GestaoDeRestaurante.DTOs.Mesa;
using GestaoDeRestaurante.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeRestaurante.Services
{
    public class MesaService
    {
        private readonly AppDbContext _context;

        public MesaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MesaResponseDTO> CriarMesa(MesaRequestDTO dto)
        {
            var existe = await _context.Mesas.AnyAsync(m => m.Numero == dto.Numero);
            if (existe)
                throw new Exception("Já existe uma mesa com esse número.");

            var mesa = new Mesa
            {
                Numero = dto.Numero,
                Capacidade = dto.Capacidade
            };

            _context.Mesas.Add(mesa);
            await _context.SaveChangesAsync();

            return MapToResponse(mesa);
        }

        public async Task<List<MesaResponseDTO>> ListarMesas()
        {
            var mesas = await _context.Mesas.OrderBy(m => m.Numero).ToListAsync();
            return mesas.Select(MapToResponse).ToList();
        }

        public async Task<List<MesaResponseDTO>> ListarMesasDisponiveis()
        {
            var mesas = await _context.Mesas.OrderBy(m => m.Numero).ToListAsync();
            return mesas.Select(MapToResponse).ToList();
        }

        public async Task<MesaResponseDTO> BuscarMesaPorId(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null)
                throw new Exception("Mesa não encontrada.");

            return MapToResponse(mesa);
        }

        public async Task<MesaResponseDTO> AtualizarMesa(int id, MesaRequestDTO dto)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null)
                throw new Exception("Mesa não encontrada.");

            mesa.Numero = dto.Numero;
            mesa.Capacidade = dto.Capacidade;
            await _context.SaveChangesAsync();

            return MapToResponse(mesa);
        }

        public async Task<bool> DeletarMesa(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null)
                throw new Exception("Mesa não encontrada.");

            _context.Mesas.Remove(mesa);
            await _context.SaveChangesAsync();
            return true;
        }

        private static MesaResponseDTO MapToResponse(Mesa mesa)
        {
            return new MesaResponseDTO
            {
                Id = mesa.Id,
                Numero = mesa.Numero,
                Capacidade = mesa.Capacidade
            };
        }
    }
}
