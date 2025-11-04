using BancoDigital.ContaCorrente.API.Aplicacao.Dtos;
using BancoDigital.ContaCorrente.API.Aplicacao.Models;
using BancoDigital.ContaCorrente.API.Domain.Interfaces;
using BancoDigital.ContaCorrente.API.Infra.Config;
using BancoDigital.ContaCorrente.API.Infra.Seguranca;

namespace BancoDigital.ContaCorrente.API.Domain.Services
{
    public class ContaCorrenteService(IIdentificacaoService identificacaoService,
        IJwtService jwtService,
        IConfiguration configuration,
        IContaCorrenteRepository contaCorrenteRepository) : IContaCorrenteService
    {
        private readonly IJwtService _jwtService = jwtService;
        private readonly IIdentificacaoService _identificacaoService = identificacaoService;
        private readonly IContaCorrenteRepository _contaCorrenteRepository = contaCorrenteRepository;
        public async Task<string> CriarContaCorrenteAsync(CriacaoContaCorrente criacaoContaCorrente)
        {
            var identificacacao = _identificacaoService.ObterIdentificacao(criacaoContaCorrente.Cpf!);
            if (await _contaCorrenteRepository.VerificarContaExistenteAsync(identificacacao))
            {
                return "Conta corrente já existente.";
            }
            var (hashSenha, salt) = SenhaHasher.HashPassword(criacaoContaCorrente.Senha!);
            var conta = new ContaCorrenteModel()
            {
                IdContaCorrente = Guid.NewGuid().ToString(),
                Nome = identificacacao,
                Numero = new Random().Next(10000000, 99999999),
                Senha = hashSenha,
                Ativo = true,
                Salt = salt
            };
            await _contaCorrenteRepository.CriarContaCorrenteAsync(conta);
            return conta.Numero.ToString();
        }
        public async Task<string> EfetuarLoginAsync(LoginContaCorrente loginContaCorrente)
        {
            var conta = new ContaCorrenteModel();
            var (hashArmazenado, salt) = SenhaHasher.HashPassword(loginContaCorrente.Senha!);
            if (SenhaHasher.VerifyPassword(loginContaCorrente.Senha!, hashArmazenado, salt))
            {
                conta = await _contaCorrenteRepository.EfetuarLoginAsync(hashArmazenado);
                var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtConfiguracoes>();
               return _jwtService.GerarToken(conta.IdContaCorrente!, conta.Nome!, jwtSettings!);
            }
            return "USER_UNAUTHORIZED.";
        }
    }
}