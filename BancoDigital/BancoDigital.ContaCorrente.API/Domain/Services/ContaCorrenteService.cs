using BancoDigital.ContaCorrente.API.Aplicacao.Dtos;
using BancoDigital.ContaCorrente.API.Aplicacao.Models;
using BancoDigital.ContaCorrente.API.Domain.Interfaces;
using BancoDigital.ContaCorrente.API.Infra.Config;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BancoDigital.ContaCorrente.API.Domain.Services
{
    public class ContaCorrenteService(IIdentificacaoService identificacaoService, IContaCorrenteRepository contaCorrenteRepository) : IContaCorrenteService
    {
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
            //await _contaCorrenteRepository.CriarContaCorrenteAsync(conta);
            return conta.Numero.ToString();
        }
        public Task<string> EfetuarLoginAsync(LoginContaCorrente loginContaCorrente)
        {
            throw new NotImplementedException();
        }
    }
}