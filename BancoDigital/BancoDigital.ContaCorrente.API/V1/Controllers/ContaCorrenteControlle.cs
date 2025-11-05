using BancoDigital.ContaCorrente.API.Aplicacao.Dtos;
using BancoDigital.ContaCorrente.API.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BancoDigital.ContaCorrente.API.V1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContaCorrenteController(IContaCorrenteService contaCorrenteService) : ControllerBase
    {
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
        [HttpPut("inativarConta")]
        public async Task<IActionResult> InativarConta(string SenhaConta)
        {
            var contaId = User.Claims.FirstOrDefault(c => c.Type == "IdConta")?.Value;
            var result = "";
            if ((result = await _contaCorrenteService.InativarContaAsync(contaId!, SenhaConta)) == "SUCESSO")
                return NoContent();
            return BadRequest(result);
        }

        [Authorize]
        [HttpPost("movimentacoes")]
        public async Task<IActionResult> MovimentarConta(MovimentacaoConta movimentacaoConta)
        {
            var identificacao = movimentacaoConta.NumeroConta;
            if (string.IsNullOrEmpty(identificacao))
                identificacao = User.Claims.FirstOrDefault(c => c.Type == "IdConta")?.Value;
            if (movimentacaoConta.Valor <= 0)
                return BadRequest(": INVALID_VALUE.");
            if (movimentacaoConta.TipoMovimentacao != "C" && movimentacaoConta.TipoMovimentacao != "D")
                return BadRequest(":INVALID_TYPE.");
            var result = await _contaCorrenteService.VerificarContaAtivaAsync(identificacao!);
            if (!result)
                return BadRequest(": INACTIVE_ACCOUNT.");
            var resultMovimentacao = await _contaCorrenteService.MovimentarContaAsync(identificacao!, movimentacaoConta);
            if (resultMovimentacao != "SUCESSO.")
                return BadRequest(resultMovimentacao);
            return NoContent();
        }

        [Authorize]
        [HttpGet("saldo")]
        public async Task<IActionResult> ObterSaldoConta()
        {
            var identificacao = User.Claims.FirstOrDefault(c => c.Type == "IdConta")?.Value;
            var contaCorrente = await _contaCorrenteService.ObterSaldoContaAsync(identificacao!);
            return Ok(contaCorrente);
        }
    }
}