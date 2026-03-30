using GestaoDeRestaurante.Data;
using GestaoDeRestaurante.DTOs.Usuario;
using GestaoDeRestaurante.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeRestaurante.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioResponseDTO> CriarUsuario(UsuarioRequestDTO dto)
        {
            var emailExiste = await _context.Usuarios
                .AnyAsync(u => u.Email == dto.Email);

            if (emailExiste)
                throw new Exception("Email já cadastrado.");

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(dto.Senha)
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return MapToResponse(usuario);
        }

        public async Task<List<UsuarioResponseDTO>> ListarUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return usuarios.Select(MapToResponse).ToList();
        }

        public async Task<UsuarioResponseDTO> BuscarUsuarioPorId(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                throw new Exception("Usuário não encontrado.");

            return MapToResponse(usuario);
        }

        public async Task<UsuarioResponseDTO> AtualizarUsuario(int id, UsuarioRequestDTO dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                throw new Exception("Usuário não encontrado.");

            var emailExiste = await _context.Usuarios
                .AnyAsync(u => u.Email == dto.Email && u.Id != id);

            if (emailExiste)
                throw new Exception("Email já está em uso.");

            usuario.Nome = dto.Nome;
            usuario.Email = dto.Email;
            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            await _context.SaveChangesAsync();

            return MapToResponse(usuario);
        }

        public async Task<bool> DeletarUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                throw new Exception("Usuário não encontrado.");

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return true;
        }

        private UsuarioResponseDTO MapToResponse(Usuario usuario)
        {
            return new UsuarioResponseDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };
        }
    }
}
