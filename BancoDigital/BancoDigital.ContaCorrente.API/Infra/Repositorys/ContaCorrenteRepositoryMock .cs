using BancoDigital.ContaCorrente.API.Aplicacao.Models;
using BancoDigital.ContaCorrente.API.Domain.Interfaces;

namespace BancoDigital.ContaCorrente.API.Infra.Repositorys
{
    public class ContaCorrenteRepositoryMock : IContaCorrenteRepository
    {
        private readonly List<ContaCorrenteModel> _contasExistentes = new()
        {
            new ContaCorrenteModel
            {
                IdContaCorrente = Guid.NewGuid().ToString(),
                Nome = "João da Silva",
                Numero = new Random().Next(10000000, 99999999),
                Senha = "",
                Ativo = true,
                Salt = ""
            },
            new ContaCorrenteModel
            {
                IdContaCorrente = Guid.NewGuid().ToString(),
                Nome = "Maria Oliveira",
                Numero = new Random().Next(10000000, 99999999),
                Senha = "",
                Ativo = true,
                Salt = ""
            },
            new ContaCorrenteModel
            {
                IdContaCorrente = Guid.NewGuid().ToString(),
                Nome = "Ana Souza",
                Numero = new Random().Next(10000000, 99999999),
                Senha = "",
                Ativo = true,
                Salt = ""
            }
        };


        public Task<IEnumerable<ContaCorrenteModel>> GetAllAsync()
        {
            return Task.FromResult(_contasExistentes.AsEnumerable());
        }

        public Task<bool> VerificarContaExistenteAsync(string nome)
        {
            var contaExistente = _contasExistentes.Any(c => c.Nome == nome);
            return Task.FromResult(contaExistente);
        }

        public Task CriarContaCorrenteAsync(ContaCorrenteModel criacaoContaCorrente)
        {
            throw new NotImplementedException();
        }
    }
}