using BancoDigital.ContaCorrente.API.Aplicacao.Models;

namespace BancoDigital.ContaCorrente.API.Domain.Interfaces
{
    public interface IContaCorrenteRepository
    {
        Task<bool> VerificarContaExistenteAsync(string nome);
        Task CriarContaCorrenteAsync(ContaCorrenteModel criacaoContaCorrente);
        Task<ContaCorrenteModel> ObterContaAsync(string identificacao);
        Task<ContaCorrenteModel> ObterContaPorIdAsync(string contaId);
        Task<int> InativarContaAsync(string contaId);

    }
}