using BancoDigital.ContaCorrente.API.Infra.Seguranca;

namespace BancoDigital.ContaCorrente.API.Domain.Interfaces
{
    public interface IJwtService
    {
        string GerarToken(string idContaCorrente, string NumeroConta, JwtConfiguracoes _jwtConfiguracoes);
    }
}