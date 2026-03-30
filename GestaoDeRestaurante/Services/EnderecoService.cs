using GestaoDeRestaurante.Data;
using GestaoDeRestaurante.DTOs.Endereco;
using GestaoDeRestaurante.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeRestaurante.Services
{
    public class EnderecoService
    {
        private readonly AppDbContext _context;

        public EnderecoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EnderecoResponseDTO> CriarEndereco(int usuarioId, EnderecoRequestDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);

            if (usuario == null)
                throw new Exception("Usuário não encontrado.");

            var endereco = new Endereco
            {
                Rua = dto.Rua,
                Numero = dto.Numero,
                Complemento = dto.Complemento,
                Bairro = dto.Bairro,
                Cidade = dto.Cidade,
                Estado = dto.Estado,
                Cep = dto.Cep,
                UsuarioId = usuarioId
            };

            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();

            return MapToResponse(endereco);
        }

        public async Task<List<EnderecoResponseDTO>> ListarEnderecoPorUsuario(int usuarioId)
        {
            var enderecos = await _context.Enderecos
                .Where(e => e.UsuarioId == usuarioId)
                .ToListAsync();

            return enderecos.Select(e => MapToResponse(e)).ToList();
        }

        public async Task<EnderecoResponseDTO> BuscarEnderecoPorId(int id)
        {
            var endereco = await _context.Enderecos
                .FirstOrDefaultAsync(e => e.Id == id);

            if (endereco == null)
                throw new Exception("Endereço não encontrado.");

            return MapToResponse(endereco);
        }

        public async Task<EnderecoResponseDTO> AtualizarEndereco(int id, EnderecoRequestDTO dto)
        {
            var endereco = await _context.Enderecos.FindAsync(id);

            if (endereco == null)
                throw new Exception("Endereço não encontrado.");

            endereco.Rua = dto.Rua;
            endereco.Numero = dto.Numero;
            endereco.Complemento = dto.Complemento;
            endereco.Bairro = dto.Bairro;
            endereco.Cidade = dto.Cidade;
            endereco.Estado = dto.Estado;
            endereco.Cep = dto.Cep;

            await _context.SaveChangesAsync();

            return MapToResponse(endereco);
        }

        public async Task<bool> DeletarEndereco(int id)
        {
            var endereco = await _context.Enderecos.FindAsync(id);

            if (endereco == null)
                throw new Exception("Endereço não encontrado.");

            _context.Enderecos.Remove(endereco);
            await _context.SaveChangesAsync();

            return true;
        }

        private EnderecoResponseDTO MapToResponse(Endereco endereco)
        {
            return new EnderecoResponseDTO
            {
                Id = endereco.Id,
                Rua = endereco.Rua,
                Numero = endereco.Numero,
                Complemento = endereco.Complemento,
                Bairro = endereco.Bairro,
                Cidade = endereco.Cidade,
                Estado = endereco.Estado,
                Cep = endereco.Cep
            };
        }
    }
}