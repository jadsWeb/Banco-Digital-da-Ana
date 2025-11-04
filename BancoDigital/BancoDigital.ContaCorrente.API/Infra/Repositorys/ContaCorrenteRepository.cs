using System.Data;
using System.Runtime.InteropServices;
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
            var sql = @"INSERT INTO ContaCorrente (IdContaCorrente, Nome, Cpf, Numero, Senha, Ativo, salt) 
            VALUES (@idConta, @Nome, @Cpf, @Numero, @Senha, @Ativo, @Salt)";
            try
            {
                var result = await _connection.ExecuteAsync(sql, new
                {
                    idConta = conta.IdContaCorrente,
                    conta.Nome,
                    conta.Cpf,
                    conta.Numero,
                    conta.Senha,
                    Ativo = conta.Ativo ? 1 : 0,
                    conta.Salt
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar conta corrente: {ex.Message}");
            }
        }

        public async Task<ContaCorrenteModel> ObterContaAsync(string identificacao)
        {
            var sql = "";
            if (identificacao.Length == 11)
                sql = "SELECT * FROM ContaCorrente WHERE Cpf = @identificacao";
            else
                sql = "SELECT * FROM ContaCorrente WHERE Numero = @identificacao";
            try
            {
                var conta = await _connection.QueryFirstOrDefaultAsync<ContaCorrenteModel>(sql, new { identificacao });
                return conta!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao efetuar login: {ex.Message}");
                return null!;
            }
        }

        public async Task<int> InativarContaAsync(string contaId)
        {
            var sql = "UPDATE ContaCorrente SET Ativo = 0 WHERE IdContaCorrente = @contaId";
            try
            {
                return await _connection.ExecuteAsync(sql, new { contaId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao inativar conta: {ex.Message}");
                return 0;
            }
        }

        public async Task<ContaCorrenteModel> ObterContaPorIdAsync(string contaId)
        {
            var sql = "SELECT * FROM ContaCorrente WHERE IdContaCorrente = @contaId";
            try
            {
                var conta = await _connection.QueryFirstOrDefaultAsync<ContaCorrenteModel>(sql, new { contaId });
                return conta!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter conta por ID: {ex.Message}");
                return null!;
            }
        }

        public async Task<bool> VerificarContaExistenteAsync(string nome)
        {
            var sql = "SELECT COUNT(1) FROM ContaCorrente WHERE Nome = @nome";
            try
            {
                var count = await _connection.QueryFirstOrDefaultAsync<int>(sql, new { nome });
                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao verificar conta existente: {ex.Message}");
                return false;
            }
        }
    }
}