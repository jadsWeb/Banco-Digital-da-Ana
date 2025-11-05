using BancoDigital.ContaCorrente.API.Aplicacao.Dtos;
using BancoDigital.ContaCorrente.API.Aplicacao.Models;

namespace BancoDigital.ContaCorrente.API.Domain.Interfaces
{
    public interface IContaCorrenteService
    {
        public Task<string> CriarContaCorrenteAsync(CriacaoContaCorrente criacaoContaCorrente);
        public Task<string> EfetuarLoginAsync(LoginContaCorrente loginContaCorrente);
        Task<string> InativarContaAsync(string contaId, string senhaConta);
        Task<string> MovimentarContaAsync(string identificacao, MovimentacaoConta movimentacaoConta);
        Task<ContaCorrenteSaldo> ObterSaldoContaAsync(string identificacao);

        Task<bool> VerificarContaAtivaAsync(string identificacao);
    }
}