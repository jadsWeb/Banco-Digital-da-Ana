public class Movimento
{
    public string IdMovimento { get; set; } = Guid.NewGuid().ToString();
    public string? IdContaCorrente { get; set; }
    public DateTime DataMovimento { get; set; }
    public string? TipoMovimento { get; set; }
    public decimal Valor { get; set; }
}