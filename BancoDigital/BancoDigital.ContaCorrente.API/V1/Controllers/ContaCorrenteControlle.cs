using System.Threading.Tasks;
using BancoDigital.ContaCorrente.API.Aplicacao.Dtos;
using BancoDigital.ContaCorrente.API.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BancoDigital.ContaCorrente.API.V1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContaCorrenteController(IMediator mediator, IContaCorrenteService contaCorrenteService) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IContaCorrenteService _contaCorrenteService = contaCorrenteService;
        
        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarContaCorrente([FromBody] CriacaoContaCorrente criacaoContaCorrente)
        {
            var validador = new CriarContaCorrenteValidacao();
            var resultadoValidacao = validador.Validate(criacaoContaCorrente);
            if (!resultadoValidacao.IsValid)
            {
                var erros = resultadoValidacao.Errors.Select(e => e.ErrorMessage);
                if (erros.Contains("CPF_INVALIDO"))
                    return BadRequest("INVALID_DOCUMENT");
                return BadRequest("INVALID_REQUEST");
            }
            var result = await _contaCorrenteService.CriarContaCorrenteAsync(criacaoContaCorrente);
            if (result == "Conta corrente já existente.")
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> EfetuarLogin([FromBody] LoginContaCorrente loginContaCorrente)
        {
            var result = await _mediator.Send(loginContaCorrente);
            result = await _contaCorrenteService.EfetuarLoginAsync(loginContaCorrente);
            return Ok(result);
        }
    }
}