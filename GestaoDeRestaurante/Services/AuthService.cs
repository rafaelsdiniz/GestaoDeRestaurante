using GestaoDeRestaurante.Data;
using GestaoDeRestaurante.DTOs.Auth;
using GestaoDeRestaurante.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GestaoDeRestaurante.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.Senha))
                throw new Exception("Email ou senha inválidos.");

            var token = GerarToken(usuario);

            return new LoginResponseDTO
            {
                Token = token,
                NomeUsuario = usuario.Nome,
                Email = usuario.Email,
                UsuarioId = usuario.Id,
                TipoUsuario = usuario.TipoUsuario.ToString()
            };
        }

        private string GerarToken(Usuario usuario)
        {
            var chave = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Chave JWT não configurada.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.TipoUsuario.ToString())
            };

            var expiresInHours = _configuration.GetValue<int>("Jwt:ExpiresInHours", 24);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expiresInHours),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
