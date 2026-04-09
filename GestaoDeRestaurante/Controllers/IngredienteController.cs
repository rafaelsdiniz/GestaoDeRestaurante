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
        public async Task<IActionResult> CriarIngrediente([FromBody] string nome)
            => Ok(await _service.CriarIngrediente(nome));

        [HttpGet]
        public async Task<IActionResult> ListarIngredientes()
            => Ok(await _service.ListarIngredientes());

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarIngredientePorId(int id)
            => Ok(await _service.BuscarIngredientePorId(id));

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarIngrediente(int id, [FromBody] string nome)
            => Ok(await _service.AtualizarIngrediente(id, nome));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarIngrediente(int id)
            => Ok(await _service.DeletarIngrediente(id));
    }
}
