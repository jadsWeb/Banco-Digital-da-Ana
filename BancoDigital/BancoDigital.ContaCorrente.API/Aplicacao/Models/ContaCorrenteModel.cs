namespace BancoDigital.ContaCorrente.API.Aplicacao.Models
{
    public class ContaCorrenteModel
    {
        public string? IdContaCorrente { get; set; }  // TEXT(37)
        public int Numero { get; set; }              // INTEGER(10)
        public string? Cpf { get; set; }              // TEXT(11)
        public string? Nome { get; set; }             // TEXT(100)
        public bool Ativo { get; set; }              // INTEGER(1)
        public string? Senha { get; set; }            // TEXT(100)
        public string? Salt { get; set; }
    }
}