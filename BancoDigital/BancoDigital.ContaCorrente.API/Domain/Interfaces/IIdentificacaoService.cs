using BancoDigital.ContaCorrente.API.Aplicacao.Dtos;

namespace BancoDigital.ContaCorrente.API.Domain.Interfaces
{
    public interface IIdentificacaoService
    {
        string ObterIdentificacao(string cpf);
        object ValidarConta(LoginContaCorrente loginContaCorrente);
    }
}