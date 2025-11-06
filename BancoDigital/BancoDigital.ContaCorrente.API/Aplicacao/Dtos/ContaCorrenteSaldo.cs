namespace BancoDigital.ContaCorrente.API.Aplicacao.Dtos
{
    public class ContaCorrenteSaldo
    {
        public string? NumeroConta { get; set; }
        public string? Nome { get; set; }
        public decimal SaldoAtual { get; set; }
        public DateTime DataHoraResposta { get; set; }
    }
}