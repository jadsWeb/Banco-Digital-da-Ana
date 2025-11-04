namespace BancoDigital.ContaCorrente.API.Domain.Interfaces
{
    public interface IIdentificacaoService
    {
        public string ObterIdentificacao(string cpf);
    }
}