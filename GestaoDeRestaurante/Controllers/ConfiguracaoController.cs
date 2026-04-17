using GestaoDeRestaurante.Data;
using GestaoDeRestaurante.DTOs.Configuracao;
using GestaoDeRestaurante.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeRestaurante.Controllers
{
    [Route("api")]
    [ApiController]
    public class ConfiguracaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConfiguracaoController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna a configuração atual do restaurante (admin autenticado)
        /// </summary>
        [HttpGet("admin/configuracoes")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ConfiguracaoRestauranteDTO>> GetConfiguracoes()
        {
            var config = await ObterOuCriarConfiguracaoAsync();

            return Ok(new ConfiguracaoRestauranteDTO
            {
                AlmocoInicio = config.AlmocoInicio,
                AlmocoFim = config.AlmocoFim,
                JantarInicio = config.JantarInicio,
                JantarFim = config.JantarFim,
                ReservaInicio = config.ReservaInicio,
                ReservaFim = config.ReservaFim,
                AntecedenciaMinimaDias = config.AntecedenciaMinimaDias
            });
        }

        /// <summary>
        /// Atualiza a configuração do restaurante (admin autenticado)
        /// </summary>
        [HttpPut("admin/configuracoes")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AtualizarConfiguracoes([FromBody] ConfiguracaoRestauranteDTO dto)
        {
            if (!TimeSpan.TryParse(dto.AlmocoInicio, out var almocoInicio) ||
                !TimeSpan.TryParse(dto.AlmocoFim, out var almocoFim))
                return BadRequest(new { mensagem = "Horários de almoço inválidos." });

            if (almocoInicio >= almocoFim)
                return BadRequest(new { mensagem = "O horário de início do almoço deve ser anterior ao fim." });

            if (!TimeSpan.TryParse(dto.JantarInicio, out var jantarInicio) ||
                !TimeSpan.TryParse(dto.JantarFim, out var jantarFim))
                return BadRequest(new { mensagem = "Horários de jantar inválidos." });

            if (jantarInicio >= jantarFim)
                return BadRequest(new { mensagem = "O horário de início do jantar deve ser anterior ao fim." });

            if (!TimeSpan.TryParse(dto.ReservaInicio, out var reservaInicio) ||
                !TimeSpan.TryParse(dto.ReservaFim, out var reservaFim))
                return BadRequest(new { mensagem = "Horários de reserva inválidos." });

            if (reservaInicio >= reservaFim)
                return BadRequest(new { mensagem = "O horário de início da reserva deve ser anterior ao fim." });

            var config = await ObterOuCriarConfiguracaoAsync();

            config.AlmocoInicio = dto.AlmocoInicio;
            config.AlmocoFim = dto.AlmocoFim;
            config.JantarInicio = dto.JantarInicio;
            config.JantarFim = dto.JantarFim;
            config.ReservaInicio = dto.ReservaInicio;
            config.ReservaFim = dto.ReservaFim;
            config.AntecedenciaMinimaDias = dto.AntecedenciaMinimaDias;

            await _context.SaveChangesAsync();

            return Ok(new { mensagem = "Configurações atualizadas com sucesso." });
        }

        /// <summary>
        /// Endpoint público — retorna os horários disponíveis para reserva
        /// </summary>
        [HttpGet("configuracoes/horarios")]
        [AllowAnonymous]
        public async Task<ActionResult<HorariosPublicosDTO>> GetHorariosPublicos()
        {
            var config = await ObterOuCriarConfiguracaoAsync();

            return Ok(new HorariosPublicosDTO
            {
                AlmocoInicio = config.AlmocoInicio,
                AlmocoFim = config.AlmocoFim,
                JantarInicio = config.JantarInicio,
                JantarFim = config.JantarFim,
                ReservaInicio = config.ReservaInicio,
                ReservaFim = config.ReservaFim,
                AntecedenciaMinimaDias = config.AntecedenciaMinimaDias
            });
        }

        private async Task<ConfiguracaoRestaurante> ObterOuCriarConfiguracaoAsync()
        {
            var config = await _context.ConfiguracoesRestaurante.FirstOrDefaultAsync();
            if (config == null)
            {
                config = new ConfiguracaoRestaurante();
                _context.ConfiguracoesRestaurante.Add(config);
                await _context.SaveChangesAsync();
            }
            return config;
        }
    }
}
