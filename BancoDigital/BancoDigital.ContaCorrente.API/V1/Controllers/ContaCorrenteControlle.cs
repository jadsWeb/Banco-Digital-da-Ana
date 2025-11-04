using System.Threading.Tasks;
using BancoDigital.ContaCorrente.API.Aplicacao.Dtos;
using BancoDigital.ContaCorrente.API.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
                return BadRequest("INVALID_DOCUMENT");
            }
            var result = await _contaCorrenteService.CriarContaCorrenteAsync(criacaoContaCorrente);
            if (result == "Conta corrente já existente.")
                return BadRequest(result);
            return Ok(result);
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> EfetuarLogin([FromBody] LoginContaCorrente loginContaCorrente)
        {
            if (string.IsNullOrEmpty(loginContaCorrente.Cpf) && string.IsNullOrEmpty(loginContaCorrente.NumeroConta))
                return BadRequest("INVALID_REQUEST");
            var result = await _contaCorrenteService.EfetuarLoginAsync(loginContaCorrente);
            if (result == "USER_UNAUTHORIZED.")
                return Unauthorized();
            return Ok(result);
        }

        [Authorize]
        [HttpPut("InativarConta")]
        public async Task<IActionResult> InativarConta(string SenhaConta)
        {
            var contaId = User.Claims.FirstOrDefault(c => c.Type == "IdConta")?.Value;
            var result = "";
            if ((result = await _contaCorrenteService.InativarContaAsync(contaId!, SenhaConta)) == "SUCESSO")
                return NoContent();
            return BadRequest(result);
        }
    }
}