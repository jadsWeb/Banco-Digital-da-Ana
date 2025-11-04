namespace BancoDigital.ContaCorrente.API.Aplicacao.Dtos
{
    public class MovimentacaoConta
    {
        public string? NumeroConta { get; set; }
        public decimal Valor { get; set; }
        public string? TipoMovimentacao { get; set; } // "CREDITO" ou "DEBITO"
    }
}