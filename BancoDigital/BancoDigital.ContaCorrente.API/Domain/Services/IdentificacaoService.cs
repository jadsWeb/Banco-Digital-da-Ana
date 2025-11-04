using BancoDigital.ContaCorrente.API.Domain.Interfaces;

namespace BancoDigital.ContaCorrente.API.Domain.Services
{
    public class IdentificacaoService : IIdentificacaoService
    {
        public string ObterIdentificacao(string cpf)
        {
            var cpfUsar = cpf.Replace(".", "").Replace("-", "");
            if (cpfUsar == "77616410057")
            {
                return "João da Silva";
            }
            if (cpfUsar == "68807841002")
            {
                return "Maria Oliveira";
            }
            if (cpfUsar == "23140134096")
            {
                return "Ana Souza";
            }
            return "Nome não encontrado";
        }
    }
}