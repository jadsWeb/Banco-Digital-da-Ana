using BancoDigital.ContaCorrente.API.Aplicacao.Dtos;
using BancoDigital.ContaCorrente.API.Aplicacao.Models;
using BancoDigital.ContaCorrente.API.Domain.Interfaces;
using BancoDigital.ContaCorrente.API.Infra.Config;
using BancoDigital.ContaCorrente.API.Infra.Seguranca;

namespace BancoDigital.ContaCorrente.API.Domain.Services
{
    public class ContaCorrenteService(IJwtService jwtService,
        IConfiguration configuration,
        IContaCorrenteRepository contaCorrenteRepository) : IContaCorrenteService
    {
        private readonly IJwtService _jwtService = jwtService;
        private readonly IContaCorrenteRepository _contaCorrenteRepository = contaCorrenteRepository;
        public async Task<string> CriarContaCorrenteAsync(CriacaoContaCorrente criacaoContaCorrente)
        {
            if (await _contaCorrenteRepository.VerificarContaExistenteAsync(criacaoContaCorrente.Nome!))
                return "Conta corrente já existente.";
            var (hashSenha, salt) = SenhaHasher.HashPassword(criacaoContaCorrente.Senha!);
            var conta = new ContaCorrenteModel()
            {
                IdContaCorrente = Guid.NewGuid().ToString(),
                Nome = criacaoContaCorrente.Nome!,
                Cpf = criacaoContaCorrente.Cpf!.Replace(".", "").Replace("-", ""),
                Numero = new Random().Next(10000000, 99999999),
                Senha = hashSenha,
                Ativo = true,
                Salt = salt
            };
            await _contaCorrenteRepository.CriarContaCorrenteAsync(conta);
            return $"Conta criada {conta.Numero}";
        }
        public async Task<string> EfetuarLoginAsync(LoginContaCorrente loginContaCorrente)
        {
            var identificacao = "";
            if (!string.IsNullOrEmpty(loginContaCorrente.NumeroConta))
                identificacao = loginContaCorrente.NumeroConta;
            else
                identificacao = loginContaCorrente.Cpf!.Replace(".", "").Replace("-", "");
            var conta = await _contaCorrenteRepository.ObterContaAsync(identificacao!);
            if (conta == null)
                return "USER_UNAUTHORIZED.";
            var senhaValida = SenhaHasher.VerifyPassword(loginContaCorrente.Senha!, conta.Senha!, conta.Salt!);
            if (!senhaValida)
                return "USER_UNAUTHORIZED.";
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtConfiguracoes>();
            return _jwtService.GerarToken(conta.IdContaCorrente!, conta.Nome!, jwtSettings!);
        }

        public async Task<string> InativarContaAsync(string contaId, string senhaConta)
        {
            var conta = await _contaCorrenteRepository.ObterContaPorIdAsync(contaId);
            if (conta == null)
                return "CONTA_NAO_ENCONTRADA.";
            var senhaValida = SenhaHasher.VerifyPassword(senhaConta, conta.Senha!, conta.Salt!);
            if (!senhaValida)
                return "SENHA_INVALIDA.";
            var result = await _contaCorrenteRepository.InativarContaAsync(contaId);
            if(result == 0)
                return "FALHA_AO_INATIVAR_CONTA.";
            return "SUCESSO.";  
        }

    }
}