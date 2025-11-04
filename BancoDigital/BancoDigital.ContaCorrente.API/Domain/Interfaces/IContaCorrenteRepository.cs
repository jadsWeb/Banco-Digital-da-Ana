using BancoDigital.ContaCorrente.API.Aplicacao.Models;

namespace BancoDigital.ContaCorrente.API.Domain.Interfaces
{
    public interface IContaCorrenteRepository
    {
        public Task<bool> VerificarContaExistenteAsync(string nome);
        public Task CriarContaCorrenteAsync(ContaCorrenteModel criacaoContaCorrente);
    }
}