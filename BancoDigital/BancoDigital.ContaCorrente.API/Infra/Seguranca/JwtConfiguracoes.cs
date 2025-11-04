namespace BancoDigital.ContaCorrente.API.Infra.Seguranca
{
    public class JwtConfiguracoes
    {
        public string Secret { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; } = 60;
        public string Issuer { get; set; } = "BankMore";
        public string Audience { get; set; } = "BankMoreClients";
    }
}