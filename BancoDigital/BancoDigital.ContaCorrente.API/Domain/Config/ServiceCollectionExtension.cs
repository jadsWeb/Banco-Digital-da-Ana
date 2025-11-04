using System.Reflection;
using BancoDigital.ContaCorrente.API.Domain.Interfaces;
using BancoDigital.ContaCorrente.API.Domain.Services;
using FluentValidation;

namespace BancoDigital.ContaCorrente.API.Domain.Config
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServicesDomain(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddScoped<IContaCorrenteService, ContaCorrenteService>();
            return services;
        }
    }
}