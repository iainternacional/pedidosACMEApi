using EnvioPedidosAcme.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace EnvioPedidosAcme.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddHttpClient();
        services.AddScoped<ISoapEnvioPedidosClient, Soap.SoapEnvioPedidosClient>();
        return services;
    }
}
