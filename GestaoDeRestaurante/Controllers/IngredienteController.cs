using GestaoDeRestaurante.DTOs.Ingrediente;
using GestaoDeRestaurante.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoDeRestaurante.Controllers
{
    [ApiController]
    [Route("api/ingrediente")]
    [Authorize(Roles = "Administrador")]
    public class IngredienteController : ControllerBase
    {
        private readonly IngredienteService _service;

        public IngredienteController(IngredienteService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CriarIngrediente([FromBody] IngredienteRequestDTO dto)
        {
            try
            {
                return Ok(await _service.CriarIngrediente(dto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListarIngredientes()
        {
            try
            {
                return Ok(await _service.ListarIngredientes());
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarIngredientePorId(int id)
        {
            try
            {
                return Ok(await _service.BuscarIngredientePorId(id));
            }
            catch (Exception ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarIngrediente(int id, [FromBody] IngredienteRequestDTO dto)
        {
            try
            {
                return Ok(await _service.AtualizarIngrediente(id, dto));
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarIngrediente(int id)
        {
            try
            {
                await _service.DeletarIngrediente(id);
                return Ok(new { mensagem = "Ingrediente removido com sucesso." });
            }
            catch (Exception ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
        }
    }
}
