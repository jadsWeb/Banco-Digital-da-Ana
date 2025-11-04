using System.Data;
using System.Reflection;
using BancoDigital.ContaCorrente.API.Domain.Interfaces;
using BancoDigital.ContaCorrente.API.Infra.Repositorys;
using FluentValidation;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace BancoDigital.ContaCorrente.API.Infra.Config
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServicesInfra(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbConnection>(sp =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                var connection = new SqliteConnection(connectionString);
                connection.Open(); 
                return connection;
            });
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
            return services;
        }
    }
}