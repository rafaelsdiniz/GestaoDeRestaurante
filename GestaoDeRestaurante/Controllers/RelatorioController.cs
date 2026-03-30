using GestaoDeRestaurante.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeRestaurante.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioController : ControllerBase
    {
        private readonly RelatorioService _relatorioService;

        public RelatorioController(RelatorioService relatorioService)
        {
            _relatorioService = relatorioService;
        }

        /// <summary>
        /// Faturamento total por tipo de atendimento em determinado período.
        /// </summary>
        [HttpGet("faturamento")]
        public async Task<IActionResult> FaturamentoPorTipoAtendimento(
            [FromQuery] DateTime dataInicio,
            [FromQuery] DateTime dataFim)
        {
            try
            {
                if (dataFim < dataInicio)
                    return BadRequest(new { mensagem = "A data de fim deve ser maior ou igual à data de início." });

                var resultado = await _relatorioService.FaturamentoPorTipoAtendimento(dataInicio, dataFim);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        /// <summary>
        /// Itens mais vendidos, com indicação se são Sugestão do Chefe.
        /// </summary>
        [HttpGet("itens-mais-vendidos")]
        public async Task<IActionResult> ItensMaisVendidos()
        {
            try
            {
                var resultado = await _relatorioService.ItensMaisVendidos();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }
    }
}
