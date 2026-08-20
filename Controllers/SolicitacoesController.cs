using Microsoft.AspNetCore.Mvc;
using UbecSolicitacoes.DTOs;
using UbecSolicitacoes.Services;

namespace UbecSolicitacoes.Controllers
{
    [ApiController]
    [Route("api/solicitacoes")]
    public class SolicitacoesController : ControllerBase
    {
        private readonly SolicitacaoService _service;

        public SolicitacoesController(SolicitacaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Listar([FromQuery] string? status, [FromQuery] bool? apenasAtrasadas)
        {
            var resultado = await _service.ListarAsync(status, apenasAtrasadas);
            return Ok(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] SolicitacaoCreateDTO dto)
        {
            try
            {
                var solicitacao = await _service.CriarAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = solicitacao.Id }, solicitacao);
            }
            catch (BusinessException ex)
            {
                return StatusCode(ex.StatusCode, new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(int id, [FromBody] SolicitacaoStatusUpdateDTO dto)
        {
            try
            {
                await _service.AtualizarStatusAsync(id, dto);
                return NoContent();
            }
            catch (BusinessException ex)
            {
                return StatusCode(ex.StatusCode, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok();
        }
    }
}