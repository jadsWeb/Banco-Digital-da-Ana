using System.Data;
using BancoDigital.ContaCorrente.API.Aplicacao.Models;
using BancoDigital.ContaCorrente.API.Domain.Interfaces;
using Dapper;

namespace BancoDigital.ContaCorrente.API.Infra.Repositorys
{
    public class ContaCorrenteRepository(IDbConnection connection) : IContaCorrenteRepository
    {
        private readonly IDbConnection _connection = connection;
        
        public async Task CriarContaCorrenteAsync(ContaCorrenteModel conta)
        {
            var sql = @"INSERT INTO ContaCorrente (IdContaCorrente, Nome, Numero, Senha, Ativo, salt) 
            VALUES (@idConta, @Nome, @Numero, @Ativo, @Senha, @Salt)";

           
            var result = await _connection.ExecuteAsync(sql, new
            {
                conta
            });
        }

        public Task<bool> VerificarContaExistenteAsync(string nome)
        {
            var sql = "SELECT COUNT(1) FROM ContaCorrente WHERE Nome = @nome";
            try
            {
                var count = _connection.QueryFirstOrDefaultAsync<int>(sql, new { nome });
                return Task.FromResult(count.Result > 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar conta existente: {ex.Message}");
                return Task.FromResult(false);
            }
        }
    }
}