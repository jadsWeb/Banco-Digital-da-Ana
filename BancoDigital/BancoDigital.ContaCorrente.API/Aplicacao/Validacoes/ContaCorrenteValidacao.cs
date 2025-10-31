using FluentValidation;

public class CriarContaCorrenteValidacao : AbstractValidator<CriacaoContaCorrente>
{
    public CriarContaCorrenteValidacao()
    {
        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatorio.")
            .Must(ValidarCpf!).WithMessage("CPF invalido");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("A senha é obrigatória.");
    }

    private bool ValidarCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;
        cpf = new string(cpf.Where(char.IsDigit).ToArray());
        if (cpf.Length != 11) return false;
        // Cálculo de validação de CPF simplificado
        return !cpf.All(c => c == cpf[0]);
    }
}