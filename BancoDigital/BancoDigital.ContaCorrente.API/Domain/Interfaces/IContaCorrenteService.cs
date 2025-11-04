using BancoDigital.ContaCorrente.API.Aplicacao.Dtos;

namespace BancoDigital.ContaCorrente.API.Domain.Interfaces
{
    public interface IContaCorrenteService
    {
        public Task<string> CriarContaCorrenteAsync(CriacaoContaCorrente criacaoContaCorrente);
        public Task<string> EfetuarLoginAsync(LoginContaCorrente loginContaCorrente);
    }
}